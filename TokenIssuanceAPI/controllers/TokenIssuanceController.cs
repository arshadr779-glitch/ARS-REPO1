using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace TokenIssuanceAPI.Controllers
{
    [ApiController]
    [Route("api/tokenissuance")]
    public class TokenIssuanceController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] JObject request)
        {
            // ASP.NET Core handles reading Request.Body automatically into 'request'
            if (request == null)
            {
                return BadRequest("Invalid JSON payload.");
            }

            // Print the incoming payload to the console logs
            Console.WriteLine(request.ToString());

            // Safely extract the correlation ID using null-conditional filtering
            var correlationId = request["data"]?["authenticationContext"]?["correlationId"]?.ToString();

            // Construct the required Microsoft Graph response object
            var response = new JObject
            {
                ["data"] = new JObject
                {
                    ["@odata.type"] = "microsoft.graph.onTokenIssuanceStartResponseData",
                    ["actions"] = new JArray
                    {
                        new JObject
                        {
                            ["@odata.type"] = "microsoft.graph.tokenIssuanceStart.provideClaimsForToken",
                            ["claims"] = new JObject
                            {
                                ["CorrelationId"] = correlationId,
                                ["ApiVersion"] = "1.0.0",
                                ["DateOfBirth"] = "01/01/2000",
                                ["CustomRoles"] = new JArray { "Writer", "Editor" }
                            }
                        }
                    }
                }
            };

            // Return the structured JSON content
            return Content(response.ToString(), "application/json");
        }
    }
}
