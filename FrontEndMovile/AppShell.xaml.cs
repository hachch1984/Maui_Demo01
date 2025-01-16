using FrontEndMovile.Service;
using FrontEndMovile.Service.SignalR;

namespace FrontEndMovile
{
    public partial class AppShell : Shell
    {
        
        private readonly Notification_ServiceSignalR signalRClientService;
        private readonly INotificationInThePhone_Service notificationInThePhone_Service;


        public AppShell(Notification_ServiceSignalR signalRClientService, INotificationInThePhone_Service notification_Service)
        {
            InitializeComponent();

            this.signalRClientService = signalRClientService;
            this.notificationInThePhone_Service = notification_Service;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Iniciar la conexión
            await signalRClientService.StartConnectionAsync();

            // Escuchar mensajes
            this.signalRClientService.OnNewMessage(message =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                   
                    this.notificationInThePhone_Service.ShowNotification("El cole says", message);

                });
            });
        }


        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            // Detener la conexión
            await signalRClientService.StopConnectionAsync();
        }


    }
}
