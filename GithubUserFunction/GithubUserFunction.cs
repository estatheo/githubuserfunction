using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GithubUserFunction
{
    public static class GithubUserFunction
    {
        [FunctionName("GithubUserFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var user = req.Query["user"];

            log.LogInformation($"GithubUserFunction processed a request for user:{user}");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "GithubUserFunction");

            var result = await client.GetAsync($"https://api.github.com/users/{user}");
            var jsonResult = result.Content.ReadAsStringAsync();

            return jsonResult != null
                ? (ActionResult)new OkObjectResult(jsonResult)
                : new BadRequestObjectResult("Please pass a valid github user name on the user query parameter.");
        }
    }
}
