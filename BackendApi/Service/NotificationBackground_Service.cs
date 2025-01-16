
using DbEf;
using Dto.EndPointName.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Threading.Tasks.Dataflow;

namespace BackendApi.Service
{
    /// <summary>
    /// Servicio de envio de notificaciones en segundo plano
    /// </summary>
    public class NotificationBackground_Service : BackgroundService
    {
        private readonly IHubContext<Notification_EndPoint> hubContext;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<NotificationBackground_Service> logger;

        public NotificationBackground_Service(IHubContext<Notification_EndPoint> hubContext, IServiceProvider serviceProvider, ILogger<NotificationBackground_Service> logger)
        {
            this.hubContext = hubContext;
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        /// <summary>
        /// Método que se ejecuta en segundo plano
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested==false)
            {
                using var scope = this.serviceProvider.CreateScope();//Crear un nuevo alcance de servicio
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();//Obtener el contexto de la base de datos

                var arr = await dbContext.Message_ToSend
                    .AsNoTracking()
                    .Where(x => x.Date.Date == DateTime.Now.Date)
                    .ToListAsync(cancellationToken);

                foreach (var obj in arr)
                {
                    var connectionId = Notification_EndPoint.GetConnectionId(obj.UserId.ToString());//Obtener el ConnectionId asociado al UserId

                    if (string.IsNullOrEmpty(connectionId)==false)
                    {
                        try
                        {
                            await hubContext.Clients.Client(connectionId).SendAsync(Notification_EndPointNameSignalR.MethodName, obj.Message, cancellationToken);//Enviar mensaje al cliente

                            using var tran = await dbContext.Database.BeginTransactionAsync(cancellationToken);

                            await dbContext.Message_Sended.AddAsync(new Message_Sended
                            {
                                Date = obj.Date,
                                Time = obj.Time,
                                UserId = obj.UserId,
                                Message = obj.Message
                            }, cancellationToken);

                            await dbContext.Message_ToSend
                                .Where(x => x.UserId == obj.UserId && x.Date.Date == obj.Date.Date && x.Time == obj.Time)
                                .ExecuteDeleteAsync(cancellationToken);

                            await dbContext.SaveChangesAsync(cancellationToken);

                            await tran.CommitAsync(cancellationToken);
                        }
                        catch (Exception ex)
                        {                           
                            this.logger.LogError(ex, $"Error al enviar mensaje al userId: {obj.UserId}");
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
        }
    }



   

}
