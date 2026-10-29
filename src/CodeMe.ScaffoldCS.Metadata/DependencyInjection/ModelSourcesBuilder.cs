using Microsoft.Extensions.DependencyInjection;

namespace CodeMe.ScaffoldCS.Metadata.DependencyInjection;

public class ModelSourcesBuilder(IServiceCollection services)
{
    public IServiceCollection Services => services;

    public IServiceCollection Build() => services;
}