using DbEf;
using Dto;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Product
{
    public class GetAll : CmdBase, IRequest<GetAll>
    {
        public List<Product_Dto_For_ShowInformation01> Result { get; protected set; } = [];

        public GetAll()
        {
        }

        public class Validator : AbstractValidator<GetAll>
        {
            public Validator(ApplicationDbContext dbContext)
            {

            }
        }



        public class Handler : IRequestHandler<GetAll, GetAll>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<GetAll> Handle(GetAll request, CancellationToken cancellationToken)
            {
                try
                {

                    request.Result = await dbContext.Product
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

