namespace GenericExtesion
{
    public class temp_shit
    {
        // public override async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        // {
        //     if (entity is JsonEntity)
        //         return await base.AddAsync(entity);
        //     else
        //     {
        //         var entityEntry =await base.AddAsync(entity);
        //         var id = entityEntry.Property<string>("Id");
        //         base.Add(new JsonEntity().SetModel(entity, id?.CurrentValue));
        //
        //         return entityEntry;
        //     }
        //     
        // }
        // public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        // {
        //     if (entity is JsonEntity)
        //         return base.Add(entity);
        //     else
        //     {
        //         var entityEntry = base.Add(entity);
        //         var id = entityEntry.Property<string>("Id");
        //         base.Add(new JsonEntity().SetModel(entity, id?.CurrentValue));
        //
        //         return entityEntry;
        //     }
        // }
        
        // public static ValueConverter<T, string> Converter<T>()
        // {
        //     return new ValueConverter<T, string>(
        //         c => JsonSerializer.Serialize(c, new JsonSerializerOptions()
        //         {
        //             WriteIndented = false,
        //             Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        //         }), c => JsonSerializer.Deserialize<T>(c, new JsonSerializerOptions()
        //         {
        //             Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        //         }));
        // }
        
        
        // async Task<T> ZipAsync<T>(string inputStr)
        // {
        //     var inputBytes = Convert.FromBase64String(Uri.UnescapeDataString(inputStr));
        //     await using var inputStream = new MemoryStream(inputBytes);
        //     await using var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress);
        //     using var streamReader = new StreamReader(gZipStream);
        //     //var decompressed = await streamReader.ReadToEndAsync();
        //     return await JsonSerializer.DeserializeAsync<T>(streamReader.BaseStream);
        // }
        //
        // async Task<string> UnzipAsync(object value)
        // {
        //     var inputBytes = JsonSerializer.SerializeToUtf8Bytes(value);
        //
        //     await using var msi = new MemoryStream(inputBytes);
        //     await using var mso = new MemoryStream();
        //     await using (var gs = new GZipStream(mso, CompressionMode.Compress))
        //     {
        //         await msi.CopyToAsync(gs);
        //     }
        //
        //     return Uri.EscapeDataString(Convert.ToBase64String(mso.ToArray()));
        // }
        
        
        
        //
        // private IEnumerable<(string path, object value)> IterateObject(ExpandoObject input)
        // {
        //     return Iterate(input);
        //     IEnumerable<(string path, object value)> Iterate(ExpandoObject value)
        //     {
        //         return value
        //             .Where(e => e.Value.GetType().IsValueType || e.Value is string)
        //             .Select(e => (path: e.Key, value: e.Value))
        //             .Concat(
        //                 value
        //                     .Where(e => e.Value is ExpandoObject)
        //                     .SelectMany(e => Iterate( e.Value  as ExpandoObject)
        //                         .Select(inner => (path: $"{e.Key}.{inner.path}", value: inner.value))
        //                     ));
        //         
        //           
        //     }
        //        
        // }

    }
}