namespace CodeMe.ScaffoldCS.Internals;

internal interface IScaffoldService
{
    ValueTask RenderAsync(CancellationToken cancellation = default);
}