using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationApi.Models
{
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
}
