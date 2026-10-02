using CodeMe.ScaffoldCS.Infrastructure.Options;
using CodeMe.ScaffoldCS.Internals;
using CodeMe.ScaffoldCS.Metadata.DependencyInjection;
using CodeMe.ScaffoldCS.Metadata.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CodeMe.ScaffoldCS.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScaffoldServices(
        this IServiceCollection services,
        Action<ScaffoldOptions>? configure = null)
    {
        services.AddHostedService<ModelLoaderBackgroundService>();
        var optionsBuilder = services.AddOptions<ScaffoldOptions>();
        if (configure != null)
        {
            optionsBuilder.Configure(configure);
        }

        services.TryAddSingleton<IScaffoldFileAccessor, DefaultScaffoldFileAccessor>();
        services.TryAddSingleton<IScaffoldService, ScaffoldService>();

        services.AddModelSources();
        services.AddOptions<ModelFileAccessorOptions>().Bind(
            (ModelFileAccessorOptions opt, ScaffoldOptions dep) => opt.BasePath = dep.ModelBasePath);

        return services;
    }

    public static IServiceCollection ConfigureScaffoldServices(
        this IServiceCollection services,
        Action<ScaffoldOptions> configure)
    {
        services.AddOptions<ScaffoldOptions>().Configure(configure);

        return services;
    }
}