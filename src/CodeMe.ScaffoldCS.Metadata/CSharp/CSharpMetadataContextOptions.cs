using Microsoft.CodeAnalysis.CSharp;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

public class CSharpMetadataContextOptions
{
    private static readonly StringComparer _pathComparer = StringComparer.OrdinalIgnoreCase;

    public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Preview;

    public HashSet<string> SourceFileNames { get; set; } = new(_pathComparer);

    public Dictionary<string, string> SourceFiles { get; set; } = new(_pathComparer);

    public HashSet<string> SourceReferences { get; set; } = new(_pathComparer);
}