using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationApi.Models
{
    public class NotificationPayload
    {
        public string DeviceId { get; set; }
        public string Message { get; set; }
        public string[] Tags { get; set; }
    }
}
