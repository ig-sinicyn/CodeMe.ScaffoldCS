using Microsoft.Extensions.Options;

namespace CodeMe.ScaffoldCS.Metadata.Internals;

public class DefaultModelFileAccessor : IModelFileAccessor, IDisposable
{
    private readonly IDisposable? _optionsSubscription;

    private string? _basePath;

    public DefaultModelFileAccessor(IOptionsMonitor<ModelFileAccessorOptions> options)
    {
        _basePath = options.CurrentValue.BasePath == null
            ? null
            : Path.GetFullPath(options.CurrentValue.BasePath);
        _optionsSubscription = options.OnChange(
            opt =>
            {
                _basePath = options.CurrentValue.BasePath == null
                    ? null
                    : Path.GetFullPath(options.CurrentValue.BasePath);
            });
    }

    public void Dispose() => _optionsSubscription?.Dispose();

    public string ResolveFullPath(string path) => _basePath == null
        ? Path.GetFullPath(path)
        : Path.Combine(_basePath, path);
}