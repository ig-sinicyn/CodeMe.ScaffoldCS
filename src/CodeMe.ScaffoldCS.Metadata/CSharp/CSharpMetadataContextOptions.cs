using Microsoft.CodeAnalysis.CSharp;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

public class CSharpMetadataContextOptions
{
    public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Preview;

    public List<string> SourceFileNames { get; set; } = new();

    public List<string> SourceReferences { get; set; } = new();
}