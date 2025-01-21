using Dto.EndPointName;
using Dto.Ges.User;
using FrontEndMovile.Service;
using FrontEndMovile.Service.SignalR;
using FrontEndMovile.Util;
using FrontEndMovile.View.User;
using FrontEndMovile.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;

namespace FrontEndMovile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            #region configuracion del objeto setting en funcion al archivo appsettings.json

            var assemblyInstance = Assembly.GetExecutingAssembly();
            using var stream = assemblyInstance.GetManifestResourceStream("FrontEndMovile.appsettings.json");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            var setting = config.GetSection("Setting").Get<Setting>();

            #endregion

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });




            builder.Services.AddSingleton<ISetting>(setting!);


            //signalR
            builder.Services.AddSingleton<INotification_ServiceSignalR, Notification_ServiceSignalR>();

            // Registrar servicios específicos de Android
#if ANDROID

            builder.Services.AddSingleton<INotificationInThePhone_Service, FrontEndMovile.Platforms.Android_CustomCode.Notification_Service>();
#endif


            //conectividad https
            builder.Services.AddSingleton(Connectivity.Current);

            builder.Services.AddTransient<HttpClientAuthHandler>();
            builder.Services.AddHttpClient(HttpClientAuthHandler.Name)
                            .AddHttpMessageHandler<HttpClientAuthHandler>();











            #region pages and viewmodels

            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<AppShell_ViewModel>();


            builder.Services.AddTransient<Login_Page>();
            builder.Services.AddTransient<Login_ViewModel>();


            builder.Services.AddTransient<PasswordRestore_Page>();
            builder.Services.AddTransient<PasswordRestore_ViewModel>();

            #endregion




#if DEBUG
            builder.Logging.AddDebug();
#endif




            return builder.Build();
        }
    }



    public static class IHttpClientFactory_Extension
    {
        public static HttpClient CreateAuthorizedClient(this IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient(HttpClientAuthHandler.Name);
        }
    }


    public class HttpClientAuthHandler : DelegatingHandler
    {
        public static string Name => "AuthorizedClient";

        private readonly ISetting setting;

        //private readonly AuthenticationService _authenticationService;

        public HttpClientAuthHandler(//AuthenticationService authenticationService
            ISetting setting
            )
        {
            this.setting = setting;
            //  _authenticationService = authenticationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestUrl = request.RequestUri.ToString();

            var urlLogin = $"{this.setting.BackendApiUrl}{User_EndPointName.EndPointName}{User_EndPointName.TokenCreation}";
            var urlPasswordRestore = $"{this.setting.BackendApiUrl}{User_EndPointName.EndPointName}{User_EndPointName.PasswordRestore}";
            var urlUserDocumentType = $"{this.setting.BackendApiUrl}{UserDocumentType_EndPointName.EndPointName}{UserDocumentType_EndPointName.GetAllOnlyActive}";



            //if (requestUrl != null &&
            //    requestUrl.ToString().Contains(urlLogin) == false &&
            //    request.ToString().Contains(urlPasswordRestore) == false)
            //{
            //    // Lógica específica para una URL determinada

            //    if (TokenNeedsRefresh())
            //    {
            //        var refreshTokenSuccess = await TryRefreshTokenAsync();
            //        if (refreshTokenSuccess == false)
            //        {
            //            // Si el refresco del token falla, redirige al login
            //            MainThread.BeginInvokeOnMainThread(async () =>
            //            {
            //                await Application.Current.MainPage.DisplayAlert("Information", $"su seccion a expirado, favor de logearse de nuevo", "Ok");

            //                await Shell.Current.GoToAsync($"{nameof(Login_Page)}", true);
            //            });
            //            // Cancela el llamado HTTP y evita generar el error
            //            var response2 = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //            {
            //                RequestMessage = request,
            //                ReasonPhrase = "Unauthorized request, and token refresh failed."
            //            };
            //            return response2;
            //        }
            //    }




            //  }


            var token = await SecureStorage.GetAsync(nameof(Token_Created.Token));

            if (string.IsNullOrEmpty(token) == false  &&  
                requestUrl.Contains(urlLogin) == false &&
                requestUrl.Contains(urlPasswordRestore) == false &&
                requestUrl.Contains(urlUserDocumentType) == false
                )
            {
                if (IsTokenExpired(token))
                {
                    return UnauthorizedRequest(request, $"su seccion a expirado, favor de ingresar nuevamente sus credenciales");
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return UnauthorizedRequest(request, $"acceso no autorizado, favor de ingresar nuevamente sus credenciales de acceso");
            }

            return response;
        }

        private HttpResponseMessage UnauthorizedRequest(HttpRequestMessage request, string message)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Atencion", $"acceso no autorizado, favor de ingresar nuevamente sus credenciales de acceso", "Ok");

                await Shell.Current.GoToAsync($"{nameof(Login_Page)}", true);
            });

            // Cancela el llamado HTTP y evita generar el error
            return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = request,
                ReasonPhrase = "Unauthorized request"
            };
        }


        public static bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var result= jwtToken.ValidTo < DateTime.UtcNow;
            return result;
        }

        private bool TokenNeedsRefresh()
        {
            // var dateEnd = Preferences.Get(nameof(Token_Dto_For_ShowInformation.Expiration),null);

            // Implementa la lógica para determinar si el token necesita ser refrescado
            return false;// _authenticationService.IsTokenExpired();
        }

        private async Task<bool> TryRefreshTokenAsync()
        {
            try
            {
                //  SecureStorage.GetAsync(nameof(Token_Created.RefreshToken));
                return await Task.FromResult(true);//  await _authenticationService.RefreshTokenAsync();
            }
            catch
            {
                // Log the exception if needed
                return false;
            }
        }
    }

}
