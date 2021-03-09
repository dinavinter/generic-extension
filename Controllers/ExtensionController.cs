using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Filters;

namespace GenericExtesion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtensionController : ControllerBase
    {
        private readonly ILogger<ExtensionController> _logger;
        private readonly ExtensionDbContext _extensionDbContext;
        private readonly HttpClient _httpClient;
 
        public ExtensionController(ILogger<ExtensionController> logger, ExtensionDbContext extensionDbContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _extensionDbContext = extensionDbContext;
            _httpClient = httpClientFactory.CreateClient(); 
        }


    
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(ExtensionModel), typeof(ExtensionModelExample))]
        public async Task<ActionResult<Extension>> Put(ExtensionModel extensionModel)
       {
           var extensionId = await _extensionDbContext.UpsertJsonAsync(extensionModel, extensionModel.Id);
            return Created($"{Request.GetDisplayUrl()}/{extensionId}", new Extension()
            {
                ExtensionId = extensionId
            });
        }


        /// <param name="extension" example="set_require_password_change">The extension</param> 
        [HttpPost("{extension}")]
        public async Task<dynamic> Execute([FromRoute] string extension, [FromBody] ExtensionPayload payload)
        {
            var extensionModel = await _extensionDbContext.FindJsonAsync<ExtensionModel>(extension);
            if (extensionModel == null)
                return NotFound();

            await Task.Delay(extensionModel.DelayMs);

          await  Task.WhenAll(extensionModel.HttpCalls
                .Select(
                    e => new HttpRequestMessage()
                    {
                        RequestUri = new UriBuilder(e.BaseUrl)
                        {
                            Query = string.Join('&', e.RequestParameters.Select(e =>
                                $"{e.Key}={payload.GetValue(e.Value?.ToString())}"))
                        }.Uri,



                    }).Select(message => _httpClient.SendAsync(message)));
                ;
        


        Response.Headers["X-DelayFor"] = TimeSpan.FromMilliseconds(extensionModel.DelayMs).ToString();
            // return Ok(  JObject.FromObject(payload).Descendants().Select(e=>$"{e.Path}-{((JProperty)e).Value}").ToArray());
            // return Ok(IterateObject(payload).Select(e=>$"{e.path}-{e.value}").ToArray());
            return Ok(extensionModel.Result ?? new {Status = "Ok"});
       }

       
       
 
    }
}