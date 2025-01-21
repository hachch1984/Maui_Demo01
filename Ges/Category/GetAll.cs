using DbEf;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Category
{



    public class GetAll : CmdBase_Result<List<Dto.Ges.Category.ShowInformation3Cat>>, IRequest<GetAll>
    {
      



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
                     new Dto.Ges.Category.ShowInformation3Cat
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

