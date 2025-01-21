using AutoMapper;
using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ges.Product
{
    public class Add : CmdBase_Data_Result<Dto.Ges.Product.AddProd,Model.Product>, IRequest<Add>
    {
        public Add(Dto.Ges.Product.AddProd data) : base(data)
        {
        }

        public class Validator : AbstractValidator<Add>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, cancellationToken) =>
                    {
                        var obj = context.InstanceToValidate.Data;


                        if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrWhiteSpace(obj.Name))
                        {
                            context.AddFailure(nameof(obj.Name), "Required");
                        }
                        else if (await dbContext.Product.AsNoTracking().AnyAsync(x => EF.Functions.Like(x.Name, $"%{obj.Name}%"), cancellationToken) == true)
                        {
                            context.AddFailure(nameof(obj.Name), "Already Exists");
                        }


                        if (string.IsNullOrEmpty(obj.Description) || string.IsNullOrWhiteSpace(obj.Description))
                        {
                            context.AddFailure(nameof(obj.Description), "Required");
                        }


                        if (obj.CategoryId <= 0)
                        {
                            context.AddFailure(nameof(obj.CategoryId), "Must be greater than 0");
                        }
                        else 
                        {
                            var objCategory = await dbContext.Category.Where(x => x.Id == obj.CategoryId).AsNoTracking().Select(x => new { x.Active }).FirstOrDefaultAsync(cancellationToken);

                            if (objCategory == null)
                            {
                                context.AddFailure(nameof(obj.CategoryId), "Not Found");
                            }
                            else if (objCategory.Active == false)
                            {
                                context.AddFailure(nameof(obj.CategoryId), "Inactive");
                            }
                        }


                        if (obj.Price <= 0)
                        {
                            context.AddFailure(nameof(obj.Price), "The price must be greater than 0");
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

                    var obj = this.mapper.Map<Model.Product>(request.Data);

                    await dbContext.Product.AddAsync(obj, cancellationToken);

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
