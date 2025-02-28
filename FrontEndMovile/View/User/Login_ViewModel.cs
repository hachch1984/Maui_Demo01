﻿
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dto.EndPointName;
using Dto.Ges.User;
using FrontEndMovile.Service.SignalR;
using FrontEndMovile.Util;
using FrontEndMovile.View.User;
using FrontEndMovile.View.Varios;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;

namespace FrontEndMovile.ViewModel
{


    public partial class Login_ViewModel : ObservableObject
    {
        #region private properties
        private readonly IConnectivity connectivity;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ISetting setting;
        private readonly INotification_ServiceSignalR notification_ServiceSignalR;


        #endregion


        #region observable properties


        ObservableCollection<UserDocumentType_ShowInformation> _UserDocumentType_List = new ObservableCollection<UserDocumentType_ShowInformation>();
        public ObservableCollection<UserDocumentType_ShowInformation> UserDocumentType_List
        {
            get
            {
                return _UserDocumentType_List;
            }
            set
            {
                _UserDocumentType_List = value;
                OnPropertyChanged();
            }
        }
        UserDocumentType_ShowInformation? _UserDocumentTypeId;
        public UserDocumentType_ShowInformation? UserDocumentTypeId
        {
            get { return _UserDocumentTypeId; }
            set
            {
                _UserDocumentTypeId = value;
                OnPropertyChanged();
                if (value != null)
                {
                    this.UserDocumentTypeId_Error = "";
                }
            }
        }
        string _UserDocumentTypeId_Error;
        public string UserDocumentTypeId_Error
        {
            get { return _UserDocumentTypeId_Error; }
            set
            {
                _UserDocumentTypeId_Error = value;
                OnPropertyChanged();
            }
        }




        string _UserDocumentValue;
        public string UserDocumentValue
        {
            get
            {
                return _UserDocumentValue;
            }
            set
            {
                _UserDocumentValue = value;
                OnPropertyChanged();
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    this.UserDocumentValue_Error = "";
                }
            }
        }
        string _UserDocumentValue_Error;
        public string UserDocumentValue_Error
        {
            get { return _UserDocumentValue_Error; }
            set
            {
                _UserDocumentValue_Error = value;
                OnPropertyChanged();
            }
        }




        string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged();
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    this.Password_Error = "";
                }
            }
        }
        string _Password_Error;
        public string Password_Error
        {
            get { return _Password_Error; }
            set
            {
                _Password_Error = value;
                OnPropertyChanged();
            }
        }

        #endregion



        public ICommand Cmd_Btn_PasswordRestore { get; }

        private async void Cmd_Btn_PasswordRestore_Execute()
        {
            await Shell.Current.GoToAsync($"//{nameof(PasswordRestore_Page)}", true);
        }

        public Login_ViewModel(IConnectivity connectivity, IHttpClientFactory httpClientFactory, ISetting setting,
            AppShell appShell,
            PasswordRestore_Page passwordRestore_Page,
            INotification_ServiceSignalR notification_ServiceSignalR
            )
        {
            this.connectivity = connectivity;
            this.httpClientFactory = httpClientFactory;
            this.setting = setting;
            this.notification_ServiceSignalR = notification_ServiceSignalR;
            this.connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;


            this.Cmd_Btn_PasswordRestore = new RelayCommand(Cmd_Btn_PasswordRestore_Execute);


            Task.Run(async () => await LoadAsync());


        }


        private async Task LoadAsync()
        {
            try
            {

                var url = $"{this.setting.BackendApiUrl}{UserDocumentType_EndPointName.EndPointName}{UserDocumentType_EndPointName.GetAllOnlyActive}";

                using var httpClient = this.httpClientFactory.CreateAuthorizedClient();

                var response = await httpClient.GetFromJsonAsync<List<UserDocumentType_ShowInformation>>(url);

                if (response is not null)
                {

                    UserDocumentType_List.Clear();

                    foreach (var item in response)
                    {
                        UserDocumentType_List.Add(item);
                    }

                    this.UserDocumentValue = string.Empty;
                    this.Password = string.Empty;
                    this.UserDocumentTypeId = null;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }



        private void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            // throw new NotImplementedException();
            Command_LoginMethodCommand.NotifyCanExecuteChanged();
        }

        private bool StatusConnection => connectivity.NetworkAccess == NetworkAccess.Internet;

        public string UserDocumentValue_Error1 { get => _UserDocumentValue_Error; set => _UserDocumentValue_Error = value; }

        [RelayCommand(CanExecute = nameof(StatusConnection))]
        private async Task Command_LoginMethod()
        {
            try
            {
                //limpiando errores
                this.Password_Error = string.Empty;
                this.UserDocumentValue_Error = string.Empty;
                this.UserDocumentTypeId_Error = string.Empty;



                //preparando la url
                var url = $"{this.setting.BackendApiUrl}{User_EndPointName.EndPointName}{User_EndPointName.TokenCreation}";

                var objJson = new Token_Creation
                {
                    UserDocumentTypeId = this.UserDocumentTypeId == null ? 0 : this.UserDocumentTypeId.Id,
                    UserDocumentValue = this.UserDocumentValue,
                    Password = this.Password
                };

                using var httpClient = this.httpClientFactory.CreateAuthorizedClient();
                var response = await httpClient.PostAsJsonAsync(url, objJson);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadFromJsonAsync<Token_Created>();
                    if (token is not null)
                    {
                        //guardando informacion del token en el dispositivo
                        //Preferences.Set(nameof(Token_Created.Token), token.Token);
                        //Preferences.Set(nameof(Token_Created.Expiration), token.Expiration);
                        //Preferences.Set(nameof(Token_Created.Email), token.Email);
                        //Preferences.Set(nameof(Token_Created.Name), token.Name);
                        //Preferences.Set(nameof(Token_Created.Id), token.Id.ToString());

                        await SecureStorage.SetAsync(nameof(Token_Created.Token), token.Token);
                        await SecureStorage.SetAsync(nameof(Token_Created.Id), token.Id.ToString());


                        //redireccionando a la pagina principal

                        await this.notification_ServiceSignalR.StartConnectionAsync(token.Token, token.Id.ToString());

                        AppShell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

                        await Shell.Current.GoToAsync($"//{nameof(Wellcome_Page)}", true);

                    }
                }
                else
                {
                    //   var errorMessage = await response.Content.ReadAsStringAsync();

                    var dictionary = await response.Content.GetErrorDictionaryAsync();  // JsonSerializer.Deserialize<Dictionary<string, List<string>>>(errorMessage);

                    if (dictionary != null)
                    {
                        foreach (var item in dictionary)
                        {
                            if (item.Key == nameof(Token_Creation.UserDocumentTypeId))
                            {
                                this.UserDocumentTypeId_Error = item.Value.FirstOrDefault();
                            }
                            else if (item.Key == nameof(Token_Creation.UserDocumentValue))
                            {
                                this.UserDocumentValue_Error = item.Value.FirstOrDefault();
                            }
                            else if (item.Key == nameof(Token_Creation.Password))
                            {
                                this.Password_Error = item.Value.FirstOrDefault();
                            }
                        }
                    }

                    //  await Application.Current.MainPage.DisplayAlert("Error", errorMessage, "Ok");
                }


            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("System Error", ex.Message, "Ok");
            }

        }





    }
}
