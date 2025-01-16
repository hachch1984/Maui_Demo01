using Dto;
using Dto.EndPointName;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BackendApi.EndPoint
{
    public static class Product_EndPoint
    {
       
        public static RouteGroupBuilder Product_EndPoint_Map(this RouteGroupBuilder endpoints)
        {
            endpoints.MapGet(Product_EndPointName.GetById, GetById);
            endpoints.MapGet(Product_EndPointName.GetAll, GetAll);
            endpoints.MapGet(Product_EndPointName.GetByCategoryId, GetByCategoryId);
            endpoints.MapGet(Product_EndPointName.GetByName, GetByName);

            endpoints.MapPost(Product_EndPointName.Add, Add);
            endpoints.MapPut(Product_EndPointName.Update, Update);
            endpoints.MapPatch(Product_EndPointName.Update_Active, Update_Active);
            endpoints.MapPatch(Product_EndPointName.Update_Price, Update_Price);
            endpoints.MapDelete(Product_EndPointName.Delete, Delete);

            return endpoints;
        }

        public static async Task<Results<Ok<Product_Dto_For_ShowInformation01>, BadRequest<Dictionary<string, string[]>>, NotFound, InternalServerError<string>>> GetById(
            long id,

            IMediator mediator,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var cmd = new Ges.Product.GetById(id);
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

        public static async Task<Results<Ok<List<Product_Dto_For_ShowInformation03>>, InternalServerError<string>>> GetByCategoryId(
            int categoryId,

            IMediator mediator,
            CancellationToken cancellationToken = default
         )
        {
            try
            {
                var cmd = new Ges.Product.GetByCategoryId(categoryId);
                await mediator.Send(cmd, cancellationToken);
                return TypedResults.Ok(cmd.Result);
            }
            catch (Exception ex)
            {

                return TypedResults.InternalServerError(ex.Message);
            }
        }


        public static async Task<Results<Ok<List<Product_Dto_For_ShowInformation01>>, InternalServerError<string>>> GetAll(
            IMediator mediator,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var cmd = new Ges.Product.GetAll();
                await mediator.Send(cmd, cancellationToken);
                return TypedResults.Ok(cmd.Result);
            }
            catch (Exception ex)
            {

                return TypedResults.InternalServerError(ex.Message);
            }
        }

        public static async Task<Results<Ok<List<Product_Dto_For_ShowInformation01>>, InternalServerError<string>>> GetByName(
            string name,

           IMediator mediator,
           CancellationToken cancellationToken = default
           )
        {
            try
            {
                var cmd = new Ges.Product.GetByName(name);
                await mediator.Send(cmd, cancellationToken);
                return TypedResults.Ok(cmd.Result);
            }
            catch (Exception ex)
            {

                return TypedResults.InternalServerError(ex.Message);
            }
        }


        public static async Task<Results<Created<long>, BadRequest<Dictionary<string, string[]>>, InternalServerError<string>>> Add(
           Product_Dto_For_Add dto,

           IMediator mediator,
           CancellationToken cancellationToken = default
           )
        {
            try
            {
                var cmd = new Ges.Product.Add(dto);

                await mediator.Send(cmd, cancellationToken);
                if (cmd.HasErrors)
                {
                    return TypedResults.BadRequest(cmd.Errors);
                }

                var url = $"{Product_EndPointName.EndPointName}{Product_EndPointName.GetById}?id={cmd.Result!.Id}";

                return TypedResults.Created(url, cmd.Result.Id);

            }
            catch (Exception ex)
            {
                return TypedResults.InternalServerError(ex.Message);
            }
        }


        public static async Task<Results<Ok, BadRequest<Dictionary<string, string[]>>, InternalServerError<string>>> Update(
           Product_Dto_For_Update dto,

           IMediator mediator,
           CancellationToken cancellationToken = default
           )
        {
            try
            {
                var cmd = new Ges.Product.Update(dto);
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
          Product_Dto_For_Update_Active dto,

          IMediator mediator,
          CancellationToken cancellationToken = default
          )
        {
            try
            {
                var cmd = new Ges.Product.Update_Active(dto);
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
        public static async Task<Results<Ok, BadRequest<Dictionary<string, string[]>>, InternalServerError<string>>> Update_Price(
          Product_Dto_For_Update_Price dto,

          IMediator mediator,
          CancellationToken cancellationToken = default
          )
        {
            try
            {
                var cmd = new Ges.Product.Update_Price(dto);
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
                var cmd = new Ges.Product.Delete(id);
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
