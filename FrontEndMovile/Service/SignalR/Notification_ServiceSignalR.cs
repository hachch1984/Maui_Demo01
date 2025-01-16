using Dto.EndPointName.SignalR;
using FrontEndMovile.Util;
using Microsoft.AspNetCore.SignalR.Client;

namespace FrontEndMovile.Service.SignalR
{

    public class Notification_ServiceSignalR
    {
        private readonly HubConnection hubConnection;


        public Notification_ServiceSignalR(ISetting setting)
        {

            var userId = Preferences.Get(nameof(Dto.Token_Dto_For_ShowInformation.Id), string.Empty);


            var url = $"{setting.BackendApiUrl}{Notification_EndPointNameSignalR.EndPointName}?userId={userId}";

            hubConnection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

        }

        public async Task StartConnectionAsync(CancellationToken cancellationToken = default)
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                await hubConnection.StartAsync(cancellationToken);
            }
        }

        public async Task StopConnectionAsync(CancellationToken cancellationToken = default)
        {
            if (hubConnection.State == HubConnectionState.Connected)
            {
                await hubConnection.StopAsync(cancellationToken);
            }
        }

        public void OnNewMessage(Action<string> onMessageReceived)
        {
            hubConnection.On<string>(Notification_EndPointNameSignalR.MethodName, message =>
               {
                   onMessageReceived?.Invoke(message);
               });
        }
    }
}
