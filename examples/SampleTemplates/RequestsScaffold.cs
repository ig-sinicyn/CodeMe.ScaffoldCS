using CodeMe.ScaffoldCS;
using CodeMe.ScaffoldCS.DependencyInjection;
using CodeMe.ScaffoldCS.Metadata.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SampleTemplates;

public class RequestsScaffold : IScaffoldStartup
{
    public void Configure(IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.Get<ScaffoldOptions>();
        if (options?.ModelPath == null)
        {
            throw new InvalidOperationException($"Please set {nameof(options.ModelPath)} value");
        }

        services.AddModelSources()
            .AddCSharpSource(options.ModelPath);

        services.AddScaffold()
            .AddScaffoldPart<CreateRequestFile>();
    }
}