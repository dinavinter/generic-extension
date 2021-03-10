using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenericExtesion
{
    public class ExtensionPayloadJsonConverter :   JsonConverter<ExtensionPayload>
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