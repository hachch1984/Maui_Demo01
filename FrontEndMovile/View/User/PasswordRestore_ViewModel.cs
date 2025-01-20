using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dto.EndPointName;
using Dto;
using System.Net.Http.Json;
using FrontEndMovile.Util;

namespace FrontEndMovile.View.User
{
    public partial class PasswordRestore_ViewModel : ObservableObject
    {

        private readonly HttpClient httpClient;
        private readonly IConnectivity connectivity;
        private readonly ISetting setting;
        string _Email;
        public string Email
        {
            get => _Email;
            set
            {
                _Email = value;
                OnPropertyChanged();
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    this.Email_Error = string.Empty;
                }
            }
        }


        string _Email_Error;
        public string Email_Error
        {
            get => _Email_Error;
            set => SetProperty(ref _Email_Error, value);
        }



        public PasswordRestore_ViewModel(HttpClient httpClient, IConnectivity connectivity, ISetting setting)
        {
            this.httpClient = httpClient;
            this.connectivity = connectivity;
            this.setting = setting;

            this.connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

        }

        private void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            this.Command_RestoreMethodCommand.NotifyCanExecuteChanged();
        }
        private bool StatusConnection => connectivity.NetworkAccess == NetworkAccess.Internet;


        [RelayCommand(CanExecute = nameof(StatusConnection))]
        private async Task Command_RestoreMethod()
        {
            try
            {

                this.Email_Error = string.Empty;

                //preparando la url
                var url = $"{this.setting.BackendApiUrl}{User_EndPointName.EndPointName}{User_EndPointName.PasswordRestore}";

                var objJson = new User_Dto_For_PasswordRestore
                {
                    Email = this.Email
                };

                var response = await this.httpClient.PostAsJsonAsync(url, objJson);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Information", $"favor de revisar su email ({this.Email}), se envio un link para resetear su contraseña", "Ok");

                    await Shell.Current.GoToAsync($"//{nameof(Login_Page)}");
                }
                else
                {

                    foreach (var item in await response.Content.GetErrorDictionaryAsync())
                    {
                        if (item.Key == nameof(Dto.User_Dto_For_PasswordRestore.Email))
                        {
                            this.Email_Error = item.Value.FirstOrDefault();
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("System Error", ex.Message, "Ok");
            }

        }


    }
}
