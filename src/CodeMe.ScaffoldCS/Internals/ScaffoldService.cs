using Microsoft.Extensions.Logging;

namespace CodeMe.ScaffoldCS.Internals;

internal sealed partial class ScaffoldService(
    IEnumerable<IScaffoldPart> parts,
    ILogger<ScaffoldService> logger) : IScaffoldService
{
    public async ValueTask RenderAsync(CancellationToken cancellation = default)
    {
        var partsToRender = parts.ToList();
        if (partsToRender.Count == 0)
        {
            LogNoParts();
            return;
        }

        LogRenderStart();
        foreach (var scaffoldPart in partsToRender)
        {
            try
            {
                await scaffoldPart.RenderAsync(cancellation);
            }
            catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogRenderFailed(ex, scaffoldPart.GetType().Name, ex.Message);
                throw;
            }
        }

        LogRenderComplete();
    }

    [LoggerMessage(
        LogLevel.Information,
        "No parts to render")]
    private partial void LogNoParts();

    [LoggerMessage(
        LogLevel.Information,
        "Begin render")]
    private partial void LogRenderStart();

    [LoggerMessage(
        LogLevel.Error,
        "Cannot render {PartName}: {Message}")]
    private partial void LogRenderFailed(Exception ex, string partName, string message);

    [LoggerMessage(
        LogLevel.Information,
        "Render complete")]
    private partial void LogRenderComplete();
}