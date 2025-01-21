using DbEf;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ges.User
{

    public class PasswordRestore : CmdBase_Data<Dto.Ges.User.PasswordRestore>, IRequest<PasswordRestore>
    {
        public PasswordRestore(Dto.Ges.User.PasswordRestore data) : base(data)
        {
        }

        public class Validator : AbstractValidator<PasswordRestore>
        {
            public Validator(ApplicationDbContext dbContext)
            {
                RuleFor(x => x)
                    .CustomAsync(async (value, context, cancellationToken) =>
                    {

                        if (string.IsNullOrEmpty(context.InstanceToValidate.Data.Email) || string.IsNullOrWhiteSpace(context.InstanceToValidate.Data.Email))
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data.Email), "Required");

                        }
                        else if (await dbContext.User.AsNoTracking().AnyAsync(x => x.Email == context.InstanceToValidate.Data.Email, cancellationToken) == false)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Data.Email), "Not exists");
                        }
                    });

            }
        }



        public class Handler : IRequestHandler<PasswordRestore, PasswordRestore>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<PasswordRestore> Handle(PasswordRestore request, CancellationToken cancellationToken)
            {
                try
                {
                    //proceso de envio de correo electronico


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

