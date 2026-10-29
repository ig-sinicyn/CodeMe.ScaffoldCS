using CodeMe.ScaffoldCS.Metadata;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CodeMe.ScaffoldCS.Internals;

internal sealed partial class ModelLoaderBackgroundService(
    IEnumerable<IModelSource> sources,
    ILogger<ModelLoaderBackgroundService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var sourcesToLoad = sources.ToList();
        if (sourcesToLoad.Count == 0)
        {
            LogNoModelsToLoad();
            return;
        }

        LogModelLoadStart(sourcesToLoad.Count);

        foreach (var modelSource in sourcesToLoad)
        {
            try
            {
                await modelSource.LoadAsync(cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogModelLoadFailed(ex, modelSource.GetType().Name, ex.Message);
                throw;
            }
        }

        LogModelLoadComplete(sourcesToLoad.Count);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    [LoggerMessage(
        LogLevel.Information,
        "No model sources registered")]
    private partial void LogNoModelsToLoad();

    [LoggerMessage(
        LogLevel.Information,
        "Begin load {SourceCount} model source(s)")]
    private partial void LogModelLoadStart(int sourceCount);

    [LoggerMessage(
        LogLevel.Error,
        "Cannot load {SourceName} model source: {Message}")]
    private partial void LogModelLoadFailed(Exception ex, string sourceName, string message);

    [LoggerMessage(
        LogLevel.Information,
        "Successfully loaded all {SourceCount} model source(s)")]
    private partial void LogModelLoadComplete(int sourceCount);
}