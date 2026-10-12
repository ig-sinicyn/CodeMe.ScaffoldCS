using System.Collections.Immutable;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

public readonly record struct MethodSummaryComment(string? Summary, IReadOnlyDictionary<string, string> Parameters)
{
    public static readonly MethodSummaryComment Empty = new(null, ImmutableDictionary<string, string>.Empty);
}