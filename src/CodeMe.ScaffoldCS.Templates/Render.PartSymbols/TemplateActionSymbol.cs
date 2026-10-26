using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct TemplateActionSymbol(Action<Template> callback) : ITemplateSymbol
{
    public static TemplateActionSymbol Create<T>(T value, Action<Template, T> callback) =>
        new(x => callback(x, value));

    public void Render(Template template)
    {
        using var _ = template.BeginMultilineStringScope();
        callback(template);
    }
}