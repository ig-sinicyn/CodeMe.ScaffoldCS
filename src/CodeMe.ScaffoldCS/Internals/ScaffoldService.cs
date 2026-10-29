using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeMe.ScaffoldCS.Internals;

internal sealed partial class ScaffoldService(
    IScaffoldFileAccessor fileAccessor,
    IEnumerable<IScaffoldPart> parts,
    IOptionsMonitor<ScaffoldOptions> options,
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

        var context = new ScaffoldContext(fileAccessor, options);
        foreach (var scaffoldPart in partsToRender)
        {
            try
            {
                await scaffoldPart.RenderAsync(context, cancellation);
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

        await SaveContextAsync(context, cancellation);
    }

    private async Task SaveContextAsync(ScaffoldContext context, CancellationToken cancellation)
    {
        var encodingName = options.CurrentValue.EncodingName;
        var defaultEncoding = encodingName == null
            ? Encoding.UTF8
            : Encoding.GetEncoding(encodingName);
        foreach (var fileContent in context.GetAllFiles())
        {
            if (fileContent.Content == null)
            {
                continue;
            }

            var encoding = fileContent.EncodingName == null
                ? defaultEncoding
                : Encoding.GetEncoding(fileContent.EncodingName);

            await File.WriteAllTextAsync(
                fileContent.Path,
                fileContent.Content,
                encoding,
                cancellation);
            LogFileSaved(fileContent.Path);
        }

        LogFilesSaved();
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

    [LoggerMessage(
        LogLevel.Information,
        "Scaffold file saved to {Path}")]
    private partial void LogFileSaved(string path);

    [LoggerMessage(
        LogLevel.Information,
        "All scaffold files saved")]
    private partial void LogFilesSaved();
}