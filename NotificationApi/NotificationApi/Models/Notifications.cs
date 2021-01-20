using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationApi.Models
{
    public class Notifications
    {
        public List<DeviceRegistration> DeviceRegistrations { get; set; }
        public List<NotificationPayload> NotificationPayloads { get; set; }
        public Notifications()
        {
            DeviceRegistrations = new List<DeviceRegistration>();
            NotificationPayloads = new List<NotificationPayload>();
        }
    }
}
