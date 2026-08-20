using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

[InterpolatedStringHandler]
public readonly struct IfTrueTemplatePart<TCondition> : ITemplatePart
{
    private readonly Template _template;
    private readonly Template.OptionsScope _scope;

    public IfTrueTemplatePart(
        int literalLength,
        int formattedCount,
        TCondition condition,
        out bool isEnabled)
        : this(literalLength, formattedCount, AmbientTemplate.Current, condition, out isEnabled)
    {
    }

    public IfTrueTemplatePart(
        int literalLength,
        int formattedCount,
        Template template,
        TCondition condition,
        out bool isEnabled)
    {
        isEnabled = Conditions.HasValue(condition);
        _template = template;
        if (isEnabled)
        {
            _scope = template.BeginTemplatePartScope();
        }
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