namespace CodeMe.ScaffoldCS.Metadata;

public interface IModelSource
{
    public ValueTask LoadAsync(CancellationToken cancellation = default);
}