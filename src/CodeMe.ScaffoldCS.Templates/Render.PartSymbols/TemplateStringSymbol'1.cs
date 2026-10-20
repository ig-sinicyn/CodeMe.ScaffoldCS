using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct TemplateStringSymbol<TTemplatePart>(Func<TTemplatePart> callback) : ITemplateSymbol
    where TTemplatePart : IInterpolatedString
{
    public static TemplateStringSymbol<TTemplatePart> Create<T>(T value, Func<T, TTemplatePart> callback) =>
        new(() => callback(value));

    public void Render(Template template)
    {
        using var _ = template.BeginMultilineStringScope();
        template.Write(callback());
    }
}