using Dto.EndPointName;
 
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BackendApi.EndPoint
{
    public static class UserDocumentType_EndPoint
    {

        public static RouteGroupBuilder UserDocumentType_EndPoint_Map(this RouteGroupBuilder endpoints)
        {

            endpoints.MapGet(UserDocumentType_EndPointName.GetAllOnlyActive, GetAllOnlyActive);


            return endpoints;
        }

        //-----------------------------------------------------------------------------------------------------

     
        private static async Task<Results<InternalServerError<string>, Ok<List<Dto.Ges.User.UserDocumentType_ShowInformation>>>> GetAllOnlyActive(
            IMediator mediator,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var cmd = new Ges.User.GetUserDocumentTypeActive();

                await mediator.Send(cmd, cancellationToken);

                return TypedResults.Ok(cmd.Result);
            }
            catch (Exception ex)
            {
                return TypedResults.InternalServerError(ex.Message);
            }

        }


    }
}
