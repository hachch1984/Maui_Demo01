using DbEf;
using Dto;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Category
{



    public class GetAll : CmdBase, IRequest<GetAll>
    {
        public List<Category_Dto_For_ShowInformation03> Result { get; protected set; } = [];

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
                    request.Result = await dbContext.Category
                     .AsNoTracking()
                     .Select(x =>
                     new Category_Dto_For_ShowInformation03
                     {
                         Id = x.Id,
                         Name = x.Name,
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

