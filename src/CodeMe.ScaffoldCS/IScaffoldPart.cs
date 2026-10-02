namespace CodeMe.ScaffoldCS;

public interface IScaffoldPart
{
    ValueTask RenderAsync(IScaffoldContext context, CancellationToken cancellation = default);
}