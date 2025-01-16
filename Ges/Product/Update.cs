using AutoMapper;
using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ges.Product
{

    public class Update : CmdBase, IRequest<Update>
    {
        protected Dto.Product_Dto_For_Update Dto { get; }

        public Update(Dto.Product_Dto_For_Update dto)
        {
            this.Dto = dto;
        }

        protected bool Result { get; set; } = false;


        public class Validator : AbstractValidator<Update>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, cancellationToken) =>
                    {
                        var obj = context.InstanceToValidate.Dto;
                        var idExists = true;


                        if (obj.Id <= 0)
                        {
                            context.AddFailure(nameof(obj.Id), "Must be greater than 0");
                            idExists = false;
                        }
                        else if (await dbContext.Product.AsNoTracking().AnyAsync(x => x.Id == obj.Id, cancellationToken) == false)
                        {
                            context.AddFailure(nameof(obj.Id), "Not Found");
                            idExists = false;
                        }


                        if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrWhiteSpace(obj.Name))
                        {
                            context.AddFailure(nameof(obj.Name), "Required");
                        }
                        else if (idExists == true && await dbContext.Product.AsNoTracking().AnyAsync(x => x.Id != obj.Id && EF.Functions.Like(x.Name, $"%{obj.Name}%"), cancellationToken) == true)
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



        public class Handler : IRequestHandler<Update, Update>
        {
            private readonly ApplicationDbContext dbContext;
            private readonly IMapper mapper;

            public Handler(ApplicationDbContext dbContext, IMapper mapper)
            {
                this.dbContext = dbContext;
                this.mapper = mapper;
            }

            public async Task<Update> Handle(Update request, CancellationToken cancellationToken)
            {
                IDbContextTransaction? tran = null;

                try
                {
                    if (dbContext.Database.CurrentTransaction == null)
                    {
                        tran = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                    }


                    var obj = this.mapper.Map<Model.Product>(request.Dto);

                    dbContext.Product.Update(obj);

                    var rowCount = await dbContext.SaveChangesAsync(cancellationToken);

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

