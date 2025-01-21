using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ges.Product
{

    public class Update_Price : CmdBase_Data<Dto.Ges.Product.Update_PriceProd>, IRequest<Update_Price>
    {
        public Update_Price(Dto.Ges.Product.Update_PriceProd data) : base(data)
        {
        }

        public class Validator : AbstractValidator<Update_Price>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, cancellationToken) =>
                    {
                        var obj = context.InstanceToValidate.Data;

                        if (obj.Id <= 0)
                        {
                            context.AddFailure(nameof(obj.Id), "Must be greater than 0");
                        }
                        else if (await dbContext.Product.AnyAsync(x => x.Id == obj.Id, cancellationToken) == false)
                        {
                            context.AddFailure(nameof(obj.Id), "Not Found");
                        }

                        if (obj.Price <= 0)
                        {
                            context.AddFailure(nameof(obj.Price), "The price must be greater than 0");
                        }

                    });

            }
        }



        public class Handler : IRequestHandler<Update_Price, Update_Price>
        {
            private readonly ApplicationDbContext dbContext;
            
            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<Update_Price> Handle(Update_Price request, CancellationToken cancellationToken)
            {
                IDbContextTransaction? tran = null;

                try
                {
                    if (dbContext.Database.CurrentTransaction == null)
                    {
                        tran = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                    }

                    var obj = request.Data;

                    await dbContext.Product
                        .Where(x => x.Id == obj.Id)
                        .ExecuteUpdateAsync(x => 
                            x.SetProperty(y => y.Price, obj.Price)
                            , cancellationToken);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    if (tran != null)
                    {
                        await tran.CommitAsync(cancellationToken);
                    }


                    return request;
                }
                catch (Exception)
                {
                    if (tran != null)
                    {
                        await tran.RollbackAsync(cancellationToken);
                    }
                    throw;
                }

            }
        }



    }
}

