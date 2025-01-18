using DbEf;
using Dto;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ges.User
{

    public class PasswordRestore : CmdBase, IRequest<PasswordRestore>
    {
        protected User_Dto_For_PasswordRestore Dto { get; }
        public bool Result { get; protected set; } = false;
        protected IConfiguration Configuration { get; }

        public PasswordRestore(User_Dto_For_PasswordRestore dto)
        {
            this.Dto = dto;
        }



        public class Validator : AbstractValidator<PasswordRestore>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, cancellationToken) =>
                    {

                        if (string.IsNullOrEmpty(context.InstanceToValidate.Dto.Email) || string.IsNullOrWhiteSpace(context.InstanceToValidate.Dto.Email))
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Dto.Email), "Required");

                        }
                        else if (await dbContext.User.AsNoTracking().AnyAsync(x => x.Email == context.InstanceToValidate.Dto.Email, cancellationToken) == false)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Dto.Email), "Not exists");
                        }
                    });

            }
        }



        public class Handler : IRequestHandler<PasswordRestore, PasswordRestore>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<PasswordRestore> Handle(PasswordRestore request, CancellationToken cancellationToken)
            {
                try
                {
                    //proceso de envio de correo electronico

                    request.Result = true;

                    return request;

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }







    }
}

