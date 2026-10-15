using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates;

[InterpolatedStringHandler]
public readonly struct TemplateString : ITemplateSymbol
{
    private readonly ITemplateWriter _writer;

    public TemplateString(int literalLength, int formattedCount)
        : this(literalLength, formattedCount, AmbientTemplate.Current)
    {
    }

    public TemplateString(int literalLength, int formattedCount, ITemplateWriter writer)
    {
        _writer = writer;
    }

    public void AppendLiteral(string? value) =>
        _writer.WriteLiteral(value);

    public void AppendLiteral(ReadOnlySpan<char> value) =>
        _writer.WriteLiteral(value);

    public void AppendFormatted<T>(T? value) =>
        _writer.Write(value);

    public void AppendFormatted<T>(T? value, int alignment) =>
        _writer.Write(value, alignment);

    public void AppendFormatted<T>(T? value, int alignment, string? format) =>
        _writer.Write(value, alignment, format);

    public void AppendFormatted<T>(T? value, string? format) =>
        _writer.Write(value, alignment: 0, format);

    public void AppendFormatted(string? value) =>
        _writer.Write(value);

    public void AppendFormatted(string? value, int alignment) =>
        _writer.Write(value, alignment);

    public void AppendFormatted(ReadOnlySpan<char> value) =>
        _writer.Write(value);

    public void AppendFormatted(ReadOnlySpan<char> value, int alignment) =>
        _writer.Write(value, alignment);

    public void AppendFormatted(Action<Template> callback) =>
        _writer.Write(callback);

    public void AppendFormatted(Func<TemplateString> callback) =>
        _writer.Write(callback);

    public void AppendFormatted<T>(IEnumerable<T> values) =>
        _writer.Write(values);
}