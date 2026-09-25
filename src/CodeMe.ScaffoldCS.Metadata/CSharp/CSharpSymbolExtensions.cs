using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

internal static class CSharpSymbolExtensions
{
    extension(ITypeSymbol symbol)
    {
        public string? Namespace =>
            symbol.ContainingNamespace is { IsGlobalNamespace: false } ns
                ? ns.ToDisplayString()
                : null;
    }

    extension(ISymbol symbol)
    {
        public string? GetDocumentationSummary() => GetSummaryFromXml(symbol.GetDocumentationCommentXml());
    }

    private static string? GetSummaryFromXml(string? documentationComment)
    {
        if (string.IsNullOrEmpty(documentationComment))
        {
            return null;
        }

        try
        {
            var xmlDoc = XDocument.Parse(documentationComment);
            return xmlDoc.Descendants("summary").FirstOrDefault()?.Value.Trim();
        }
        catch (XmlException)
        {
            return null;
        }
    }
}