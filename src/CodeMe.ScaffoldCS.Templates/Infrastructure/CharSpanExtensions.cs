namespace CodeMe.ScaffoldCS.Templates.Infrastructure;

internal static class CharSpanExtensions
{
    public static int NextIndexOf(
        this ReadOnlySpan<char> span,
        int startIndex,
        char value)
    {
        var index = span[startIndex..].IndexOf(value);
        return index < 0 ? index : startIndex + index;
    }

    public static int NextIndexOfAny(
        this ReadOnlySpan<char> span,
        int startIndex,
        char value0,
        char value1)
    {
        var index = span[startIndex..].IndexOfAny(value0, value1);
        return index < 0 ? index : startIndex + index;
    }

    public static int NextIndexOfAny(
        this ReadOnlySpan<char> span,
        int startIndex,
        params ReadOnlySpan<char> separators)
    {
        var index = span[startIndex..].IndexOfAny(separators);
        return index < 0 ? index : startIndex + index;
    }

    public static bool IsMultiline(this ReadOnlySpan<char> span)
    {
        var index = span.IndexOfAny('\r', '\n');
        return index >= 0;
    }

    public static string ToReversedString(this ReadOnlySpan<char> span) => string.Create(
        span.Length, span, (chars, source) =>
        {
            source.CopyTo(chars);
            chars.Reverse();
        });
}