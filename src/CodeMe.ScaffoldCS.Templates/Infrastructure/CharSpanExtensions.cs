namespace CodeMe.ScaffoldCS.Templates.Infrastructure;

internal static class CharSpanExtensions
{
    public static int NextIndexOfAny(
        this ReadOnlySpan<char> span,
        int startIndex,
        char value0,
        char value1)
    {
        var index = span[startIndex..].IndexOfAny(value0, value1);
        return index < 0 ? index : startIndex + index;
    }

    public static bool IsMultiline(this ReadOnlySpan<char> span)
    {
        var index = span.IndexOfAny('\r', '\n');
        return index >= 0;
    }
}