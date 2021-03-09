using System.Dynamic;
using System.IO;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;

namespace GenericExtesion
{
    public class ExtensionPayloadExample: IExamplesProvider<ExtensionPayload>
    {
        public ExtensionPayload GetExamples()
        {
            return System.Text.Json.JsonSerializer.Deserialize<ExtensionPayload>(File.ReadAllText(".seed/onLoginPayload.json"));
        }
    } 
    
}