
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dto;
using Dto.EndPointName;
using FrontEndMovile.Util;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace FrontEndMovile.ViewModel
{


    public partial class Login_ViewModel : ObservableObject
    {
        #region private properties
        private readonly IConnectivity connectivity;
        private readonly HttpClient httpClient;
        private readonly ISetting setting;
        private readonly AppShell appShell;

        #endregion







        #region observable properties

        [ObservableProperty]
        string email;
        [ObservableProperty]
        string password;


        ObservableCollection<UserDocumentType_Dto_For_OnlyActives>DocumentType = new ObservableCollection<UserDocumentType_Dto_For_OnlyActives>();

        #endregion


        public Login_ViewModel(IConnectivity connectivity, HttpClient httpClient, ISetting setting,AppShell appShell )
        {
            this.connectivity = connectivity;
            this.httpClient = httpClient;
            this.setting = setting;
            this.appShell = appShell;
            this.connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;



        }

        private void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            // throw new NotImplementedException();
            Command_LoginMethodCommand.NotifyCanExecuteChanged();
        }

        private bool StatusConnection => connectivity.NetworkAccess == NetworkAccess.Internet;

        public async Task<bool> CallEndPoint_Login(string email, string password)
        {
            try
            {


                using var client = new HttpClient();
                var url =  $"{this.setting.BackendApiUrl}{User_EndPointName.EndPointName}{User_EndPointName.TokenCreation}";
                var response = await client.PostAsJsonAsync(url, new { email, password });

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadFromJsonAsync<Token_Dto_For_ShowInformation>();
                    if (token is not null)
                    {
                        Preferences.Set(nameof(Token_Dto_For_ShowInformation.Token), token.Token);
                        Preferences.Set(nameof(Token_Dto_For_ShowInformation.Expiration), token.Expiration);
                        Preferences.Set(nameof(Token_Dto_For_ShowInformation.Email), token.Email);
                        Preferences.Set(nameof(Token_Dto_For_ShowInformation.Name), token.Name);
                        Preferences.Set(nameof(Token_Dto_For_ShowInformation.Id), token.Id.ToString());

                        //Save the token in the local storage
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [RelayCommand(CanExecute = nameof(StatusConnection))]
        private async Task Command_LoginMethod()
        {
            var result = await CallEndPoint_Login(Email, Password);
            if (result)
            {
              
                await Application.Current.MainPage.DisplayAlert("Success", "Login successful, welcome: " + Preferences.Get(nameof(Token_Dto_For_ShowInformation.Name), ""), "Ok");
                Application.Current.MainPage = this.appShell;
            }
            else
            {
              
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password", "Ok");
            }
        }



        
    }
}
