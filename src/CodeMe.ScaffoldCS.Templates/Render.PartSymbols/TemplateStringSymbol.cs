using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct TemplateStringSymbol(Func<TemplateString> callback) : ITemplateSymbol
{
    public static TemplateStringSymbol Create<T>(T value, Func<T, TemplateString> callback) =>
        new(() => callback(value));

    public void Render(Template template)
    {
        using var _ = template.BeginMultilineStringScope();
        template.Write(callback());
    }
}