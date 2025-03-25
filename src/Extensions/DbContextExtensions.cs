using System.Reflection;
using Externalator.Attributes;
using Externalator.ValueGeneration;
using Microsoft.EntityFrameworkCore;

public static class DbContextExtensions
{
    public static void AddExternalIdConventions(this DbContext ctx, ModelBuilder modelBuilder)
    {
        Dictionary<string, string> _externalIdTypes = new();

        foreach (var prop in ctx.GetType().GetTypeInfo().GetProperties().Where(prop =>
            prop.PropertyType.IsConstructedGenericType &&
            prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)))
        {
            var typeParam = prop.PropertyType.GetGenericArguments().First();

            var idAttr = prop.GetCustomAttribute<ExternalIdAttribute>();
            if (idAttr == null) return;

            var ti = typeParam.GetTypeInfo();

            var idConfigAttr = ti.GetCustomAttribute<ExternalIdConfigAttribute>()
              ?? throw new Exception($"ExternalIdAttribute requires an ExternalIdConfigAttribute to be set on {ti.Name}.");

            if (!_externalIdTypes.ContainsKey(idConfigAttr.Prefix))
            {
                _externalIdTypes.Add(idConfigAttr.Prefix, ti.Name);
            }
            else if (_externalIdTypes[idConfigAttr.Prefix] != ti.Name)
            {
                throw new InvalidOperationException($"{idConfigAttr.Prefix} is defined on multiple classes: {_externalIdTypes[idConfigAttr.Prefix]}, {ti.Name}.");
            }

            // Add automatic value generator for external IDs
            modelBuilder
              .Entity(ti)
              .Property(prop.Name)
              .HasValueGenerator<ExternalIdValueGenerator>();

            // Add an index for external IDs
            modelBuilder
              .Entity(ti)
              .HasIndex(prop.Name)
              .IsUnique();
        }
    }
}