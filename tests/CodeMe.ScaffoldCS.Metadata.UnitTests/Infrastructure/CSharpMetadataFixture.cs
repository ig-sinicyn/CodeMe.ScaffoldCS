using CodeMe.ScaffoldCS.Metadata.CSharp;
using CodeMe.ScaffoldCS.Metadata.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CodeMe.ScaffoldCS.Metadata.UnitTests.Infrastructure;

public class CSharpMetadataFixture : IAsyncLifetime
{
    private readonly IServiceProvider _services;

    public CSharpMetadataFixture()
    {
        var services = new ServiceCollection();
        services
            .AddModelSources()
            .AddCSharpSource(WellKnownTestClasses.BasicDtos);

        _services = services.BuildServiceProvider();
        MetadataContext = _services.GetRequiredService<ICSharpMetadataContext>();
    }

    public ICSharpMetadataContext MetadataContext { get; }

    public async ValueTask InitializeAsync()
    {
        var sources = _services.GetServices<IModelSource>();
        foreach (var modelSource in sources)
        {
            await modelSource.LoadAsync();
        }
    }

    public ValueTask DisposeAsync()
    {
        (_services as IDisposable)?.Dispose();
        return ValueTask.CompletedTask;
    }
}