using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FGO.Database.Generator
{
    public class JsonDbEntityConverter<TEntity> : JsonConverter<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> dbSet;

        public JsonDbEntityConverter(DbSet<TEntity> dbSet)
        {
            this.dbSet = dbSet;
        }

        public override TEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return dbSet.Find(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, TEntity value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}