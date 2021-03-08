using System.Dynamic;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Filters;

namespace GenericExtesion
{
    public class ExtensionPayloadExample: IExamplesProvider<ExpandoObject>
    {
        public ExpandoObject GetExamples()
        {
            return JsonSerializer.Deserialize<ExpandoObject>(File.ReadAllText(".seed/onLoginPayload.json"));
        }
    }


    // public class Payload : ExpandoObjectConverter
    // {
    //     
    // }
    
}