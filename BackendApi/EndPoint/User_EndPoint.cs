using Dto;
using Dto.EndPointName;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BackendApi.EndPoint
{
    public static class User_EndPoint
    {

        public static RouteGroupBuilder User_EndPoint_Map(this RouteGroupBuilder endpoints)
        {
            endpoints.MapGet(User_EndPointName.GetById, GetById);

            endpoints.MapGet(User_EndPointName.GetAll, GetAll);//.RequireAuthorization(Authorization_CustomPolicy.IsAdmin);

            endpoints.MapPost(User_EndPointName.TokenCreation, TokenCreation)
                .WithOpenApi(x =>
                {
                    x.Summary = "Token Creation";
                    x.Description = "Check the email y the password, if all it's ok them generate a token";
                    //x.Parameters[0].Description = "Result's email";
                    //x.Parameters[1].Description = "Result's password";
                    x.RequestBody.Description = "Json object";
                    return x;
                });


            endpoints.MapPost(User_EndPointName.PasswordRestore, PasswordRestore);

            return endpoints;
        }




        private static async Task<Results<InternalServerError<string>, Ok, NotFound, BadRequest<Dictionary<string, string[]>>>> PasswordRestore(
        User_Dto_For_PasswordRestore dto,

        IMediator mediator,
        CancellationToken cancellationToken = default
    )
        {
            try
            {
                var cmd = new Ges.User.PasswordRestore(dto);

                await mediator.Send(cmd, cancellationToken);

                if (cmd.HasErrors)
                {
                    return TypedResults.BadRequest(cmd.Errors);
                }
                            

                return TypedResults.Ok();
            }
            catch (Exception ex)
            {
                return TypedResults.InternalServerError(ex.Message);
            }

        }


        //-----------------------------------------------------------------------------------------------------

        private static async Task<Results<InternalServerError<string>, Ok<User_Dto_For_ShowInformation>, NotFound, BadRequest<Dictionary<string, string[]>>>> GetById(
            int id,

            IMediator mediator,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var cmd = new Ges.User.GetById(id);

                await mediator.Send(cmd, cancellationToken);

                if (cmd.HasErrors)
                {
                    return TypedResults.BadRequest(cmd.Errors);
                }

                var user = cmd.Result;

                return user is null ? TypedResults.NotFound() : TypedResults.Ok(user);
            }
            catch (Exception ex)
            {
                return TypedResults.InternalServerError(ex.Message);
            }

        }

        private static async Task<Results<InternalServerError<string>, Ok<List<User_Dto_For_ShowInformation>>>> GetAll(
            IMediator mediator,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var cmd = new Ges.User.GetAll();

                await mediator.Send(cmd, cancellationToken);

                return TypedResults.Ok(cmd.Result);
            }
            catch (Exception ex)
            {
                return TypedResults.InternalServerError(ex.Message);
            }

        }


        //-----------------------------------------------------------------------------------------------------


        private static async Task<Results<Ok<Token_Dto_For_ShowInformation>, InternalServerError<string>, BadRequest<Dictionary<string, string[]>>>> TokenCreation(
            Token_Dto_For_Create dto,

            IConfiguration configuration,
            IMediator mediator,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var cmd = new Ges.User.Login(dto, configuration);

                await mediator.Send(cmd, cancellationToken);

                if (cmd.HasErrors)
                {
                    return TypedResults.BadRequest(cmd.Errors);
                }

                return TypedResults.Ok(cmd.Result);
            }
            catch (Exception ex)
            {
                return TypedResults.InternalServerError(ex.Message);
            }

        }

    }
}
