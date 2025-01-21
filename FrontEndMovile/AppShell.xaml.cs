using Dto.Ges.User;
using FrontEndMovile.Service;
using FrontEndMovile.Service.SignalR;
using FrontEndMovile.View.User;
using FrontEndMovile.View.Varios;

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







            this.notification_ServiceSignalR.OnNewMessage(message =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {

                    this.notificationInThePhone_Service.ShowNotification("The school says", message);

                });
            });

            Task.Factory.StartNew(async () =>
             {






              



             });

        }




        protected override async void OnAppearing()
        {
            base.OnAppearing();


            var token = await SecureStorage.GetAsync(nameof(Token_Created.Token));
            var userId = await SecureStorage.GetAsync(nameof(Token_Created.Id));



            if (string.IsNullOrEmpty(token) == false && HttpClientAuthHandler.IsTokenExpired(token) == false)
            {

                await this.notification_ServiceSignalR.StartConnectionAsync(token, userId);
                AppShell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
                await Shell.Current.GoToAsync($"//{nameof(Wellcome_Page)}", true);
            }
            else
            {
                AppShell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
                await Shell.Current.GoToAsync($"//{nameof(Login_Page)}", true);
            }

        }


        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            await this.notification_ServiceSignalR.StopConnectionAsync();
        }


    }
}
