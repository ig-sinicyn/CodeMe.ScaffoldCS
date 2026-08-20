using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Render;

public readonly struct CollectionSymbol<T>(
    IEnumerable<T> source,
    Func<T, TemplatePart>? callback,
    CollectionRenderOptions options) : ITemplateSymbol
{
    public void Render(Template template)
    {
        var hasFormat = callback == null && (options.Alignment != 0 || options.Format != null);

        var hasItem = false;
        using var _ = template.BeginOptionsScope(IndentationFormat.CurrentLine);
        foreach (var item in source)
        {
            if (hasItem && !string.IsNullOrEmpty(options.Separator))
            {
                template.WriteLiteral(options.Separator);
            }

            if (callback != null)
            {
                template.Write(callback(item));
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