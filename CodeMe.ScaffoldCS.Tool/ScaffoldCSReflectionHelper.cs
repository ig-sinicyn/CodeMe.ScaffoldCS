using System.Reflection;

namespace CodeMe.ScaffoldCS.Tool;

internal static class ScaffoldCSReflectionHelper
{
    public const string CoreAssemblyName = "CodeMe.ScaffoldCS";

    private const string StartupInterfaceFullName = "CodeMe.ScaffoldCS.IScaffoldStartup";
    private const string ScaffoldHostTypeFullName = "CodeMe.ScaffoldCS.Hosting.ScaffoldHost";

    // TODO: disposable reflection-only scope for assembly loading with custom assembly load context
    // TODO: consider using [UnsafeAccessor]
    public static (Assembly targetAssembly, Assembly coreAssembly) LoadAssemblies(string assemblyPath)
    {
        // Load target assembly and enable probing for its dependent assemblies;
        var targetAssembly = Assembly.LoadFrom(assemblyPath);
        var coreAssembly = Assembly.Load(CoreAssemblyName);

        return (targetAssembly, coreAssembly);
    }

    public static Type GetTemplateStartupType(this Assembly assembly, string? templateName) =>
        assembly.GetTypes().FirstOrDefault(x => x.IsStartupType(templateName))
        ?? throw new TemplateLoadException($"Cannot find template {templateName} in {assembly}");

    public static bool IsStartupType(this Type candidate, string? templateName) =>
        (templateName == null
            || candidate.Name.Equals(templateName, StringComparison.OrdinalIgnoreCase)
            || candidate.FullName!.Equals(templateName, StringComparison.OrdinalIgnoreCase)
            || candidate.Name.TrimSuffixes("Startup").Equals(templateName, StringComparison.OrdinalIgnoreCase))
        && candidate.IsStartupType();

    private static bool IsStartupType(this Type candidate) =>
        candidate is { IsPublic: true, IsAbstract: false } && candidate.GetInterface(StartupInterfaceFullName) != null;

    private static ReadOnlySpan<char> TrimSuffixes(this string name, params ReadOnlySpan<string> suffixes)
    {
        foreach (var suffix in suffixes)
        {
            if (name.EndsWith(suffix, StringComparison.Ordinal))
            {
                return name.AsSpan()[..^suffix.Length];
            }
        }

        return name;
    }

    public static Func<string[], Type, CancellationToken, Task<int>> GetTemplateRunner(this Assembly coreAssembly)
    {
        var hostType = coreAssembly.GetType(ScaffoldHostTypeFullName)
            ?? throw new TemplateLoadException($"Cannot resolve {ScaffoldHostTypeFullName} in {coreAssembly}");

        var runMethod = hostType.GetMethod(
                "RunAsync",
                BindingFlags.Public | BindingFlags.Static, [typeof(string[]), typeof(Type), typeof(CancellationToken)])
            ?? throw new TemplateLoadException($"Cannot resolve run method in {hostType}");

        return (args, template, cancellation) => (Task<int>)runMethod.Invoke(null, [args, template, cancellation])!;
    }
}