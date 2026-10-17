using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct CollectionSymbol<T>(
    IEnumerable<T> source,
    Func<T, TemplateString>? callback,
    CollectionRenderOptions options) : ITemplatePartSymbol
{
    public void Render(ITemplatePartWriter writer)
    {
        var hasFormat = options.Alignment != 0 || options.Format != null;
        var hasItem = false;

        using var _ = writer.BeginMultilineStringScope();
        foreach (var item in source)
        {
            if (hasItem && !string.IsNullOrEmpty(options.Separator))
            {
                writer.WriteLiteral(options.Separator);
            }

            if (callback != null)
            {
                writer.Write(TemplateStringSymbol.Create(item, callback));
            }
            else if (hasFormat)
            {
                writer.Write(item, options.Alignment, options.Format);
            }
            else
            {
                writer.Write(item);
            }

            hasItem = true;
        }

        if (!hasItem && options.EmptyValue != null)
        {
            writer.Write(options.EmptyValue);
        }
    }
}