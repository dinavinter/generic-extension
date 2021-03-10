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
    [Route("extension")]
    public class ExtensionController : ControllerBase
    {
        private readonly ILogger<ExtensionController> _logger;
        private readonly ExtensionDbContext _extensionDbContext;
 
        public ExtensionController(ILogger<ExtensionController> logger, ExtensionDbContext extensionDbContext,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _extensionDbContext = extensionDbContext;
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

        /// <param name="extensionId" example="set_require_password_change">The extension</param> 
        [HttpPost("{extensionId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(ExtensionModel), typeof(ExtensionModelExample))]
        public async Task<ActionResult<Extension>> Post(string extensionId, ExtensionModel extensionModel)
        {
            await _extensionDbContext.UpsertJsonAsync(extensionModel, extensionId);
            return Created(Request.GetDisplayUrl(), extensionModel);
        }

        /// <param name="extensionId" example="delay_3000">The extension</param> 
        [HttpGet("{extensionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(statusCode:0, typeof(ExtensionModelExample))]
        public async Task<ActionResult<ExtensionModel>> Get(string extensionId)
        {
            var extension= await _extensionDbContext.FindJsonAsync<ExtensionModel>(extensionId);
            if (extension == null)
                return NotFound(extensionId);
            return extension;
        }
    }
}