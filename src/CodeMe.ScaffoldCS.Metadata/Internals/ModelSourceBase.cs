namespace CodeMe.ScaffoldCS.Metadata.Internals;

public abstract class ModelSourceBase<TState> : IModelSource
    where TState : class
{
    private readonly IModelSource[] _dependencies;

    private TState? _state;

    protected ModelSourceBase(params IModelSource[] dependencies)
    {
        _dependencies = dependencies;
    }

    protected TState State => _state
        ?? throw new InvalidOperationException(
            $"{GetType().Name} is not initialized. Please call {nameof(LoadAsync)} first");

    public async ValueTask LoadAsync(CancellationToken cancellation = default)
    {
        if (_state != null)
        {
            return;
        }

        foreach (var dependency in _dependencies)
        {
            await dependency.LoadAsync(cancellation);
        }

        _state = await InitialiseAsync(cancellation);
    }

    protected abstract ValueTask<TState> InitialiseAsync(CancellationToken cancellation = default);
}