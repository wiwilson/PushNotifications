using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        HttpClient httpClient;

        public static Notifications notifications = new Notifications();

        public NotificationController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://192.168.50.145:7071/api/");
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (notifications != null)
            {
                return Ok(notifications);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public async Task<IActionResult> RegisterDevice(DeviceRegistration deviceRegistration)
        {
            StringContent stringContent = new StringContent(JsonSerializer.Serialize(deviceRegistration), encoding: Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("RegisterDevice", stringContent);
            if(result.IsSuccessStatusCode)
            {
                notifications.DeviceRegistrations.Add(deviceRegistration);
                return Ok(deviceRegistration);
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> PushNotification(NotificationPayload notificationPayload)
        {
            StringContent stringContent = new StringContent(JsonSerializer.Serialize(notificationPayload), encoding: Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("PushNotification", stringContent);
            if(result.IsSuccessStatusCode)
            {
                notifications.NotificationPayloads.Add(notificationPayload);
                return Ok(notificationPayload);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> UnregisterDevice(string installationId)
        {
            var result = await httpClient.GetAsync($"UnregisterDevice?installationId={installationId}");
            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        public class NotificationActionResult
        {
            public string Text { get; set; }
        }
    }
}
