using AutoMapper;
using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ges.Category
{

    public class Update : CmdBase, IRequest<Update>
    {
        protected Dto.Category_Dto_For_Update Dto { get; }

        public Update(Dto.Category_Dto_For_Update dto)
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
                        else if (await dbContext.Category.AsNoTracking().AnyAsync(x => x.Id == obj.Id, cancellationToken) == false)
                        {
                            context.AddFailure(nameof(obj.Id), "Not Found");
                            idExists = false;
                        }

                        if (string.IsNullOrEmpty(obj.Description) || string.IsNullOrWhiteSpace(obj.Description))
                        {
                            context.AddFailure(nameof(obj.Description), "Required");
                        }

                        if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrWhiteSpace(obj.Name))
                        {
                            context.AddFailure(nameof(obj.Name), "Required");
                        }
                        else if (idExists == true && await dbContext.Category.AsNoTracking().AnyAsync(x => x.Id != obj.Id && x.Name == obj.Name, cancellationToken) == true)
                        {
                            context.AddFailure(nameof(obj.Name), "Already Exists");
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


                    var obj = this.mapper.Map<Model.Category>(request.Dto);

                   dbContext.Category.Update(obj);

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

