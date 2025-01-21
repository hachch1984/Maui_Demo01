using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.User
{




    public class GetUserDocumentTypeActive : CmdBase_Result<List<Dto.Ges.User.UserDocumentType_ShowInformation>>, IRequest<GetUserDocumentTypeActive>
    {






        //public class Validator : AbstractValidator<GetUserDocumentTypeActive>
        //{
        //    public Validator()
        //    {

        //    }
        //}



        public class Handler : IRequestHandler<GetUserDocumentTypeActive, GetUserDocumentTypeActive>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<GetUserDocumentTypeActive> Handle(GetUserDocumentTypeActive request, CancellationToken cancellationToken)
            {
                try
                {

                    request.Result = await dbContext.UserDocumentType
                        .Where(x => x.Active == true)
                        .OrderBy(x => x.Name)
                        .AsNoTracking()
                        .Select(x => new Dto.Ges.User. UserDocumentType_ShowInformation { Id = x.Id, Name = x.Name })
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
