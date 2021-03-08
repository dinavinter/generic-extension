using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
using System.Text.Json;

namespace GenericExtesion
{
    public class JsonEntity
    {
        [Key] public string Id { get; set; }

        public TEntity GetModel<TEntity>()
        {
            return Unzip<TEntity>(Zipped);
        }

        public static JsonEntity FromObject<TEntity>(TEntity entity, string id = null)
        {
            return new JsonEntity()
            {
                Zipped = Zip(entity),
                Id = id
            };
        }

        public JsonEntity SetModel<TEntity>(TEntity entity)
        {
            Zipped = Zip(entity);
            return this;
        }


        public string Zipped { get; set; }


        static T Unzip<T>(string inputStr)
        {
            var inputBytes = Convert.FromBase64String(Uri.UnescapeDataString(inputStr));
            using var inputStream = new MemoryStream(inputBytes);
            using var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            using var streamReader = new StreamReader(gZipStream);
            return JsonSerializer.Deserialize<T>(streamReader.ReadToEnd());
        }

        static string Zip(object value)
        {
            var inputBytes = JsonSerializer.SerializeToUtf8Bytes(value);

            using var msi = new MemoryStream(inputBytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                msi.CopyTo(gs);
            }

            return Uri.EscapeDataString(Convert.ToBase64String(mso.ToArray()));
        }
    }
}