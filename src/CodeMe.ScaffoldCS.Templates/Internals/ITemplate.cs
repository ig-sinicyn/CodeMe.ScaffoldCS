using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public interface ITemplate
{
    void WriteLiteral(string? value);

    void WriteLiteral(ReadOnlySpan<char> value);

    void Write<T>(T? value);

    void Write<T>(T? value, int alignment);

    void Write<T>(T? value, int alignment, string? format);

    void Write<T>(T? value, string? format);

    void Write(string? value);

    void Write(string? value, int alignment);

    void Write(ReadOnlySpan<char> value);

    void Write(ReadOnlySpan<char> value, int alignment);

    void Write(Action<Template> callback);

    void Write(Func<TemplatePart> callback);

    void Write<T>(IEnumerable<T> values);
}