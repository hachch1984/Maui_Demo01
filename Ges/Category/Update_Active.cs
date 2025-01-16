using AutoMapper;
using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ges.Category
{

    public class Update_Active : CmdBase, IRequest<Update_Active>
    {
        protected Dto.Category_Dto_For_Update_Active Dto { get; }

        public Update_Active(Dto.Category_Dto_For_Update_Active dto)
        {
            this.Dto = dto;
        }

        protected bool Result { get; set; } = false;


        public class Validator : AbstractValidator<Update_Active>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, cancellationToken) =>
                    {
                        var obj = context.InstanceToValidate.Dto;

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

                    var obj = request.Dto;

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

                    request.Result = true;

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

