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
    }
}