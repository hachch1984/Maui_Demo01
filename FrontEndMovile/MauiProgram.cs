using Dto;
using Dto.EndPointName;
using FrontEndMovile.Service;
using FrontEndMovile.Service.SignalR;
using FrontEndMovile.Util;
using FrontEndMovile.View.User;
using FrontEndMovile.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
            //  builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddTransient<AuthHandler>();
            builder.Services.AddHttpClient("AuthorizedClient")
                            .AddHttpMessageHandler<AuthHandler>();











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




    public class AuthHandler : DelegatingHandler
    {
        private readonly ISetting setting;

        //private readonly AuthenticationService _authenticationService;

        public AuthHandler(//AuthenticationService authenticationService
            ISetting setting
            )
        {
            this.setting = setting;
            //  _authenticationService = authenticationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Uri requestUrl = request.RequestUri;

            var urlLogin = $"{this.setting.BackendApiUrl}{User_EndPointName.EndPointName}{User_EndPointName.TokenCreation}";
            var urlPasswordRestore = $"{this.setting.BackendApiUrl}{User_EndPointName.EndPointName}{User_EndPointName.PasswordRestore}";


            if (requestUrl != null &&
                requestUrl.ToString().Contains(urlLogin) == false &&
                request.ToString().Contains(urlPasswordRestore) == false)
            {
                // Lógica específica para una URL determinada

                if (TokenNeedsRefresh())
                {
                    var refreshTokenSuccess = await TryRefreshTokenAsync();
                    if (refreshTokenSuccess == false)
                    {
                        // Si el refresco del token falla, redirige al login
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await Application.Current.MainPage.DisplayAlert("Information", $"su seccion a expirado, favor de logearse de nuevo", "Ok");

                            await Shell.Current.GoToAsync($"{nameof(Login_Page)}", true);
                        });
                        // Cancela el llamado HTTP y evita generar el error
                        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                        {
                            RequestMessage = request,
                            ReasonPhrase = "Unauthorized request, and token refresh failed."
                        };
                        return response;
                    }
                }

                // Configura el nuevo token en la solicitud si el token fue refrescado exitosamente
                var token = Preferences.Get(nameof(Token_Dto_For_ShowInformation.Token), string.Empty);
                if (string.IsNullOrEmpty(token) == false)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
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
