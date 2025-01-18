using Dto;
using FrontEndMovile.Service;
using FrontEndMovile.Service.SignalR;
using FrontEndMovile.View.User;

namespace FrontEndMovile
{
    public partial class AppShell : Shell
    {

        private readonly Notification_ServiceSignalR signalRClientService;
        private readonly INotificationInThePhone_Service notificationInThePhone_Service;
        public Login_Page Login_Page { get; set; }


        public AppShell(Notification_ServiceSignalR signalRClientService, INotificationInThePhone_Service notification_Service)
        {
            InitializeComponent();

            this.signalRClientService = signalRClientService;
            this.notificationInThePhone_Service = notification_Service;


            var user = Preferences.Get(nameof(Token_Dto_For_ShowInformation.Name), string.Empty);

            this.LblUser.Text = user;

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

        private void BnLogout_Clicked(object sender, EventArgs e)
        {
            // Clear preferences
            Preferences.Clear();
            this.Login_Page.Clear();
            // Restart application
            Application.Current.MainPage = this.Login_Page;
        }
    }
}
