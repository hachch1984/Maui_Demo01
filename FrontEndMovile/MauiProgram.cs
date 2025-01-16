using FrontEndMovile.Service;
using FrontEndMovile.Service.SignalR;
using FrontEndMovile.Util;
using FrontEndMovile.View.User;
using FrontEndMovile.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
            builder.Services.AddSingleton<Notification_ServiceSignalR>();

            // Registrar servicios específicos de Android
#if ANDROID
        
            builder.Services.AddSingleton<INotificationInThePhone_Service, FrontEndMovile.Platforms.Android_CustomCode.Notification_Service>();
#endif


            //conectividad https
            builder.Services.AddSingleton(Connectivity.Current);
            builder.Services.AddSingleton<HttpClient>();












            #region pages and viewmodels

            builder.Services.AddTransient<AppShell>();


            builder.Services.AddTransient<Login_Page>();
            builder.Services.AddTransient<Login_ViewModel>();

            #endregion




#if DEBUG
            builder.Logging.AddDebug();
#endif




            return builder.Build();
        }
    }
}
