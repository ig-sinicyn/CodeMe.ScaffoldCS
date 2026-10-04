using CodeMe.ScaffoldCS.Infrastructure.Options;
using CodeMe.ScaffoldCS.Internals;
using CodeMe.ScaffoldCS.Metadata.DependencyInjection;
using CodeMe.ScaffoldCS.Metadata.Internals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CodeMe.ScaffoldCS.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScaffoldServices(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<ScaffoldOptions>? configure = null)
    {
        var optionsBuilder = services.AddOptions<ScaffoldOptions>();
        optionsBuilder.Bind(configuration);
        if (configure != null)
        {
            optionsBuilder.Configure(configure);
        }

        services.AddModelSources();
        services.AddOptions<ModelFileAccessorOptions>().Bind(
            (ModelFileAccessorOptions opt, ScaffoldOptions dep) => opt.BasePath = dep.ModelBasePath);

        services.TryAddSingleton<IScaffoldFileAccessor, DefaultScaffoldFileAccessor>();
        services.TryAddSingleton<IScaffoldService, ScaffoldService>();
        services.AddHostedService<ModelLoaderBackgroundService>();

        return services;
    }

    public static ScaffoldBuilder AddScaffold(this IServiceCollection services) => new(services);

    public static ScaffoldBuilder ConfigureScaffoldServices(
        this ScaffoldBuilder builder,
        Action<ScaffoldOptions> configure)
    {
        builder.Services.ConfigureScaffoldServices(configure);

        return builder;
    }

    private static IServiceCollection ConfigureScaffoldServices(
        this IServiceCollection services,
        Action<ScaffoldOptions> configure)
    {
        services.AddOptions<ScaffoldOptions>().Configure(configure);

        return services;
    }

    public static ScaffoldBuilder AddScaffoldPart<TPart>(this ScaffoldBuilder builder) =>
        builder.AddScaffoldPart(typeof(TPart));

    public static ScaffoldBuilder AddScaffoldPart(
        this ScaffoldBuilder builder,
        Type scaffoldPart)
    {
        if (!scaffoldPart.IsAssignableTo(typeof(IScaffoldPart)))
        {
            throw new ArgumentException(
                $"The {scaffoldPart} type must implement {nameof(IScaffoldPart)} interface", nameof(scaffoldPart));
        }

        builder.Services.AddSingleton(typeof(IScaffoldPart), scaffoldPart);

        return builder;
    }
}