using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Clenka.BenElvisPoc.FuncApp
{
    public class SaveRsvp
    {
        private readonly ILogger<SaveRsvp> _logger;

        public SaveRsvp(ILogger<SaveRsvp> logger)
        {
            _logger = logger;
        }

        [Function("SaveRsvp")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
