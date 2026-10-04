using CodeMe.ScaffoldCS.DependencyInjection;
using CodeMe.ScaffoldCS.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeMe.ScaffoldCS.Hosting;

public static class ScaffoldHost
{
    public static async Task<int> RunAsync<TStartup>(
        string[] args,
        CancellationToken cancellation = default)
        where TStartup : IScaffoldStartup =>
        await RunAsync(typeof(TStartup), new ScaffoldHostOptions(args), cancellation);

    public static async Task<int> RunAsync<TStartup>(
        ScaffoldHostOptions options,
        CancellationToken cancellation = default)
        where TStartup : IScaffoldStartup =>
        await RunAsync(typeof(TStartup), options, cancellation);

    public static async Task<int> RunAsync(
        Type startup,
        ScaffoldHostOptions options,
        CancellationToken cancellation = default)
    {
        if (!startup.IsAssignableTo(typeof(IScaffoldStartup)))
        {
            throw new ArgumentException(
                $"The {startup} type must implement {nameof(IScaffoldStartup)} interface", nameof(startup));
        }

        var host = BuildHost(startup, options);
        return await RunHostAsync(host, cancellation);
    }

    private static IHost BuildHost(Type startup, ScaffoldHostOptions options)
    {
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings
            {
                Args = options.Args,
                ContentRootPath = options.BasePath
            });

        // Setup host
        options.ConfigureConfig?.Invoke(builder.Configuration);
        options.Configure?.Invoke(builder.Services, builder.Configuration);

        // Setup scaffolding
        builder.Services.AddScaffoldServices(builder.Configuration);

        // Startup
        var startupInstance = (IScaffoldStartup)Activator.CreateInstance(startup)!;
        startupInstance.Configure(builder.Services, builder.Configuration);

        var host = builder.Build();
        return host;
    }

    private static async Task<int> RunHostAsync(IHost host, CancellationToken cancellation)
    {
        await host.StartAsync(cancellation);

        var render = host.Services.GetRequiredService<ScaffoldService>();

        await render.RenderAsync(cancellation);

        await host.StopAsync(cancellation);

        return Environment.ExitCode;
    }
}