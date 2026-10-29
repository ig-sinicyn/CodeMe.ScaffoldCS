using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

internal readonly struct CollectionSymbolPart<T> : ITemplateSymbol
{
    private readonly IEnumerable<T> _source;
    private readonly object? _callback;
    private readonly CollectionRenderOptions? _options;

    public CollectionSymbolPart(
        IEnumerable<T> source,
        Func<T, TemplateString>? callback = null,
        CollectionRenderOptions? options = null)
    {
        _source = source;
        _callback = callback;
        _options = options;
    }

    public CollectionSymbolPart(
        IEnumerable<T> source,
        Action<Template, T> callback,
        CollectionRenderOptions? options = null)
    {
        _source = source;
        _callback = callback;
        _options = options;
    }

    public void Render(Template template)
    {
        using var _ = template.BeginMultilineStringScope();

        var options = _options ?? template.DefaultCollectionRenderOptions;

        var hasItem = false;
        var hasFormat = options.Alignment != 0 || options.Format != null;
        foreach (var item in _source)
        {
            if (hasItem && !string.IsNullOrEmpty(options.Separator))
            {
                template.WriteLiteral(options.Separator);
            }

            if (_callback is Func<T, TemplateString> x)
            {
                template.Write(TemplateStringSymbol.Create(item, x));
            }
            else if (_callback is Action<Template, T> x2)
            {
                template.Write(TemplateActionSymbol.Create(item, x2));
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