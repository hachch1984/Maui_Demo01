using DbEf; 
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Product
{
    public class GetByCategoryId : CmdBase_Data_Result<int,List<Dto.Ges.Product.ShowInformation03Prod>>, IRequest<GetByCategoryId>
    {
        public GetByCategoryId(int data) : base(data)
        {
        }

        public class Validator : AbstractValidator<GetByCategoryId>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, CancellationToken) =>
                    {
                        var id = context.InstanceToValidate.Data;

                        if (id <= 0)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "The name must be greater than 0");
                        }
                        else if (await dbContext.Category.AnyAsync(x => x.Id == id, CancellationToken) == false)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "Not Found");
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
                        .Where(x => x.CategoryId == request.Data)
                        .Include(x => x.Category)
                        .AsNoTracking()
                       .Select(x => new Dto.Ges.Product.ShowInformation03Prod
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
