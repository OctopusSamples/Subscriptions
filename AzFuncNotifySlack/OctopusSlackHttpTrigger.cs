using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Octopus
{
    // Requires Environment variable of:
    //  - SLACK_URI_APIKEY - https://api.slack.com/authentication/basics
    public static class OctopusSlackHttpTrigger
    {
        [FunctionName("OctopusSlackHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var client = new SlackClient(Environment.GetEnvironmentVariable("SLACK_URI_APIKEY"));

            var data = await new StreamReader(req.Body).ReadToEndAsync();
            
            var octoMessage = JsonConvert.DeserializeObject<OctoMessage>(data);
            var slackMessage = string.Format(
                                    "{0} (by {1}) - <{2}|Go to Octopus>", 
                                    octoMessage.Message, 
                                    octoMessage.Username,
                                    octoMessage.GetSpaceUrl());

            try
            {
                var responseText = client.PostMessage(text: slackMessage);                
                return new OkObjectResult(responseText);
            }
            catch (System.Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}


