using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.NotificationHubs;

namespace NotificationHubFunctionApp
{
    public static class PushNotification
    {


        private const string FcmSampleNotificationContent = "{\"data\":{\"message\":\"{0}\"}}";
        private const string FcmSampleSilentNotificationContent = "{ \"message\":{\"data\":{ \"Nick\": \"Mario\", \"body\": \"great match!\", \"Room\": \"PortugalVSDenmark\" } }}";
        private const string AppleSampleNotificationContent = "{\"aps\":{\"alert\":\"{0}\"}}";
        private const string AppleSampleSilentNotificationContent = "{\"aps\":{\"content-available\":1}, \"foo\": 2 }";

        [FunctionName("PushNotification")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            NotificationPayload notification = JsonConvert.DeserializeObject<NotificationPayload>(requestBody);

            var hub = NotificationHubClient.CreateClientFromConnectionString(Constants.NotificationConnectionString, Constants.NotificationHubName);

            if (notification.DeviceId != null)
            {
                await hub.SendDirectNotificationAsync(new FcmNotification("{\"data\":{\"message\":\"" + notification.Message + "\"}}"), notification.DeviceId);
                //await hub.SendDirectNotificationAsync(new AppleNotification("{\"aps\":{\"alert\":\"" + notification.Message + "\"}}"), notification.DeviceId);
            }
            else
            {
                await hub.SendFcmNativeNotificationAsync("{\"data\":{\"message\":\"" + notification.Message + "\"}}", notification.Tags);
                //await hub.SendAppleNativeNotificationAsync("{\"aps\":{\"alert\":\"" + notification.Message + "\"}}", notification.Tags);
            }

            return new OkResult();
        }

    }
}
