using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct CollectionSymbol<T>(
    IEnumerable<T> source,
    Func<T, TemplateString>? callback,
    CollectionRenderOptions options) : ITemplateSymbol
{
    public void Render(Template template)
    {
        var hasFormat = options.Alignment != 0 || options.Format != null;
        var hasItem = false;

        using var _ = template.BeginMultilineStringScope();
        foreach (var item in source)
        {
            if (hasItem && !string.IsNullOrEmpty(options.Separator))
            {
                template.WriteLiteral(options.Separator);
            }

            if (callback != null)
            {
                template.Write(TemplateStringSymbol.Create(item, callback));
            }
            else if (hasFormat)
            {
                template.Write(item, options.Alignment, options.Format);
            }
            else
            {
                template.Write(item);
            }

            hasItem = true;
        }

        if (!hasItem && options.EmptyValue != null)
        {
            template.Write(options.EmptyValue);
        }
    }
}