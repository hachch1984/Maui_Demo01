using DbEf; 
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Category
{
    public class GetById : CmdBase_Data_Result<int, Dto.Ges.Category.ShowInformation3Cat>, IRequest<GetById>
    {
        public GetById(int data) : base(data)
        {
        }

        public class Validator : AbstractValidator<GetById>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .Custom((value, context) =>
                    {
                        var id = context.InstanceToValidate.Data;

                        if (id <= 0)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "The name must be greater than 0");
                        }
                        else if (!dbContext.Category.Any(x => x.Id == id))
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "Not exist");
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

                    request.Result = await dbContext.Category
                        .Where(x => x.Id == request.Data)
                        .AsNoTracking()
                        .Select(x => new Dto.Ges.Category.ShowInformation3Cat { Id = x.Id, Name = x.Name, Description = x.Description, Active = x.Active })
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
