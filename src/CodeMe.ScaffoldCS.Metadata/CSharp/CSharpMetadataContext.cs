using CodeMe.ScaffoldCS.Metadata.CodeModel;
using CodeMe.ScaffoldCS.Metadata.Internals;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

public class CSharpMetadataContext : ModelSourceBase<CSharpCompilation>
{
    private readonly IFileAccessor _fileAccessor;

    private readonly IOptionsMonitor<CSharpMetadataContextOptions> _options;

    public CSharpMetadataContext(
        IFileAccessor fileAccessor,
        IOptionsMonitor<CSharpMetadataContextOptions> options)
    {
        _fileAccessor = fileAccessor;
        _options = options;
    }

    protected override async ValueTask<CSharpCompilation> InitialiseAsync(CancellationToken cancellation = default)
    {
        var options = _options.CurrentValue;
        var parseOptions = CSharpParseOptions.Default
            .WithLanguageVersion(options.LanguageVersion)
            .WithDocumentationMode(DocumentationMode.Parse);

        var syntaxTrees = new List<SyntaxTree>();
        foreach (var fileName in options.SourceFileNames.Distinct())
        {
            var path = _fileAccessor.ResolveFullPath(fileName);
            var syntaxTree = CSharpSyntaxTree.ParseText(
                await File.ReadAllTextAsync(path, cancellation),
                parseOptions,
                path);
            syntaxTrees.Add(syntaxTree);
        }

        var mscorlibPath = typeof(object).Assembly.Location;
        var references = options.SourceReferences
            .Select(x => _fileAccessor.ResolveFullPath(x))
            .Prepend(mscorlibPath)
            .Distinct()
            .Select(x => MetadataReference.CreateFromFile(x))
            .ToArray();

        var compilation = CSharpCompilation.Create(
            $"CodeMe.ScaffoldCS.Metadata.{Guid.NewGuid()}",
            syntaxTrees: syntaxTrees,
            references: references);

        return compilation;
    }

    public static readonly CSharpParseOptions LatestLanguageParseOptions =
        CSharpParseOptions.Default
            .WithLanguageVersion(LanguageVersion.Preview)
            .WithDocumentationMode(DocumentationMode.Parse);

    public DtoModel GetDto(string fileName, string typeName)
    {
        var path = _fileAccessor.ResolveFullPath(fileName);

        var file = State.SyntaxTrees.First(x => x.FilePath.Equals(fileName));
        var type = CSharpMetadataParser.ResolveTargetType((CompilationUnitSyntax)file.GetRoot(), typeName);

        return CSharpMetadataParser.ParseDto(type, State);
    }
}