using Microsoft.Extensions.DependencyInjection;

namespace CodeMe.ScaffoldCS.DependencyInjection;

public class ScaffoldBuilder(IServiceCollection services)
{
    public IServiceCollection Services => services;

    public IServiceCollection Build() => services;
}