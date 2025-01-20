using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontEndMovile.View.User;

namespace FrontEndMovile
{
    public partial class AppShell_ViewModel:ObservableObject
    {

      


         


        [RelayCommand]
        private async void BnLogout_Clicked()
        {
            Preferences.Clear();
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            await Shell.Current.GoToAsync($"//{ nameof(  Login_Page)}");
        }



        public AppShell_ViewModel( )
        {
           
          


        }


    }
}
