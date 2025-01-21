using DbEf; 
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.Product
{
    public class GetById : CmdBase_Data_Result<long, Dto.Ges.Product.ShowInformation01Prod>, IRequest<GetById>
    {
        public GetById(long data) : base(data)
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

                    request.Result = await dbContext.Product
                        .Where(x => x.Id == request.Data)
                        .Include(x => x.Category)
                        .AsNoTracking()
                       .Select(x => new Dto.Ges.Product.ShowInformation01Prod
                       {
                           Id = x.Id,
                           Name = x.Name,
                           Price = x.Price,
                           Description = x.Description,
                           Active = x.Active,
                           Category = new Dto.Ges.Category.ShowInformation1Cat { Id = x.Category.Id, Name = x.Category.Name }

                       })
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
