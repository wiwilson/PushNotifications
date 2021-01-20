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
using static NotificationHubFunctionApp.DeviceRegistration;

namespace NotificationHubFunctionApp
{
    public static class RegisterDevice
    {
        [FunctionName("RegisterDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DeviceRegistration data = JsonConvert.DeserializeObject<DeviceRegistration>(requestBody);
            
            string token = req.Query["token"];
            token = token ?? data?.ChannelName;
            if (token == null)
            {
                return new BadRequestObjectResult("Please pass a token (\"token\") on the query string or in the request body");
            }

            var hub = NotificationHubClient.CreateClientFromConnectionString(Constants.NotificationConnectionString, Constants.NotificationHubName);

            var installation = new Installation
            {
                InstallationId = data.InstallationId,
                Platform = data.Platform == Platforms.apns ? NotificationPlatform.Apns : NotificationPlatform.Fcm,
                PushChannel = token
            };

            if (data.Tags.Length > 0)
            {
                var tagsList = new ArraySegment<string>(data.Tags);
                installation.Tags = tagsList;
            }

            try
            {
                hub.CreateOrUpdateInstallation(installation);
                var response = $"{{ \"installationId\" = \"{installation.InstallationId}\" }}";

                return new OkObjectResult(response);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);

            }
        }
    }
}
