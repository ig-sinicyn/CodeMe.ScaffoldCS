using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeMe.ScaffoldCS.Infrastructure.Options;

public static class OptionsBuilderExtensions
{
    public static OptionsBuilder<TOptions> Bind<TOptions, TSourceOptions>(
        this OptionsBuilder<TOptions> builder,
        Action<TOptions, TSourceOptions> map)
        where TOptions : class
        where TSourceOptions : class => BindToCore(builder, sourceName: null, map);

    public static OptionsBuilder<TOptions> Bind<TOptions, TSourceOptions>(
        this OptionsBuilder<TOptions> builder,
        string? sourceName,
        Action<TOptions, TSourceOptions> map)
        where TOptions : class
        where TSourceOptions : class => BindToCore(builder, sourceName, map);

    private static OptionsBuilder<TOptions> BindToCore<TOptions, TSourceOptions>(
        OptionsBuilder<TOptions> builder,
        string? sourceName,
        Action<TOptions, TSourceOptions> map)
        where TOptions : class
        where TSourceOptions : class
    {
        builder.Services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(
            sp =>
            {
                var sourceMonitor = sp.GetRequiredService<IOptionsMonitor<TSourceOptions>>();
                return new OptionsChangeTokenSource<TOptions, TSourceOptions>(builder.Name, sourceMonitor, sourceName);
            });

        builder.Services.AddSingleton<IConfigureOptions<TOptions>>(
            sp =>
            {
                var sourceMonitor = sp.GetRequiredService<IOptionsMonitor<TSourceOptions>>();

                return new ConfigureNamedOptions<TOptions>(
                    builder.Name,
                    target =>
                    {
                        var source = sourceName is null
                            ? sourceMonitor.CurrentValue
                            : sourceMonitor.Get(sourceName);

                        map(target, source);
                    });
            });

        return builder;
    }
}