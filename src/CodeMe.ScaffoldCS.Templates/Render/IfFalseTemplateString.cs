using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

[InterpolatedStringHandler]
public readonly struct IfFalseTemplateString<TCondition> : IInterpolatedString
{
    private readonly ITemplatePartWriter _writer;

    public IfFalseTemplateString(
        int literalLength,
        int formattedCount,
        TCondition condition,
        out bool isEnabled)
        : this(literalLength, formattedCount, AmbientTemplate.Current, condition, out isEnabled)
    {
    }

    public IfFalseTemplateString(
        int literalLength,
        int formattedCount,
        ITemplatePartWriter writer,
        TCondition condition,
        out bool isEnabled)
    {
        isEnabled = !Conditions.HasValue(condition);
        _writer = writer;
    }

    public void AppendLiteral(string? value) => _writer.WriteLiteral(value);

    public void AppendLiteral(ReadOnlySpan<char> value) => _writer.WriteLiteral(value);

    public void AppendFormatted(string? value) => _writer.Write(value);

    public void AppendFormatted(string? value, int alignment) => _writer.Write(value, alignment);

    public void AppendFormatted(ReadOnlySpan<char> value) => _writer.Write(value);

    public void AppendFormatted(ReadOnlySpan<char> value, int alignment) => _writer.Write(value, alignment);

    public void AppendFormatted<T>(T? value) => _writer.Write(value);

    public void AppendFormatted<T>(T? value, int alignment) => _writer.Write(value, alignment);

    public void AppendFormatted<T>(T? value, int alignment, string? format) => _writer.Write(value, alignment, format);

    public void AppendFormatted<T>(T? value, string? format) => _writer.Write(value, alignment: 0, format);

    public void AppendFormatted(Action<ITemplatePartWriter> callback) => _writer.Write(callback);

    public void AppendFormatted(Func<TemplateString> callback) => _writer.Write(callback);

    public void AppendFormatted<T>(IEnumerable<T> values) => _writer.Write(values);
}