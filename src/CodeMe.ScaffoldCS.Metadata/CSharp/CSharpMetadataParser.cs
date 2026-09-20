using CodeMe.ScaffoldCS.Metadata.CodeModel;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

internal static class CSharpMetadataParser
{
    public static TypeDeclarationSyntax ResolveTargetType(CompilationUnitSyntax root, string typeName)
    {
        var candidates = root.DescendantNodes()
            .OfType<TypeDeclarationSyntax>()
            .Where(type => type is RecordDeclarationSyntax or ClassDeclarationSyntax or InterfaceDeclarationSyntax)
            .ToArray();

        if (candidates.Length == 0)
        {
            throw new InvalidOperationException($"No class or interface named '{typeName}' was found in the file.");
        }

        if (string.IsNullOrWhiteSpace(typeName))
        {
            return candidates[0];
        }

        var match = candidates.FirstOrDefault(type => type.NameMatches(typeName));

        if (match is null)
        {
            throw new InvalidOperationException($"No class or interface named '{typeName}' was found in the file.");
        }

        return match;
    }

    public static DtoModel ParseDto(TypeDeclarationSyntax declaration, CSharpCompilation compilation)
    {
        var model = compilation.GetSemanticModel(declaration.SyntaxTree);
        var symbol = model.GetDeclaredSymbol(declaration)
            ?? throw new InvalidOperationException(
                $"Unable to resolve the target type '{declaration.Name}'.");

        var fields = declaration.Members
            .OfType<PropertyDeclarationSyntax>()
            .Select(
                property => new EntityField(
                    property.Identifier.ValueText,
                    CSharpTypeResolver.Resolve(property.Type, model),
                    GetXmlSummary(property)))
            .ToArray();

        return new Entity(
            symbol.Name,
            new TypeName(symbol.Name, CSharpTypeResolver.GetNamespace(symbol)),
            GetXmlSummary(targetType),
            fields);
    }
}