using System.Reflection;
using CodeMe.ScaffoldCS.Metadata.CSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CodeMe.ScaffoldCS.Metadata.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IModelSourcesBuilder AddModelSources(
        this IServiceCollection services,
        string? rootDictionary = null)
    {
        services.AddFileAccessor();
        return new ModelSourcesBuilder(services)
            .SetBasePath(rootDictionary);
    }

    private static IServiceCollection AddFileAccessor(this IServiceCollection services)
    {
        services.AddOptions<FileAccessorOptions>();
        services.TryAddSingleton<IFileAccessor, DefaultFileAccessor>();

        return services;
    }

    public static IModelSourcesBuilder SetBasePath(this IModelSourcesBuilder modelBuilder, string? path)
    {
        modelBuilder.Services.AddOptions<FileAccessorOptions>().Configure(opt => opt.BasePath = path);
        return modelBuilder;
    }

    public static IModelSourcesBuilder AddCSharpSource(this IModelSourcesBuilder modelBuilder, string filename)
    {
        ArgumentException.ThrowIfNullOrEmpty(filename);

        modelBuilder.Services.AddCSharpSourceContext(opt => opt.SourceFileNames.Add(filename));
        return modelBuilder;
    }

    public static IModelSourcesBuilder AddCSharpSource(
        this IModelSourcesBuilder modelBuilder,
        string filename,
        string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(filename);
        ArgumentException.ThrowIfNullOrEmpty(content);

        modelBuilder.Services.AddCSharpSourceContext(opt => opt.SourceFiles.Add(filename, content));
        return modelBuilder;
    }

    public static IModelSourcesBuilder AddCSharpSources(
        this IModelSourcesBuilder modelBuilder,
        IReadOnlyDictionary<string, string> sourceFiles)
    {
        ArgumentNullException.ThrowIfNull(sourceFiles);

        modelBuilder.Services.AddCSharpSourceContext(
            opt =>
            {
                foreach (var (fileName, content) in sourceFiles)
                {
                    opt.SourceFiles.Add(fileName, content);
                }
            });
        return modelBuilder;
    }

    public static IModelSourcesBuilder AddCSharpReference(
        this IModelSourcesBuilder modelBuilder,
        Type assemblyType) => modelBuilder.AddCSharpReference(assemblyType.Assembly.Location);

    public static IModelSourcesBuilder AddCSharpReference(
        this IModelSourcesBuilder modelBuilder,
        Assembly assembly) => modelBuilder.AddCSharpReference(assembly.Location);

    public static IModelSourcesBuilder AddCSharpReference(
        this IModelSourcesBuilder modelBuilder,
        string assemblyPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(assemblyPath);

        modelBuilder.Services.ConfigureCSharpSourceContext(opt => opt.SourceReferences.Add(assemblyPath));
        return modelBuilder;
    }

    private static IServiceCollection AddCSharpSourceContext(
        this IServiceCollection services,
        Action<CSharpMetadataContextOptions>? configure = null)
    {
        var optionsBuilder = services.AddOptions<CSharpMetadataContextOptions>();

        var oldCount = services.Count;
        services.TryAddSingleton<ICSharpMetadataContext, CSharpMetadataContext>();
        if (services.Count > oldCount)
        {
            services.AddSingleton<IModelSource>(
                provider => (IModelSource)provider.GetRequiredService<ICSharpMetadataContext>());
        }

        if (configure != null)
        {
            optionsBuilder.Configure(configure);
        }

        return services;
    }

    private static IServiceCollection ConfigureCSharpSourceContext(
        this IServiceCollection services,
        Action<CSharpMetadataContextOptions> configure)
    {
        services.AddOptions<CSharpMetadataContextOptions>().Configure(configure);

        return services;
    }
}