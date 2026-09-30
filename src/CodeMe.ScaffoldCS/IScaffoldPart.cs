namespace CodeMe.ScaffoldCS;

public interface IScaffoldPart
{
    Task RenderAsync(CancellationToken cancellation = default);
}