using BackendApi.EndPoint;
using Dto.EndPointName;
using Dto.EndPointName.SignalR;

namespace BackendApi
{
    public static class Program_EndPoint
    {
        public static void Configure(WebApplicationBuilder builder, WebApplication app)
        {

            //app.MapGet("/", () => " :)  Hello World!");


            app.MapGroup(UserDocumentType_EndPointName.EndPointName).UserDocumentType_EndPoint_Map();


            app.MapGroup(User_EndPointName.EndPointName).User_EndPoint_Map();

            app.MapGroup(Category_EndPointName.EndPointName).Category_EndPoint_Map();

            app.MapGroup(Product_EndPointName.EndPointName).Product_EndPoint_Map();//.RequireAuthorization();




            #region SignalR

            app.MapHub<Notification_EndPoint>(Notification_EndPointNameSignalR.EndPointName);

            #endregion

        }
    }
}
