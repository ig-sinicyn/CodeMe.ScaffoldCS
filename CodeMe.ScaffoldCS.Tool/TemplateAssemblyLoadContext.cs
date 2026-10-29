using System.Reflection;
using System.Runtime.Loader;

namespace CodeMe.ScaffoldCS.Tool;

public class TemplateAssemblyLoadContext : AssemblyLoadContext, IDisposable
{
    private readonly AssemblyDependencyResolver _resolver;

    public TemplateAssemblyLoadContext(string mainAssemblyPath) : base(isCollectible: true)
    {
        _resolver = new AssemblyDependencyResolver(mainAssemblyPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
    }

    public void Dispose() => Unload();
}