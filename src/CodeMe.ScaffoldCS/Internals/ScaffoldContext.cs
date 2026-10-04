using System.Collections.Concurrent;
using CodeMe.ScaffoldCS.Metadata.CSharp;
using Microsoft.Extensions.Options;

namespace CodeMe.ScaffoldCS.Internals;

public class ScaffoldContext(
    IScaffoldFileAccessor fileAccessor,
    IOptionsMonitor<ScaffoldOptions> options) : IScaffoldContext
{
    private readonly ConcurrentDictionary<string, IScaffoldFileContent> _files = new(PathHelper.PathComparer);

    public ScaffoldRenderOptions RenderOptions => options.CurrentValue.Render;

    public IReadOnlyCollection<IScaffoldFileContent> GetAllFiles() => _files.Values.ToArray();

    public async ValueTask<IScaffoldFileContent> GetOrCreateFileAsync(string path, CancellationToken cancellation)
    {
        var fullPath = fileAccessor.ResolveFullPath(path);

        var allowOverwrite = options.CurrentValue.AllowOverwrite;
        if (_files.TryGetValue(fullPath, out var existing))
        {
            if (!allowOverwrite)
            {
                throw new InvalidOperationException(
                    $"There is a new scaffolded file at {path}."
                    + $" Enable {nameof(ScaffoldOptions)}.{nameof(ScaffoldOptions.AllowOverwrite)} option"
                    + " to allow file overwrite");
            }

            return existing;
        }

        var newFile = new ScaffoldFileContent
        {
            Path = fullPath
        };

        if (File.Exists(fullPath))
        {
            if (!allowOverwrite)
            {
                throw new InvalidOperationException(
                    $"There is an existing file at {path}."
                    + $" Enable {nameof(ScaffoldOptions)}.{nameof(ScaffoldOptions.AllowOverwrite)} option"
                    + " to allow file overwrite");
            }

            await using var stream = File.OpenRead(fullPath);
            using var reader = new StreamReader(stream);
            newFile.Content = await reader.ReadToEndAsync(cancellation);
            newFile.EncodingName = reader.CurrentEncoding.WebName;
        }

        return _files.AddOrUpdate(
            fullPath,
            newFile,
            (_, x) =>
            {
                if (!allowOverwrite)
                {
                    throw new InvalidOperationException(
                        $"There is a new scaffolded file at {path}."
                        + $" Enable {nameof(ScaffoldOptions)}.{nameof(ScaffoldOptions.AllowOverwrite)} option"
                        + " to allow file overwrite");
                }

                return x;
            });
    }

    public void RemoveFile(string path) =>
        _files.TryRemove(fileAccessor.ResolveFullPath(path), out _);
}