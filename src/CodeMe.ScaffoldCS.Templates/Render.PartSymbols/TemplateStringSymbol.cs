using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct TemplateStringSymbol(Func<TemplateString> callback) : ITemplatePartSymbol
{
    public static TemplateStringSymbol Create<T>(T value, Func<T, TemplateString> callback) =>
        new(() => callback(value));

    public void Render(ITemplatePartWriter writer)
    {
        using var _ = writer.BeginMultilineStringScope();
        callback();
    }
}