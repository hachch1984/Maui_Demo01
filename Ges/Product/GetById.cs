using DbEf;
using Dto;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Product
{
    public class GetById : CmdBase, IRequest<GetById>
    {
        protected long Id { get; }

        public Product_Dto_For_ShowInformation01? Result { get; protected set; }

        public GetById(long id)
        {
            Id = id;
        }



        public class Validator : AbstractValidator<GetById>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .Custom((value, context) =>
                    {
                        var id = context.InstanceToValidate.Id;

                        if (id <= 0)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Id), "The name must be greater than 0");
                        }
                    });

            }
        }



        public class Handler : IRequestHandler<GetById, GetById>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<GetById> Handle(GetById request, CancellationToken cancellationToken)
            {
                try
                {

                    request.Result = await dbContext.Product
                        .Where(x => x.Id == request.Id)
                        .Include(x => x.Category)
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
                       .FirstOrDefaultAsync(cancellationToken);

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
