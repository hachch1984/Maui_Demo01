﻿using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ges.Product
{

    public class Delete : CmdBase_Data<int>, IRequest<Delete>
    {
        public Delete(int data) : base(data)
        {
        }

        public class Validator : AbstractValidator<Delete>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, cancellationToken) =>
                    {
                        var id = context.InstanceToValidate.Data;

                        if (id <= 0)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "Must be greater than 0");
                        }
                        else if (await dbContext.Product.AsNoTracking().AnyAsync(x => x.Id == id, cancellationToken) == false)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data), "Not Found");
                        } 

                    });

            }
        }



        public class Handler : IRequestHandler<Delete, Delete>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<Delete> Handle(Delete request, CancellationToken cancellationToken)
            {
                IDbContextTransaction? tran = null;

                try
                {
                    if (dbContext.Database.CurrentTransaction == null)
                    {
                        tran = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                    }

                    var rowCount = await dbContext.Product
                            .Where(x => x.Id == request.Data)
                           .ExecuteDeleteAsync(cancellationToken);

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

