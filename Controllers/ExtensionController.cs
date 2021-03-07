using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GenericExtesion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtensionController : ControllerBase
    {
        private readonly ILogger<ExtensionController> _logger;

        public ExtensionController(ILogger<ExtensionController> logger)
        {
            _logger = logger;
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /ExtensionModel
        ///     {
        ///        "Result": { "Status": "FAIL",  "Data": { "userFacingErrorMessage" : "You Suck!" } }
        ///       
        ///     }
        ///
        /// </remarks>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Extension>> Put(ExtensionModel extensionModel)
        {
            var extensionId = await UnzipAsync(extensionModel);
            return Created($"{Request.GetDisplayUrl()}/{extensionId}", new Extension()
            {
                ExtensionId = extensionId
            });
        }


        [HttpGet("{extension}")]
        public async Task<dynamic> Get([FromRoute]string extension)
        {
            var extensionModel = await ZipAsync<ExtensionModel>(extension);
            await Task.Delay(extensionModel.DelayMs);
            
            return extensionModel.Result;
        }


        async Task<T> ZipAsync<T>(string inputStr)
        {
            var inputBytes = Convert.FromBase64String(Uri.UnescapeDataString(inputStr));
            await using var inputStream = new MemoryStream(inputBytes);
            await using var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            using var streamReader = new StreamReader(gZipStream);
            //var decompressed = await streamReader.ReadToEndAsync();
            return await JsonSerializer.DeserializeAsync<T>(streamReader.BaseStream);
        }

        async Task<string> UnzipAsync(object value)
        {
            var inputBytes = JsonSerializer.SerializeToUtf8Bytes(value);

            await using var msi = new MemoryStream(inputBytes);
            await using var mso = new MemoryStream();
            await using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                await msi.CopyToAsync(gs);
            }

            return Uri.EscapeDataString(Convert.ToBase64String(mso.ToArray()));
        }

        
    }
}