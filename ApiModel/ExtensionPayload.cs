using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
 using JsonReader=Newtonsoft.Json.JsonReader; 
using BlushingPenguin.JsonPath;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

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
                return _jsonDocument.SelectToken(value.TrimStart('$'))?.GetString();
                // return _tokens.FirstOrDefault(x => x.Path == value.TrimStart('$')).Value;
            }

            return value;
        }
 
        public JsonDocument? GetDoc()
        {
            return _jsonDocument;
        }
    
    }

    public class ExtensionPayloadJsonConverter : JsonConverter<ExtensionPayload>
    {
        public override ExtensionPayload Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonDocument.TryParseValue(ref reader, out var document);
            return new ExtensionPayload(document);
        }

        public override void Write(Utf8JsonWriter writer, ExtensionPayload value, JsonSerializerOptions options)
        {
            value.GetDoc()?.WriteTo(writer);
        }
    }
}