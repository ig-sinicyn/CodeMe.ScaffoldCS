namespace CodeMe.ScaffoldCS.Metadata;

public interface IModelSource<out T> : IModelSource
{
    public T Model { get; }
}