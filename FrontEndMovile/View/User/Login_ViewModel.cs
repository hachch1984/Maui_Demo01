
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dto;
using Dto.EndPointName;
using FrontEndMovile.Util;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;

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


        ObservableCollection<UserDocumentType_Dto_For_OnlyActives> _UserDocumentType_List = new ObservableCollection<UserDocumentType_Dto_For_OnlyActives>();
        public ObservableCollection<UserDocumentType_Dto_For_OnlyActives> UserDocumentType_List
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
        UserDocumentType_Dto_For_OnlyActives? _UserDocumentTypeId;
        public UserDocumentType_Dto_For_OnlyActives? UserDocumentTypeId
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


        public Login_ViewModel(IConnectivity connectivity, HttpClient httpClient, ISetting setting, AppShell appShell)
        {
            this.connectivity = connectivity;
            this.httpClient = httpClient;
            this.setting = setting;
            this.appShell = appShell;
            this.connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;



            Task.Run(async () => await LoadAsync());


        }


        private async Task LoadAsync()
        {
            try
            {

                var url = $"{this.setting.BackendApiUrl}{UserDocumentType_EndPointName.EndPointName}{UserDocumentType_EndPointName.GetAllOnlyActive}";
                var response = await this.httpClient.GetFromJsonAsync<List<UserDocumentType_Dto_For_OnlyActives>>(url);

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

                var objJson = new Token_Dto_For_Create
                {
                    UserDocumentTypeId = this.UserDocumentTypeId == null ? 0 : this.UserDocumentTypeId.Id,
                    UserDocumentValue = this.UserDocumentValue,
                    Password = this.Password
                };

                var response = await this.httpClient.PostAsJsonAsync(url, objJson);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadFromJsonAsync<Token_Dto_For_ShowInformation>();
                    if (token is not null)
                    {
                        //guardando informacion del token en el dispositivo
                        Preferences.Set(nameof(Token_Dto_For_ShowInformation.Token), token.Token);
                        Preferences.Set(nameof(Token_Dto_For_ShowInformation.Expiration), token.Expiration);
                        Preferences.Set(nameof(Token_Dto_For_ShowInformation.Email), token.Email);
                        Preferences.Set(nameof(Token_Dto_For_ShowInformation.Name), token.Name);
                        Preferences.Set(nameof(Token_Dto_For_ShowInformation.Id), token.Id.ToString());

                        //mostrando mensaje de bienvenida
                        await Application.Current.MainPage.DisplayAlert("Success", $"Bienvenido: {token.Name}", "Ok");

                        //redireccionando a la pagina principal
                        Application.Current.MainPage = this.appShell;
                    }
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    var dictionary = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(errorMessage);

                    if (dictionary != null)
                    {
                        foreach (var item in dictionary)
                        {
                            if (item.Key == nameof(Token_Dto_For_Create.UserDocumentTypeId))
                            {
                                this.UserDocumentTypeId_Error = item.Value.FirstOrDefault();
                            }
                            else if (item.Key == nameof(Token_Dto_For_Create.UserDocumentValue))
                            {
                                this.UserDocumentValue_Error = item.Value.FirstOrDefault();
                            }
                            else if (item.Key == nameof(Token_Dto_For_Create.Password))
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
