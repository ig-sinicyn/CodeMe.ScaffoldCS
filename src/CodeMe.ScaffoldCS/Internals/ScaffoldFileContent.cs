namespace CodeMe.ScaffoldCS.Internals;

public class ScaffoldFileContent : IScaffoldFileContent
{
    public required string Path { get; init; }
    public string? EncodingName { get; set; }
    public string? Content { get; set; }
}