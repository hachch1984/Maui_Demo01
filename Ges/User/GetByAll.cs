using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.User
{




    public class GetAll : CmdBase, IRequest<GetAll>
    {


        public List<Dto.User_Dto_For_ShowInformation> Result { get; protected set; } = new List<Dto.User_Dto_For_ShowInformation>();

        public GetAll()
        { }



        public class Validator : AbstractValidator<GetAll>
        {
            public Validator()
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

                    request.Result = await dbContext.User
                        .OrderBy(x => x.Name)
                        .AsNoTracking()
                        .Select(x => new Dto.User_Dto_For_ShowInformation { Id = x.Id, Name = x.Name, Email = x.Email })
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
