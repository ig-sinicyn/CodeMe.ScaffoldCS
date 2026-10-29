using System.Reflection;
using CodeMe.ScaffoldCS.Metadata.CSharp;
using CodeMe.ScaffoldCS.Metadata.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CodeMe.ScaffoldCS.Metadata.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static ModelSourcesBuilder AddModelSources(this IServiceCollection services)
    {
        services.AddModelFileAccessor();

        return new ModelSourcesBuilder(services);
    }

    private static IServiceCollection AddModelFileAccessor(this IServiceCollection services)
    {
        services.AddOptions<ModelFileAccessorOptions>();
        services.TryAddSingleton<IModelFileAccessor, DefaultModelFileAccessor>();

        return services;
    }

    public static ModelSourcesBuilder SetBasePath(this ModelSourcesBuilder modelBuilder, string? path)
    {
        modelBuilder.Services.AddOptions<ModelFileAccessorOptions>().Configure(opt => opt.BasePath = path);
        return modelBuilder;
    }

    public static ModelSourcesBuilder AddCSharpSource(this ModelSourcesBuilder modelBuilder, string filename)
    {
        ArgumentException.ThrowIfNullOrEmpty(filename);

        modelBuilder.Services.AddCSharpModelProvider(opt => opt.SourceFileNames.Add(filename));
        return modelBuilder;
    }

    public static ModelSourcesBuilder AddCSharpSource(
        this ModelSourcesBuilder modelBuilder,
        string filename,
        string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(filename);
        ArgumentException.ThrowIfNullOrEmpty(content);

        modelBuilder.Services.AddCSharpModelProvider(opt => opt.SourceFiles.Add(filename, content));
        return modelBuilder;
    }

    public static ModelSourcesBuilder AddCSharpSources(
        this ModelSourcesBuilder modelBuilder,
        IReadOnlyDictionary<string, string> sourceFiles)
    {
        ArgumentNullException.ThrowIfNull(sourceFiles);

        modelBuilder.Services.AddCSharpModelProvider(
            opt =>
            {
                foreach (var (fileName, content) in sourceFiles)
                {
                    opt.SourceFiles.Add(fileName, content);
                }
            });
        return modelBuilder;
    }

    public static ModelSourcesBuilder AddCSharpReference(this ModelSourcesBuilder modelBuilder, Type assemblyType) =>
        modelBuilder.AddCSharpReference(assemblyType.Assembly.Location);

    public static ModelSourcesBuilder AddCSharpReference(this ModelSourcesBuilder modelBuilder, Assembly assembly) =>
        modelBuilder.AddCSharpReference(assembly.Location);

    public static ModelSourcesBuilder AddCSharpReference(
        this ModelSourcesBuilder modelBuilder,
        string assemblyPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(assemblyPath);

        modelBuilder.Services.ConfigureCSharpModelProvider(opt => opt.SourceReferences.Add(assemblyPath));
        return modelBuilder;
    }

    private static IServiceCollection AddCSharpModelProvider(
        this IServiceCollection services,
        Action<CSharpModelProviderOptions>? configure = null)
    {
        var optionsBuilder = services.AddOptions<CSharpModelProviderOptions>();

        var oldCount = services.Count;
        services.TryAddSingleton<ICSharpModelProvider, CSharpModelProvider>();
        if (services.Count > oldCount)
        {
            services.AddSingleton<IModelSource>(
                provider => (IModelSource)provider.GetRequiredService<ICSharpModelProvider>());
        }

        if (configure != null)
        {
            optionsBuilder.Configure(configure);
        }

        return services;
    }

    private static IServiceCollection ConfigureCSharpModelProvider(
        this IServiceCollection services,
        Action<CSharpModelProviderOptions> configure)
    {
        services.AddOptions<CSharpModelProviderOptions>().Configure(configure);

        return services;
    }
}