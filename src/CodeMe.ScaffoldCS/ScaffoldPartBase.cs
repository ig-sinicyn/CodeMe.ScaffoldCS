using CodeMe.ScaffoldCS.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace CodeMe.ScaffoldCS;

public abstract class ScaffoldPartBase(IServiceProvider services) : IScaffoldPart
{
    protected virtual bool Enabled => true;

    protected IScaffoldContext Context { get; private set; } = null!;

    public async ValueTask RenderAsync(IScaffoldContext context, CancellationToken cancellation = default)
    {
        if (!Enabled)
        {
            return;
        }

        var prev = Context;
        try
        {
            Context = context;
            foreach (var scaffoldPart in GetParts())
            {
                await scaffoldPart.RenderAsync(context, cancellation);
            }
        }
        finally
        {
            Context = prev;
        }
    }

    protected IEnumerable<IScaffoldPart> GetParts()
    {
        foreach (var partType in GetPartTypes())
        {
            if (!partType.IsAssignableTo(typeof(IScaffoldPart)))
            {
                throw new InvalidOperationException(
                    $"The {partType} type must implement {nameof(IScaffoldPart)} interface");
            }

            yield return (IScaffoldPart)ActivatorUtilities.CreateInstance(services, partType);
        }
    }

    protected abstract IEnumerable<Type> GetPartTypes();
}