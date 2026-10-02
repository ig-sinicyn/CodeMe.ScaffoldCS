using Microsoft.Extensions.Options;

namespace CodeMe.ScaffoldCS.Internals;

public class DefaultScaffoldFileAccessor : IScaffoldFileAccessor, IDisposable
{
    private readonly IDisposable? _optionsSubscription;

    private string? _basePath;

    public DefaultScaffoldFileAccessor(IOptionsMonitor<ScaffoldOptions> options)
    {
        _basePath = options.CurrentValue.OutputPath == null
            ? null
            : Path.GetFullPath(options.CurrentValue.OutputPath);
        _optionsSubscription = options.OnChange(
            opt =>
            {
                _basePath = opt.OutputPath == null
                    ? null
                    : Path.GetFullPath(opt.OutputPath);
            });
    }

    public void Dispose() => _optionsSubscription?.Dispose();

    public string ResolveFullPath(string path) => _basePath == null
        ? Path.GetFullPath(path)
        : Path.Combine(_basePath, path);
}