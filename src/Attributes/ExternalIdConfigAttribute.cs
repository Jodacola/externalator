namespace Externalator.Attributes;

public class ExternalIdConfigAttribute : Attribute
{
    public ExternalIdConfigAttribute()
    {

    }

    public ExternalIdConfigAttribute(string prefix)
    {
        Prefix = prefix;
    }

    public string Prefix { get; set; } = string.Empty;
}