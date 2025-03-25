# externalator
Automatic Stripe-style ID generation for EF database contexts.

## Getting Started
This repo is for example purposes (maybe I'll create a NuGet package at some point).

If you copy this in as-is, you can use it in a pretty straightforward fashion.

First, in one of your model classes, add the two attributes as described in the following example:

```csharp
using Externalator.Attributes;

[ExternalIdConfig(Prefix = "best")] // Add this to configure the external ID generation for this class
public class BestClass
{
    public long Id { get; set; }

    [ExternalId] // Add this to instruct Externalator to know which field(s) for which to generate an external ID
    public string ExternalId { get; set; }
}
```

Then, in your `DbContext`, use the included extension method, like so:

```csharp
public class BestDbContext : DbContext
{
    public DbSet<BestClass> BestThings { get; set; }

    // Override this method on your DbContext
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        AddExternalIdConventions(modelBuilder); // Then use this extension method
    }
}
```

And there you go!

The gist is that the attributes are read via the extension method, which adds `ExternalIdValueGenerator` to the field specified by the attachment of the `[ExternalId]` attribute.