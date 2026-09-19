namespace CodeMe.ScaffoldCS.Metadata;

public interface IFileAccessor
{
    public bool FileExists(string path);

    public TextReader OpenReadFileRead(string path);

    public TextWriter CreateFile(string path);
}