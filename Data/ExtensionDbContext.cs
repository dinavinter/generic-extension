using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace GenericExtesion
{
    public class ExtensionDbContext : DbContext
    {
        private readonly IExamplesProvider<ExtensionModel> _examplesProvider;

        public ExtensionDbContext(DbContextOptions<ExtensionDbContext> options,
            IExamplesProvider<ExtensionModel> examplesProvider) : base(options)
        {
            _examplesProvider = examplesProvider;
        }

        public DbSet<JsonEntity> Extensions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<JsonEntity>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            var example = _examplesProvider.GetExamples();
            modelBuilder.Entity<JsonEntity>()
                .HasData(new[]
                {
                    JsonEntity.FromObject(example, example.Id)
                });
        }


        public async ValueTask<string> UpsertJsonAsync<TEntity>(TEntity entity, string id = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var jsonEntity =
                await Extensions.FirstOrDefaultAsync(e => e.Id == id, cancellationToken: cancellationToken);

            if (jsonEntity != null)
            {
                jsonEntity.SetModel(entity);
                Extensions.Update(jsonEntity);
                await SaveChangesAsync(cancellationToken);
                return id;
            }

            var entityEntry = await Extensions.AddAsync(JsonEntity.FromObject(entity, id), cancellationToken);
            await SaveChangesAsync(cancellationToken);
            return entityEntry.Entity.Id;
        }

        public async ValueTask<TEntity> FindJsonAsync<TEntity>(string id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var jsonEntity =
                await Extensions.FirstOrDefaultAsync(e => e.Id == id, cancellationToken: cancellationToken);
            return jsonEntity != null ? jsonEntity.GetModel<TEntity>() : default;
        }


      
    }
}