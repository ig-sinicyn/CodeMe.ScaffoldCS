namespace CodeMe.ScaffoldCS;

public interface IScaffoldContext
{
    public ScaffoldRenderOptions RenderOptions { get; }

    IReadOnlyCollection<IScaffoldFileContent> GetAllFiles();

    ValueTask<IScaffoldFileContent> GetOrCreateFileAsync(string path, CancellationToken cancellation);

    void RemoveFile(string path);
}