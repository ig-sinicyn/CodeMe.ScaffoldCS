using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

internal static class SymbolExtensions
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

        public MethodSummaryComment GetMethodDocumentationSummary() => GetMethodSummaryFromXml(symbol.GetDocumentationCommentXml());
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

    private static MethodSummaryComment GetMethodSummaryFromXml(string? documentationComment)
    {
        if (string.IsNullOrEmpty(documentationComment))
        {
            return MethodSummaryComment.Empty;
        }

        try
        {
            var xmlDoc = XDocument.Parse(documentationComment);
            var summary = xmlDoc.Descendants("summary").FirstOrDefault()?.Value.Trim();
            var parameters = xmlDoc.Descendants("param")
                .Where(x => x.Attribute("name") != null)
                .ToDictionary(x => x.Attribute("name")!.Value, x => x.Value.Trim());
            return new MethodSummaryComment(summary, parameters);
        }
        catch (XmlException)
        {
            return MethodSummaryComment.Empty;
        }
    }
}