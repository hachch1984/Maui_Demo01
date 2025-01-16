using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using FrontEndMovile.Service;

//FrontEndMovile.Platforms.Android
namespace FrontEndMovile.Platforms.Android_CustomCode
{
    public class Notification_Service : INotificationInThePhone_Service
    {
        private const string ChannelId = "default_channel";
        private const string ChannelName = "General Notifications";

        public Notification_Service()
        {
            CreateNotificationChannel();
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(ChannelId, ChannelName, NotificationImportance.Default)
                {
                    Description = "Canal de notificaciones generales"
                };

                var notificationManager = (NotificationManager)Android.App.Application.Context.GetSystemService(Context.NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }

        public void ShowNotification(string title, string message)
        {
            var notificationManager = NotificationManagerCompat.From(Android.App.Application.Context);

            var notification = new NotificationCompat.Builder(Android.App.Application.Context, ChannelId)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetPriority(NotificationCompat.PriorityMax)
                .SetSmallIcon(Resource.Drawable.notification1)
                .SetAutoCancel(true)
                .Build();

            notificationManager.Notify(new Random().Next(), notification);
        }
    }
}
