    C#
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    namespace TokenIssuanceAPI.Controllers
    {
    [ApiController]
    [Route("api/tokenissuance")]
    public class TokenIssuanceController : ControllerBase
    {
    [HttpPost]
    public async Task<IActionResult> Post()
    {
    string body;
    using (var reader = new StreamReader(Request.Body))
    {
    body = await reader.ReadToEndAsync();
    }
    Console.WriteLine(body);
    var request =
    JsonConvert.DeserializeObject<JObject>(body);
    var correlationId =
    request?["data"]?["authenticationContext"]?["correlationId"]?.ToString();
    var response = new JObject
    {
    ["data"] = new JObject
    {
    ["@odata.type"] =
    "microsoft.graph.onTokenIssuanceStartResponseData",
    ["actions"] = new JArray
    {
    new JObject
    {
    ["@odata.type"] =
    "microsoft.graph.tokenIssuanceStart.provideClaimsForToken",
    ["claims"] = new JObject
    {
    ["CorrelationId"] = correlationId,
    ["ApiVersion"] = "1.0.0",
    ["DateOfBirth"] = "01/01/2000",
    ["CustomRoles"] = new JArray
    {
    "Writer",
    "Editor"
    }
    }
    }
    }
    }
    };
    return Content(
    response.ToString(),
    "application/json");
    }
    }
    }
