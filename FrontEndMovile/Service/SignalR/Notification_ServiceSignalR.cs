using Dto.EndPointName.SignalR;
using FrontEndMovile.Util;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace FrontEndMovile.Service.SignalR
{

    public interface INotification_ServiceSignalR
    {
        Task StartConnectionAsync(string token, string userId, CancellationToken cancellationToken = default);
        Task StopConnectionAsync(CancellationToken cancellationToken = default);
        void OnNewMessage(Action<string> onMessageReceived);
        Task RestartConnectionAsync(string token, string userId, CancellationToken cancellationToken = default);
    }


    public class Notification_ServiceSignalR : INotification_ServiceSignalR
    {
        private HubConnection hubConnection;
        private readonly ISetting setting;

        public Notification_ServiceSignalR(ISetting setting)
        {
            this.setting = setting;
        }

        public async Task StartConnectionAsync(string token, string userId, CancellationToken cancellationToken = default)
        {
            var url = $"{setting.BackendApiUrl}{Notification_EndPointNameSignalR.EndPointName}?userId={userId}";

            try
            {



                this.hubConnection = new HubConnectionBuilder()
                 .WithUrl(url, options =>
                 {
                     options.AccessTokenProvider = () => Task.FromResult($"{token}");
                 })
                 .WithAutomaticReconnect()
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                 .Build();
            }
            catch (Exception ex)
            {

                throw;
            }

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
            }
            if (this.hubConnection != null)
            {
                await this.hubConnection.DisposeAsync();
                this.hubConnection = null;
            }
        }

        public async Task RestartConnectionAsync(string token, string userId, CancellationToken cancellationToken = default)
        {
            await this.StopConnectionAsync(cancellationToken);
            await this.StartConnectionAsync(token, userId, cancellationToken);
        }

        private Action<string> _OnMessageReceived;

        public void OnNewMessage(Action<string> onMessageReceived)
        {
            this._OnMessageReceived = onMessageReceived;
        }
    }
}
