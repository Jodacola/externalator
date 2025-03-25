using System.Reflection;
using Externalator.Attributes;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SimpleBase;

namespace Externalator.ValueGeneration;

public class ExternalIdValueGenerator : ValueGenerator<string>
{
    private static Dictionary<Type, ExternalIdConfigAttribute?> _configCache = new();

    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
    {
        var type = entry.Entity.GetType();

        if (!_configCache.TryGetValue(type, out var config))
        {
            if (!_configCache.TryGetValue(type, out config))
            {
                config = type.GetCustomAttribute<ExternalIdConfigAttribute>();
                _configCache[type] = config;
            }
        }

        if (config is null)
        {
            throw new Exception($"Entity {type.Name} does not have an external ID configuration");
        }

        return $"{config.Prefix}_{Guid.NewGuid().ToBase58()}";
    }
}