using DbEf; 
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Category
{
    public class GetByName : CmdBase_Data_Result<string, List<Dto.Ges.Category.ShowInformation3Cat>>, IRequest<GetByName>
    {
        public GetByName(string data) : base(data)
        {
        }

        public class Validator : AbstractValidator<GetByName>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .Custom((value, context) =>
                    {
                        var name = context.InstanceToValidate.Data;

                        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "Required");
                        }
                        else if (name.Length < 3)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "The name must be at least 3 characters");
                        }
                    });

            }
        }



        public class Handler : IRequestHandler<GetByName, GetByName>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<GetByName> Handle(GetByName request, CancellationToken cancellationToken)
            {
                try
                {

                    request.Result = await dbContext.Category
                        .Where(x => EF.Functions.Like(x.Name, $"%{request.Data}%"))
                        .AsNoTracking()
                        .Select(x => new Dto.Ges.Category.ShowInformation3Cat { Id = x.Id, Name = x.Name, Description = x.Description, Active = x.Active })
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
