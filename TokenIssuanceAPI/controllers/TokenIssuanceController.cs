using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TokenIssuanceAPI.Controllers
{
    [ApiController]
    [Route("api/tokenissuance")]
    public class TokenIssuanceController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] JsonNode request)
        {
            if (request == null)
            {
                return BadRequest("Invalid JSON payload.");
            }

            // Log incoming request payload
            Console.WriteLine(request.ToJsonString());

            // Safely extract correlationId from incoming payload
            string correlationId = request?["data"]?["authenticationContext"]?["correlationId"]?.GetValue<string>() ?? string.Empty;

            // Construct the exact response schema expected by Microsoft Entra ID
            var response = new
            {
                data = new
                {
                    odataType = "microsoft.graph.onTokenIssuanceStartResponseData",
                    actions = new[]
                    {
                        new
                        {
                            odataType = "microsoft.graph.tokenIssuanceStart.provideClaimsForToken",
                            claims = new
                            {
                                correlationId = correlationId,
                                apiVersion = "1.0.0",
                                dateOfBirth = "01/01/2000",
                                customRoles = new[] { "Writer", "Editor" }
                            }
                        }
                    }
                }
            };

            // Options to ensure @odata.type properties format with exact @ prefix
            var responseJson = new JsonObject
            {
                ["data"] = new JsonObject
                {
                    ["@odata.type"] = "microsoft.graph.onTokenIssuanceStartResponseData",
                    ["actions"] = new JsonArray
                    {
                        new JsonObject
                        {
                            ["@odata.type"] = "microsoft.graph.tokenIssuanceStart.provideClaimsForToken",
                            ["claims"] = new JsonObject
                            {
                                ["correlationId"] = correlationId,
                                ["apiVersion"] = "1.0.0",
                                ["dateOfBirth"] = "01/01/2000",
                                ["customRoles"] = new JsonArray { "Writer", "Editor" }
                            }
                        }
                    }
                }
            };

            return Content(responseJson.ToJsonString(), "application/json");
        }
    }
}
