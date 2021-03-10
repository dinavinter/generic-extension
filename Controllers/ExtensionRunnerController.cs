using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Filters;

namespace GenericExtesion.Controllers
{
    [ApiController]
    [Route("extension/run")]
    public class ExtensionRunnerController : ControllerBase
    {
        private readonly ILogger<ExtensionRunnerController> _logger;
        private readonly ExtensionDbContext _extensionDbContext;
        private readonly HttpClient _httpClient;

        public ExtensionRunnerController(ILogger<ExtensionRunnerController> logger, ExtensionDbContext extensionDbContext,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _extensionDbContext = extensionDbContext;
            _httpClient = httpClientFactory.CreateClient();
        }

        /// <param name="extensionId" example="set_require_password_change">The extension</param> 
        [HttpPost("{extensionId}")]
        [SwaggerRequestExample(typeof(ExtensionPayload), typeof(ExtensionPayloadExample))] 
        public async Task<dynamic> Execute([FromRoute] string extensionId, [FromBody] ExtensionPayload payload)
        {
            var extensionModel = await _extensionDbContext.FindJsonAsync<ExtensionModel>(extensionId);
            if (extensionModel == null)
                return NotFound();

            await extensionModel.Delay(HttpContext);

            await Task.WhenAll(extensionModel.HttpCalls
                .Select(e => e.GetRequest(payload))
                .Select(_httpClient.SendAsync)
                .Select(LogResponse));
                   

            return Ok(extensionModel.Result);
        }

        private async Task LogResponse(Task<HttpResponseMessage> response) => Log(await Json(await response));
 
        async Task<JsonElement> Json(HttpResponseMessage response)
        {
            var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            return doc.RootElement;
        }


        void Log(JsonElement json) => _logger.LogInformation($"response error code {ErrorCode(json)}");

        int ErrorCode(JsonElement json) => json.GetProperty("errorCode").GetInt32();
    }
}