using DbEf;
using Dto;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Product
{
    public class GetByCategoryId : CmdBase, IRequest<GetByCategoryId>
    {
        protected int CategoryId { get; }

        public List<Product_Dto_For_ShowInformation03> Result { get; protected set; } = [];

        public GetByCategoryId(int categoryId)
        {
            CategoryId = categoryId;
        }



        public class Validator : AbstractValidator<GetByCategoryId>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, CancellationToken) =>
                    {
                        var id = context.InstanceToValidate.CategoryId;

                        if (id <= 0)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.CategoryId), "The name must be greater than 0");
                        }
                        else if (await dbContext.Category.AnyAsync(x => x.Id == id, CancellationToken) == false)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.CategoryId), "Not Found");
                        }
                    });

            }
        }



        public class Handler : IRequestHandler<GetByCategoryId, GetByCategoryId>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<GetByCategoryId> Handle(GetByCategoryId request, CancellationToken cancellationToken)
            {
                try
                {

                    request.Result = await dbContext.Product
                        .Where(x => x.CategoryId == request.CategoryId)
                        .Include(x => x.Category)
                        .AsNoTracking()
                       .Select(x => new Product_Dto_For_ShowInformation03
                       {
                           Id = x.Id,
                           Name = x.Name,
                           Price = x.Price,
                           Description = x.Description,
                           Active = x.Active
                       })
                       .OrderBy(x => x.Name)
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
