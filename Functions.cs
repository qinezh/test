using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerlessDraw
{
    public static class Functions
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "serverlesseditor")] SignalRConnectionInfo connectionInfo,
            ILogger log)
        {
            return connectionInfo;
        }

        [FunctionName("update")]
        public static async Task Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalR(HubName = "serverlesseditor")] IAsyncCollector<SignalRMessage> messages)
        {
            var json = await req.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            await messages.AddAsync(new SignalRMessage
            {
                Target = "contentUpdated",
                Arguments = new[] { data }
            });
        }
    }
}
