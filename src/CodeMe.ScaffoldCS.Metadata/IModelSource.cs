namespace CodeMe.ScaffoldCS.Metadata;

public interface IModelSource<out T>
{
    public ValueTask LoadAsync(CancellationToken cancellation = default);

    public T Source { get; }
}