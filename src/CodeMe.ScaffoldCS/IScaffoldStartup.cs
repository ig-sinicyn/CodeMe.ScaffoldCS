using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeMe.ScaffoldCS;

public interface IScaffoldStartup
{
    void Configure(IServiceCollection services, IConfiguration configuration);
}