using CodeMe.ScaffoldCS.Metadata.CodeModel;
using CodeMe.ScaffoldCS.Metadata.Internals;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

public class CSharpModelProvider : ModelSourceBase<CSharpCompilation>, ICSharpModelProvider
{
    private readonly IModelFileAccessor _fileAccessor;

    private readonly IOptionsMonitor<CSharpModelProviderOptions> _options;

    public CSharpModelProvider(
        IModelFileAccessor fileAccessor,
        IOptionsMonitor<CSharpModelProviderOptions> options)
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

        var allSources = new HashSet<string>(options.SourceFiles.Comparer);
        foreach (var fileName in options.SourceFileNames)
        {
            var path = _fileAccessor.ResolveFullPath(fileName);
            if (!allSources.Add(path))
            {
                throw new InvalidOperationException($"Cannot add {fileName} file as it was used already");
            }

            var syntaxTree = CSharpSyntaxTree.ParseText(
                await File.ReadAllTextAsync(path, cancellation),
                parseOptions,
                path,
                cancellationToken: cancellation);
            syntaxTrees.Add(syntaxTree);
        }

        foreach (var (fileName, content) in options.SourceFiles)
        {
            var path = _fileAccessor.ResolveFullPath(fileName);
            if (!allSources.Add(path))
            {
                throw new InvalidOperationException($"Cannot add {fileName} file as it was used already");
            }

            var syntaxTree = CSharpSyntaxTree.ParseText(
                content,
                parseOptions,
                path,
                cancellationToken: cancellation);
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

    public DtoModel GetDtoByType(string typeName)
    {
        var files = State.SyntaxTrees.Select(x => (CompilationUnitSyntax)x.GetRoot());
        var type = CSharpModelParser.ResolveTargetType(files, typeName);

        return CSharpModelParser.ParseDto(type, State);
    }

    public DtoModel GetDtoByFile(string fileName, string? typeName = null)
    {
        var path = _fileAccessor.ResolveFullPath(fileName);
        var file = (CompilationUnitSyntax)State.SyntaxTrees.First(x => x.FilePath.Equals(path)).GetRoot();
        var type = CSharpModelParser.ResolveTargetType(file, typeName);

        return CSharpModelParser.ParseDto(type, State);
    }
}