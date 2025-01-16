using FrontEndMovile.Service;
using FrontEndMovile.Service.SignalR;
using System.Collections.ObjectModel;

namespace FrontEndMovile.View;

public partial class NotificationPage : ContentPage
{
    private readonly Notification_ServiceSignalR signalRClientService;
    private readonly INotificationInThePhone_Service notification_Service;

    public ObservableCollection<string> Notifications { get; } = [];

    public NotificationPage(Notification_ServiceSignalR signalRClientService, INotificationInThePhone_Service notification_Service)
    {
        InitializeComponent();
        this.signalRClientService = signalRClientService;
        this.notification_Service = notification_Service;
        NotificationsList.ItemsSource = Notifications;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Iniciar la conexión
        await signalRClientService.StartConnectionAsync();

        // Escuchar mensajes
        this.signalRClientService.OnNewMessage(message =>
           {
               MainThread.BeginInvokeOnMainThread(() =>
               {
                   Notifications.Add(message);

                   this.notification_Service.ShowNotification("El cole says", message);

               });
           });
    }
    //~NotificationPage()
    //{
    //    // Detener la conexión
    //    signalRClientService.StopConnectionAsync().ConfigureAwait(false);
    //}
    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        // Detener la conexión
        await signalRClientService.StopConnectionAsync();
    }
}