using AutoMapper;
using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ges.Category
{

    public class Update_Active : CmdBase_Data<Dto.Ges.Category.Update_ActiveCat>, IRequest<Update_Active>
    {
        public Update_Active(Dto.Ges.Category.Update_ActiveCat data) : base(data)
        {
        }

        public class Validator : AbstractValidator<Update_Active>
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
                        else if (await dbContext.Category.AsNoTracking().AnyAsync(x => x.Id == obj.Id, cancellationToken) == false)
                        {
                            context.AddFailure(nameof(obj.Id), "Not Found");
                        }


                    });

            }
        }



        public class Handler : IRequestHandler<Update_Active, Update_Active>
        {
            private readonly ApplicationDbContext dbContext;
            private readonly IMapper mapper;

            public Handler(ApplicationDbContext dbContext, IMapper mapper)
            {
                this.dbContext = dbContext;
                this.mapper = mapper;
            }

            public async Task<Update_Active> Handle(Update_Active request, CancellationToken cancellationToken)
            {
                IDbContextTransaction? tran = null;

                try
                {
                    if (dbContext.Database.CurrentTransaction == null)
                    {
                        tran = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                    }

                    var obj = request.Data;

                    await dbContext.Category
                        .Where(x => x.Id == obj.Id)
                        .ExecuteUpdateAsync(x => 
                            x.SetProperty(y => y.Active, obj.Active)
                            
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

