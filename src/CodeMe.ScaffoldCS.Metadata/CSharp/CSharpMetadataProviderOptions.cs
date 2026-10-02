using Microsoft.CodeAnalysis.CSharp;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

public class CSharpMetadataProviderOptions
{
    private static readonly StringComparer _pathComparer = StringComparer.OrdinalIgnoreCase;

    public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Preview;

    public HashSet<string> SourceFileNames { get; } = new(_pathComparer);

    public Dictionary<string, string> SourceFiles { get; } = new(_pathComparer);

    public HashSet<string> SourceReferences { get; } = new(_pathComparer);
}