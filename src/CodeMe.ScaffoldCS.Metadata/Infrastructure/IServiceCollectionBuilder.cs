using Microsoft.Extensions.DependencyInjection;

namespace CodeMe.ScaffoldCS.Metadata.Infrastructure;

public interface IServiceCollectionBuilder
{
    IServiceCollection Services { get; }

    IServiceCollection Build();
}