using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public interface ITemplateValueWriter
{
    TemplateTextWriterOptions Options { get; set; }

    CurrentLineHandlingMode CurrentLineHandling { get; set; }

    void ClearLine();

    void WriteLine();

    void WriteLiteral(string? value);

    void WriteLiteral(ReadOnlySpan<char> value);

    void Write(string? value);

    void Write(string? value, int alignment);

    void Write(ReadOnlySpan<char> value);

    void Write(ReadOnlySpan<char> value, int alignment);

    void Write<T>(T? value);

    void Write<T>(T? value, int alignment, string? format = null);
}