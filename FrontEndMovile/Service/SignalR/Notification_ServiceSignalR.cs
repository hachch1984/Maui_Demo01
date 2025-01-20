using Dto.EndPointName.SignalR;
using FrontEndMovile.Util;
using Microsoft.AspNetCore.SignalR.Client;

namespace FrontEndMovile.Service.SignalR
{

    public interface INotification_ServiceSignalR
    {
        Task StartConnectionAsync(string userId, CancellationToken cancellationToken = default);
        Task StopConnectionAsync(CancellationToken cancellattionToken = default);
        void OnNewMessage(Action<string> onMessageReceived);
    }


    public class Notification_ServiceSignalR : INotification_ServiceSignalR
    {
        private HubConnection hubConnection;
        private readonly ISetting setting;

        public Notification_ServiceSignalR(ISetting setting)
        {
            this.setting = setting;
        }

        public async Task StartConnectionAsync(string userId, CancellationToken cancellationToken = default)
        {

            var url = $"{setting.BackendApiUrl}{Notification_EndPointNameSignalR.EndPointName}?userId={userId}";


            this.hubConnection = new HubConnectionBuilder()
                    .WithUrl(url)
                    .WithAutomaticReconnect()
                    .Build();


            this.hubConnection.On<string>(Notification_EndPointNameSignalR.MethodName, message =>
             {
                 this._OnMessageReceived.Invoke(message);
             });


            await this.hubConnection.StartAsync(cancellationToken);

        }



        public async Task StopConnectionAsync(CancellationToken cancellationToken = default)
        {
            if (this.hubConnection != null && this.hubConnection.State == HubConnectionState.Connected)
            {
                await this.hubConnection.StopAsync(cancellationToken);
                await this.hubConnection.DisposeAsync();
            }
        }

        private Action<string> _OnMessageReceived;

        public void OnNewMessage(Action<string> onMessageReceived)
        {
            this._OnMessageReceived = onMessageReceived;
        }




    }
}
