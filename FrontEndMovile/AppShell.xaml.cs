using FrontEndMovile.Service;
using FrontEndMovile.Service.SignalR;
using FrontEndMovile.View.User;

namespace FrontEndMovile
{
    public partial class AppShell : Shell
    {
        private readonly INotification_ServiceSignalR notification_ServiceSignalR;
        private readonly INotificationInThePhone_Service notificationInThePhone_Service;

        public AppShell(AppShell_ViewModel appShell_ViewModel,
            INotification_ServiceSignalR notification_ServiceSignalR,
            INotificationInThePhone_Service notificationInThePhone_Service)
        {
            InitializeComponent();

            BindingContext = appShell_ViewModel;

            this.notification_ServiceSignalR = notification_ServiceSignalR;
            this.notificationInThePhone_Service = notificationInThePhone_Service;
        }




        protected override async void OnAppearing()
        {
            base.OnAppearing();

          



            this.notification_ServiceSignalR.OnNewMessage(message =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {

                    this.notificationInThePhone_Service.ShowNotification("The school says", message);

                });
            });


            var accessToken = Preferences.Get("accesstoken", string.Empty);

            if (string.IsNullOrEmpty(accessToken))
            {
                await Shell.Current.GoToAsync($"//{nameof(Login_Page)}",true);
            }

        }


        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            await this.notification_ServiceSignalR.StopConnectionAsync();
        }


    }
}
