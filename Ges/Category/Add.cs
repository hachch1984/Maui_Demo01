using AutoMapper;
using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ges.Category
{

    public class Add : CmdBase_Data_Result<Dto.Ges.Category.AddCat,Model.Category>, IRequest<Add>
    {
        public Add(Dto.Ges.Category.AddCat data) : base(data)
        {
        }

        //protected Dto.Category_Dto_For_Add Dto { get; }


        //public Add(Dto.Category_Dto_For_Add dto)
        //{
        //    this.Dto = dto;
        //}

        //public Model.Category Result { get; protected set; } = null!;


        public class Validator : AbstractValidator<Add>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, cancellationToken) =>
                    {
                        var obj = context.InstanceToValidate.Data;

                        if (string.IsNullOrEmpty(obj.Description) || string.IsNullOrWhiteSpace(obj.Description))
                        {
                            context.AddFailure(nameof(obj.Description), "Required");
                        }

                        if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrWhiteSpace(obj.Name))
                        {
                            context.AddFailure(nameof(obj.Name), "Required");
                        }
                        else if (await dbContext.Category.AsNoTracking().AnyAsync(x => x.Name == obj.Name, cancellationToken) == true)
                        {
                            context.AddFailure(nameof(obj.Name), "Already Exists");
                        }
                    });

            }
        }


        public class Handler : IRequestHandler<Add, Add>
        {
            private readonly ApplicationDbContext dbContext;
            private readonly IMapper mapper;

            public Handler(ApplicationDbContext dbContext, IMapper mapper)
            {
                this.dbContext = dbContext;
                this.mapper = mapper;
            }

            public async Task<Add> Handle(Add request, CancellationToken cancellationToken)
            {
                IDbContextTransaction? tran = null;

                try
                {
                    if (dbContext.Database.CurrentTransaction == null)
                    {
                        tran = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                    }

                    var obj = this.mapper.Map<Model.Category>(request.Data);

                    await dbContext.Category.AddAsync(obj, cancellationToken);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    if (tran != null)
                    {
                        await tran.CommitAsync(cancellationToken);
                    }

                    request.Result = obj;

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

