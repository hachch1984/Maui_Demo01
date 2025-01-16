using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.User
{




    public class GetById : CmdBase, IRequest<GetById>
    {
        protected int Id { get; }
        public Dto.User_Dto_For_ShowInformation? Result { get; protected set; } = default!;

        public GetById(int id)
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

                    request.Result = await dbContext.User
                        .Where(x => x.Id == request.Id)
                        .AsNoTracking()
                        .Select(x => new Dto.User_Dto_For_ShowInformation { Id = x.Id, Name = x.Name, Email = x.Email })
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
