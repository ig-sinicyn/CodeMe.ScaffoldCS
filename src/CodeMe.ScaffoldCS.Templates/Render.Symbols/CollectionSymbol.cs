using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Render.Parts;

public readonly struct CollectionSymbol<T>(
    IEnumerable<T> source,
    Func<T, TemplateString>? callback,
    CollectionRenderOptions options) : ITemplateSymbol
{
    public void Render(ITemplatePartWriter writer)
    {
        var hasFormat = options.Alignment != 0 || options.Format != null;
        var hasItem = false;
        using var _ = writer.BeginOptionsScope(IndentationFormat.CurrentLine);
        foreach (var item in source)
        {
            if (hasItem && !string.IsNullOrEmpty(options.Separator))
            {
                writer.WriteLiteral(options.Separator);
            }

            if (callback != null)
            {
                writer.Write(callback(item));
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