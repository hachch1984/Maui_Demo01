using DbEf;
using Dto;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.Util;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ges.User
{

    public class Login : CmdBase, IRequest<Login>
    {
        protected Token_Dto_For_Create Dto { get; }
        public Dto.Token_Dto_For_ShowInformation Result { get; protected set; } = null!;
        protected IConfiguration Configuration { get; }

        public Login(Token_Dto_For_Create dto, IConfiguration configuration)
        {
            this.Dto = dto;
            this.Configuration = configuration;
        }



        public class Validator : AbstractValidator<Login>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, cancellationToken) =>
                    {
                        var obj = context.InstanceToValidate.Dto;
                        var error = false;
                        

                        if (string.IsNullOrEmpty(obj.Password) || string.IsNullOrWhiteSpace(obj.Password))
                        {
                            context.AddFailure(nameof(obj.Password), "Required");
                            error = true;
                        }


                        if (obj.UserDocumentTypeId<=0)
                        {
                            context.AddFailure(nameof(obj.UserDocumentTypeId), "Required");                           
                            error = true;
                        }
                        else if (await dbContext.UserDocumentType.AsNoTracking().AnyAsync(x => x.Id == obj.UserDocumentTypeId && x.Active==true, cancellationToken) == false)
                        {
                            context.AddFailure(nameof(obj.UserDocumentTypeId), "Invalid");
                            error = true;
                        }



                        if (string.IsNullOrEmpty(obj.UserDocumentValue) || string.IsNullOrWhiteSpace(obj.UserDocumentValue))
                        {
                            context.AddFailure(nameof(obj.UserDocumentValue), "Required");
                            error = true;
                        }
                        


                        if (error == false)
                        {
                            var exists = await Where( obj.UserDocumentTypeId,   obj.UserDocumentValue, obj.Password, dbContext).AnyAsync(cancellationToken);

                            if (exists == false)
                            {
                                context.AddFailure(nameof(obj.Password), "Invalid");
                            }
                        }
                    });

            }
        }



        public class Handler : IRequestHandler<Login, Login>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<Login> Handle(Login request, CancellationToken cancellationToken)
            {
                try
                {

                    var obj = await Where(request.Dto.UserDocumentTypeId,    request.Dto.UserDocumentValue, request.Dto.Password, dbContext)
                        .Select(x => new { x.Id, x.Email, x.Name })
                        .FirstAsync(cancellationToken);


                    #region building token


                    var tokenCreation = DateTime.UtcNow;
                    var tokenExpiration = tokenCreation.AddMinutes(15);


                    var formatDateTime = "yyyy/MM/dd HH:mm:ss.fff";
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Email, obj.Email),
                        new(ClaimTypes.NameIdentifier, obj.Id.ToString()),
                        new("Token Creation",tokenCreation.ToString(formatDateTime)),
                        new("Token Expiration",tokenExpiration.ToString(formatDateTime))
                    };


                    if (obj.Id == 1)
                    {
                        claims.Add(new(Authorization_CustomPolicy.IsAdmin, "true"));
                    }
                    else
                    {
                        claims.Add(new(Authorization_CustomPolicy.IsUser, "true"));
                    }


                    var key = new SymmetricSecurityKey(Convert.FromBase64String(request.Configuration["JwtTokenSigningKey"]!));
                   

                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
                    

                    var jwtSecutiryToken = new JwtSecurityToken(
                        issuer: null,
                        audience: null,
                        claims: claims,
                        expires: tokenExpiration,
                        signingCredentials: credenciales);


                    var token = new JwtSecurityTokenHandler().WriteToken(jwtSecutiryToken);


                    var tokenDto = new Token_Dto_For_ShowInformation
                    {
                        Id = obj.Id,
                        Email = obj.Email,
                        Name = obj.Name,
                        Token = token,
                        Expiration = tokenExpiration,
                        Creation = tokenCreation
                    };


                    request.Result = tokenDto;


                    #endregion

                    return request;

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }



        protected static IQueryable<Model.User> Where(int userDocumentTypeId,string userDocumentValue, string password, ApplicationDbContext dbContext)
        {
            return dbContext.User
                .AsNoTracking()
                .Where(x => x.UserDocumentTypeId==userDocumentTypeId && x.UserDocumentValue==userDocumentValue && EF.Functions.Collate(x.Password, "Latin1_General_BIN") == password);
        }



    }
}

