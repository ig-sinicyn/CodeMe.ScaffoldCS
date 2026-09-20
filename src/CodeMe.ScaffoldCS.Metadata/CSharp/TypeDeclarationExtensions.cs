using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

internal static class TypeDeclarationExtensions
{
    extension(TypeDeclarationSyntax type)
    {
        public string Name => type.Identifier.ValueText;

        public string? GetNamespace()
        {
            var result = string.Join(
                '.',
                type.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().Select(x => x.Name.ToString()).Reverse());

            return result.Length == 0 ? null : result;
        }

        public string? GetFullName()
        {
            var namespaceName = type.GetNamespace();

            return namespaceName == null
                ? type.Identifier.ValueText
                : $"{namespaceName}.{type.Identifier.ValueText}";
        }

        public bool NameMatches(string typeName) =>
            string.Equals(type.Identifier.ValueText, typeName, StringComparison.Ordinal);

        public bool FullNameMatches(string typeName) =>
            string.Equals(type.GetFullName(), typeName, StringComparison.Ordinal);
    }
}