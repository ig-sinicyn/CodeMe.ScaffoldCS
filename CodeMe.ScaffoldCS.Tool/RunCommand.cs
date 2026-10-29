using System.Reflection;

namespace CodeMe.ScaffoldCS.Tool;

internal static class RunCommand
{
    public static async Task<int> RunAsync(
        string assemblyPath,
        string? templateName,
        string[] args,
        CancellationToken cancellation = default)
    {
        assemblyPath = Path.GetFullPath(assemblyPath);
        var context = new TemplateAssemblyLoadContext(assemblyPath);
        try
        {
            var assembly = context.LoadFromAssemblyPath(assemblyPath);
            var coreAssembly = context.LoadFromAssemblyName(
                new AssemblyName(ScaffoldCSReflectionHelper.CoreAssemblyName));

            var startupType = assembly.GetTemplateStartupType(templateName);
            var runner = coreAssembly.GetTemplateRunner();

            return await runner(args, startupType, cancellation);
        }
        finally
        {
            context.Unload();
        }
    }
}