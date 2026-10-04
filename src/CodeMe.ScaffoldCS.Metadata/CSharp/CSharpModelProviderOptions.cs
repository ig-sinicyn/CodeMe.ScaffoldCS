using Microsoft.CodeAnalysis.CSharp;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

public class CSharpModelProviderOptions
{
    public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Preview;

    public HashSet<string> SourceFileNames { get; } = new(PathHelper.PathComparer);

    public Dictionary<string, string> SourceFiles { get; } = new(PathHelper.PathComparer);

    public HashSet<string> SourceReferences { get; } = new(PathHelper.PathComparer);
}