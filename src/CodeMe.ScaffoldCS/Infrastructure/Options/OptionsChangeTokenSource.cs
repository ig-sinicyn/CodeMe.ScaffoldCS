using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace CodeMe.ScaffoldCS.Infrastructure.Options;

public sealed class OptionsChangeTokenSource<TOptions, TSourceOptions> : IOptionsChangeTokenSource<TOptions>,
    IDisposable
    where TOptions : class
    where TSourceOptions : class
{
    private CancellationTokenSource? _changeToken;
    private readonly IDisposable? _changeSubscription;

    public OptionsChangeTokenSource(
        string? name,
        IOptionsMonitor<TSourceOptions> sourceMonitor,
        string? sourceName = null)
    {
        Name = name;

        _changeSubscription = sourceMonitor.OnChange(
            (_, changedName) =>
            {
                if (!string.Equals(changedName, sourceName, StringComparison.Ordinal))
                {
                    return;
                }

                var changeToken = Interlocked.Exchange(ref _changeToken, null);
                try
                {
                    changeToken?.Cancel();
                }
                finally
                {
                    changeToken?.Dispose();
                }
            });
    }

    public void Dispose()
    {
        _changeToken?.Dispose();
        _changeSubscription?.Dispose();
    }

    public string? Name { get; }

    public IChangeToken GetChangeToken()
    {
        var changeToken = LazyInitializer.EnsureInitialized(ref _changeToken, () => new CancellationTokenSource());
        return new CancellationChangeToken(changeToken.Token);
    }
}