namespace CodeMe.ScaffoldCS.Templates.Internals;

public interface IInterpolatedString
{
    void AppendLiteral(string? value);

    void AppendLiteral(ReadOnlySpan<char> value);

    void AppendFormatted(string? value);

    void AppendFormatted(string? value, int alignment);

    void AppendFormatted(ReadOnlySpan<char> value);

    void AppendFormatted(ReadOnlySpan<char> value, int alignment);

    void AppendFormatted<T>(T? value);

    void AppendFormatted<T>(T? value, int alignment);

    void AppendFormatted<T>(T? value, int alignment, string? format);

    void AppendFormatted<T>(T? value, string? format);

    void AppendFormatted(Action<ITemplatePartWriter> callback);

    void AppendFormatted(Func<TemplateString> callback);

    void AppendFormatted<T>(IEnumerable<T> values);
}