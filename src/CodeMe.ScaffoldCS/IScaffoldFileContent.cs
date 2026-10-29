namespace CodeMe.ScaffoldCS;

public interface IScaffoldFileContent
{
    string Path { get; }

    public string? EncodingName { get; set; }

    public string? Content { get; set; }
}