using DbEf; 
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.User
{




    public class GetById : CmdBase_Data_Result<int, Dto.Ges.User.ShowInformation01>, IRequest<GetById>
    {
        public GetById(int data) : base(data)
        {
        }

        public class Validator : AbstractValidator<GetById>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .Custom((value, context) =>
                    {
                        var id = context.InstanceToValidate.Data;

                        if (id <= 0)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "The name must be greater than 0");
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

                    request.Result = await dbContext.User
                        .Where(x => x.Id == request.Data)
                        .AsNoTracking()
                        .Select(x => new Dto.Ges.User.ShowInformation01 { Id = x.Id, Name = x.Name, Email = x.Email })
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
