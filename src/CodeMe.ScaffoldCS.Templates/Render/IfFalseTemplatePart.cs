using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Redner.Symbols;

namespace CodeMe.ScaffoldCS.Templates.Render;

[InterpolatedStringHandler]
public readonly struct IfFalseTemplatePart<TCondition> : ITemplatePart
{
    private readonly Template _template;

    public IfFalseTemplatePart(
        int literalLength,
        int formattedCount,
        TCondition condition,
        out bool isEnabled)
        : this(literalLength, formattedCount, AmbientTemplate.Current, condition, out isEnabled)
    {
    }

    public IfFalseTemplatePart(
        int literalLength,
        int formattedCount,
        Template template,
        TCondition condition,
        out bool isEnabled)
    {
        isEnabled = !Conditions.HasValue(condition);
        _template = template;
    }

    public void AppendLiteral(string? value) =>
        _template.WriteLiteral(value);

    public void AppendLiteral(ReadOnlySpan<char> value) =>
        _template.WriteLiteral(value);

    public void AppendFormatted<T>(T? value) =>
        _template.Write(value);

    public void AppendFormatted<T>(T? value, int alignment) =>
        _template.Write(value, alignment);

    public void AppendFormatted<T>(T? value, int alignment, string? format) =>
        _template.Write(value, alignment, format);

    public void AppendFormatted<T>(T? value, string? format) =>
        _template.Write(value, alignment: 0, format);

    public void AppendFormatted(string? value) =>
        _template.Write(value);

    public void AppendFormatted(string? value, int alignment) =>
        _template.Write(value, alignment);

    public void AppendFormatted(ReadOnlySpan<char> value) =>
        _template.Write(value);

    public void AppendFormatted(ReadOnlySpan<char> value, int alignment) =>
        _template.Write(value, alignment);

    public void AppendFormatted(Action<Template> callback) =>
        _template.Write(new TemplateActionSymbol(callback));

    public void AppendFormatted(Func<TemplatePart> callback) =>
        _template.Write(new TemplatePartSymbol(callback));

    public void AppendFormatted<T>(IEnumerable<T> values) =>
        _template.Write(values.Render(_template.DefaultCollectionRenderOptions));
}