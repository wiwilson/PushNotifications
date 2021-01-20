using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationHubFunctionApp
{
    public static class Constants
    {
        public const string NotificationConnectionString = "Endpoint=sb://notificationhub-jh.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=r95q95IrMw7jqVwUVB4XUrTvFCYloznEwUe6YkdjRZ8=";
        public const string NotificationHubName = "NotificationHubPoC-JH";
    }

    public class DeviceRegistration
    {
        public string InstallationId { get; set; }
        public Platforms Platform { get; set; }
        public string ChannelName { get; set; }
        public string[] Tags { get; set; }

        public enum Platforms
        {
            apns,
            fcm
        }

    }

    public class NotificationPayload
    {
        public string DeviceId { get; set; }
        public string Message { get; set; }
        public string[] Tags { get; set; }
    }

}
