using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontEndMovile.Service.SignalR;
using FrontEndMovile.View.User;

namespace FrontEndMovile
{
    public partial class AppShell_ViewModel:ObservableObject
    {
        private readonly INotification_ServiceSignalR notification_ServiceSignalR;

        [RelayCommand]
        private async void BnLogout_Clicked()
        {
            await this.notification_ServiceSignalR.StopConnectionAsync();

            SecureStorage.RemoveAll();

            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            await Shell.Current.GoToAsync($"//{ nameof(  Login_Page)}", true);
        }



        public AppShell_ViewModel( INotification_ServiceSignalR notification_ServiceSignalR)
        {
            this.notification_ServiceSignalR = notification_ServiceSignalR;
        }


    }
}
