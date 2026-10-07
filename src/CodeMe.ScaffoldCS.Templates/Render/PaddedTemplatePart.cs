using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

public static class Padding
{
    public const int Center = 0;
    public const int Left = 1;
    public const int Right = -1;
}

[InterpolatedStringHandler]
public readonly struct PaddedTemplatePart : ITemplateScope
{
    private readonly Template _template;
    private readonly Template.OptionsScope _scope;

    public PaddedTemplatePart(int literalLength, int formattedCount)
        : this(literalLength, formattedCount, AmbientTemplate.Current)
    {
    }

    public PaddedTemplatePart(int literalLength, int formattedCount, Template template)
    {
        _template = template;
        _scope = template.BeginTemplatePartScope();
    }

    void ITemplateScope.DisposeIfSame(Template caller) =>
        _scope.DisposeIfSame(caller);

    public void AppendLiteral(string? value) =>
        _template.WriteLiteral(value);

    public void AppendFormatted<T>(T? value) =>
        _template.Write(value);

    public void AppendFormatted<T>(T? value, int alignment) =>
        _template.Write(value, alignment);

    public void AppendFormatted<T>(T? value, int alignment, string? format) =>
        _template.Write(value, alignment, format);

    public void AppendFormatted<T>(T? value, string? format) =>
        _template.Write(value, 0, format);

    public void AppendFormatted(string? value) =>
        _template.Write(value);

    public void AppendFormatted(string? value, int alignment) =>
        _template.Write(value, alignment);

    public void AppendFormatted(Action<Template> callback) =>
        callback(_template);

    public void AppendFormatted(Func<TemplatePart> callback) =>
        _template.Write(callback());

    public void AppendFormatted<T>(IEnumerable<T> values) =>
        _template.Write(values);
}