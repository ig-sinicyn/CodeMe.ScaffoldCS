using Microsoft.Extensions.DependencyInjection;

namespace CodeMe.ScaffoldCS;

public abstract class ScaffoldPartBase(IServiceProvider services) : IScaffoldPart
{
    public async ValueTask RenderAsync(IScaffoldContext context, CancellationToken cancellation = default)
    {
        foreach (var scaffoldPart in GetParts())
        {
            await scaffoldPart.RenderAsync(context, cancellation);
        }
    }

    protected IEnumerable<IScaffoldPart> GetParts()
    {
        foreach (var partType in GetPartTypes())
        {
            yield return (IScaffoldPart)ActivatorUtilities.CreateInstance(services, partType);
        }
    }

    protected abstract IEnumerable<Type> GetPartTypes();
}