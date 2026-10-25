using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates;

[InterpolatedStringHandler]
public readonly struct TemplateString : IInterpolatedString
{
    private readonly Template _template;

    public TemplateString(int literalLength, int formattedCount)
        : this(literalLength, formattedCount, AmbientTemplate.Current)
    {
    }

    public TemplateString(int literalLength, int formattedCount, Template template)
    {
        _template = template;
    }

    public void AppendLiteral(string? value) => _template.WriteLiteral(value);

    public void AppendLiteral(ReadOnlySpan<char> value) => _template.WriteLiteral(value);

    public void AppendFormatted(string? value) => _template.Write(value);

    public void AppendFormatted(string? value, int alignment) => _template.Write(value, alignment);

    public void AppendFormatted(ReadOnlySpan<char> value) => _template.Write(value);

    public void AppendFormatted(ReadOnlySpan<char> value, int alignment) => _template.Write(value, alignment);

    public void AppendFormatted<T>(T value) => _template.Write(value);

    public void AppendFormatted<T>(T value, int alignment) => _template.Write(value, alignment);

    public void AppendFormatted<T>(T value, int alignment, string? format) => _template.Write(value, alignment, format);

    public void AppendFormatted<T>(T value, string? format) => _template.Write(value, alignment: 0, format);

    public void AppendFormatted(Action<Template> callback) => _template.Write(callback);

    public void AppendFormatted(Func<TemplateString> callback) => _template.Write(callback);

    public void AppendFormatted<T>(IEnumerable<T> values) => _template.Write(values);
}