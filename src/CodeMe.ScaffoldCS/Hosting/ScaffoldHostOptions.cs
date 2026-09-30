using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeMe.ScaffoldCS.Hosting;

public record ScaffoldHostOptions(
    string[] Args,
    string? BasePath,
    Action<IServiceCollection, IConfiguration>? Configure = null,
    Action<IConfigurationBuilder>? ConfigureConfig = null);