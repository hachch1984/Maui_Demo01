using DbEf; 
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Product
{
    public class GetByName : CmdBase_Data_Result<string, List<Dto.Ges.Product.ShowInformation01Prod>>, IRequest<GetByName>
    {
        public GetByName(string data) : base(data)
        {
        }

        public class Validator : AbstractValidator<GetByName>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .Custom((value, context) =>
                    {
                        var name = context.InstanceToValidate.Data;

                        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "Required");
                        }
                        else if (name.Length < 3)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "The name must be at least 3 characters");
                        }
                    });

            }
        }



        public class Handler : IRequestHandler<GetByName, GetByName>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<GetByName> Handle(GetByName request, CancellationToken cancellationToken)
            {
                try
                {

                    request.Result = await dbContext.Product
                        .Where(x => EF.Functions.Like(x.Name, $"%{request.Data}%"))
                        .AsNoTracking()
                         .Select(x => new Dto.Ges.Product.ShowInformation01Prod
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Price = x.Price,
                             Description = x.Description,
                             Active = x.Active,
                             Category = new Dto.Ges.Category.ShowInformation1Cat { Id = x.Category.Id, Name = x.Category.Name }

                         })
                         .OrderBy(x => x.Category.Name)
                         .ThenBy(x => x.Name)
                         .ToListAsync(cancellationToken);

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
