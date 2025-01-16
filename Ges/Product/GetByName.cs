using DbEf;
using Dto;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Product
{
    public class GetByName : CmdBase, IRequest<GetByName>
    {
        protected string Name { get; }

        public List<Product_Dto_For_ShowInformation01> Result { get; protected set; } = [];

        public GetByName(string name)
        {
            Name = name;
        }



        public class Validator : AbstractValidator<GetByName>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .Custom((value, context) =>
                    {
                        var name = context.InstanceToValidate.Name;

                        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Name), "Required");
                        }
                        else if (name.Length < 3)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Name), "The name must be at least 3 characters");
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
                        .Where(x => EF.Functions.Like(x.Name, $"%{request.Name}%"))
                        .AsNoTracking()
                         .Select(x => new Product_Dto_For_ShowInformation01
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Price = x.Price,
                             Description = x.Description,
                             Active = x.Active,
                             Category = new Category_Dto_For_ShowInformation01 { Id = x.Category.Id, Name = x.Category.Name }

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
