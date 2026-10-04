using CodeMe.ScaffoldCS.Hosting;

namespace SampleTemplates;

public static class Program
{
    public static Task<int> Main(string[] args) =>
        ScaffoldHost.RunAsync<RequestsScaffold>(args);
}