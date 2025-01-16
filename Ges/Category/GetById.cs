using DbEf;
using Dto;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Category
{
    public class GetById : CmdBase, IRequest<GetById>
    {
        protected int Id { get; }

        public Category_Dto_For_ShowInformation03? Result { get; protected set; }

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

                    request.Result = await dbContext.Category
                        .Where(x => x.Id == request.Id)
                        .AsNoTracking()
                        .Select(x => new Category_Dto_For_ShowInformation03 { Id = x.Id, Name = x.Name, Description = x.Description, Active = x.Active })
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
