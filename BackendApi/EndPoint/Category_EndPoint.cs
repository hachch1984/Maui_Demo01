using Dto;
using Dto.EndPointName;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BackendApi.EndPoint
{
    public static class Category_EndPoint
    {
      
        public static RouteGroupBuilder Category_EndPoint_Map(this RouteGroupBuilder endpoints)
        {
            endpoints.MapGet(Category_EndPointName.GetById, GetById);
            endpoints.MapGet(Category_EndPointName.GetAll, GetAll);

            endpoints.MapPost(Category_EndPointName.Add, Add);
            endpoints.MapPut(Category_EndPointName.Update, Update);
            endpoints.MapPatch(Category_EndPointName.Update_Active, Update_Active);
            endpoints.MapDelete(Category_EndPointName.Delete, Delete);

            return endpoints;
        }

        public static async 
            Task<
            Results<
                Ok<Category_Dto_For_ShowInformation03>,
                BadRequest<Dictionary<string, string[]>>, 
                NotFound, 
                InternalServerError<string>
                >
            > GetById(

            int id,

            IMediator mediator,
            CancellationToken cancellationToken = default
            )
        {
            try
            { 
                var cmd = new Ges.Category.GetById(id);
                await mediator.Send(cmd, cancellationToken);
                if (cmd.HasErrors)
                {
                    return TypedResults.BadRequest(cmd.Errors);
                }
                return cmd.Result is null ? TypedResults.NotFound() : TypedResults.Ok(cmd.Result);
            }
            catch (Exception ex)
            {

                return TypedResults.InternalServerError(ex.Message);
            }
        }

        public static async Task<Results<Ok<List<Category_Dto_For_ShowInformation03>>, InternalServerError<string>>> GetAll(
            IMediator mediator,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var cmd = new Ges.Category.GetAll();
                await mediator.Send(cmd, cancellationToken);
                return TypedResults.Ok(cmd.Result);
            }
            catch (Exception ex)
            {

                return TypedResults.InternalServerError(ex.Message);
            }
        }

        public static async Task<
            Results<
                Created<int>, 
                BadRequest<Dictionary<string, string[]>>,
                InternalServerError<string>
                >> Add(
            Category_Dto_For_Add dto,

            IMediator mediator,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var cmd = new Ges.Category.Add(dto);
                await mediator.Send(cmd, cancellationToken);
                if (cmd.HasErrors)
                {
                    return TypedResults.BadRequest(cmd.Errors);
                }

                var url = $"{Category_EndPointName.EndPointName}{Category_EndPointName.GetById}?id={cmd.Result.Id}";

                return TypedResults.Created(url, cmd.Result.Id);


            }
            catch (Exception ex)
            {
                return TypedResults.InternalServerError(ex.Message);
            }
        }

        public static async Task<Results<Ok, BadRequest<Dictionary<string, string[]>>, InternalServerError<string>>> Update(
            Category_Dto_For_Update dto,

            IMediator mediator,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var cmd = new Ges.Category.Update(dto);
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
        public static async Task<Results<Ok, BadRequest<Dictionary<string, string[]>>, InternalServerError<string>>> Update_Active(
        Category_Dto_For_Update_Active dto,

        IMediator mediator,
        CancellationToken cancellationToken = default
        )
        {
            try
            {
                var cmd = new Ges.Category.Update_Active(dto);
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

      


        public static async Task<Results<Ok, BadRequest<Dictionary<string, string[]>>, InternalServerError<string>>> Delete(
            int id,

            IMediator mediator,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var cmd = new Ges.Category.Delete(id);
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

    }
}
