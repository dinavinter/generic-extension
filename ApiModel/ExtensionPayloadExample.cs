using System.IO;
using System.Text.Json;
using Swashbuckle.AspNetCore.Filters;

namespace GenericExtesion
{
    public class ExtensionPayloadExample: IExamplesProvider<JsonElement>
    {
        public JsonElement GetExamples()
        {
            return JsonDocument.Parse(File.OpenRead(".seed/onLoginPayload.json")).RootElement;
        }
    }
    
    
}