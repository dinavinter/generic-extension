using System.Text.Json;
using BlushingPenguin.JsonPath;

namespace GenericExtesion
{
    [System.Text.Json.Serialization.JsonConverter(typeof(ExtensionPayloadJsonConverter))]
    public class ExtensionPayload
    {
        private readonly JsonDocument _jsonDocument;
  
        public ExtensionPayload()
        {
            
        }

        public ExtensionPayload(JsonDocument tokens)
        {
            _jsonDocument = tokens;
        }


        public object? GetValue(string? value)
        {
            
            if (value?.StartsWith("$") == true)
            {
                return _jsonDocument.SelectToken(value.TrimStart('$'))?.GetRawText().TrimStart('"').TrimEnd('"');
                // return _tokens.FirstOrDefault(x => x.Path == value.TrimStart('$')).Value;
            }

            return value;
        }
 
        public JsonDocument? GetDoc()
        {
            return _jsonDocument;
        }
    
    }

   
}