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
    public static class UnregisterDevice
    {
        [FunctionName("UnregisterDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string installationId = req.Query["installationId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            installationId = installationId ?? data?.installationid;

            var hub = NotificationHubClient.CreateClientFromConnectionString(Constants.NotificationConnectionString, Constants.NotificationHubName);

            hub.DeleteInstallation(installationId);

            string responseMessage = string.IsNullOrEmpty(installationId)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {installationId}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
