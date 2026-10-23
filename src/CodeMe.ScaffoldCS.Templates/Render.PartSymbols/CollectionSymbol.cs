using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct CollectionSymbol<T>(
    IEnumerable<T> source,
    Func<T, TemplateString>? callback,
    CollectionRenderOptions options,
    PaddingRenderOptions? paddingOptions = null,
    string? paddingGroupName = null) : ITemplateSymbol
{
    public void Render(Template template)
    {
        using var _ = template.BeginMultilineStringScope();

        // TODO: paddingGroup name
        if (paddingOptions != null)
        {
            using var __ = template.BeginPaddingScope(paddingGroupName, paddingOptions);
            RenderCore(template);
            return;
        }

        RenderCore(template);
    }

    private void RenderCore(Template template)
    {
        var hasFormat = options.Alignment != 0 || options.Format != null;
        var hasItem = false;
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

            template.CurrentPadding?.NextItem();
            hasItem = true;
        }

        if (!hasItem && options.EmptyValue != null)
        {
            template.Write(options.EmptyValue);
        }
    }
}