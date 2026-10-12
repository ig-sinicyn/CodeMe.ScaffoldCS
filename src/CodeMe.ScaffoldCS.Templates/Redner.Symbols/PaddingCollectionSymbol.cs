using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Internals.Padding;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct PaddingCollectionSymbol<T>(
    IEnumerable<T> source,
    Func<T, PaddedTemplatePart> callback,
    CollectionRenderOptions options,
    PaddingRenderOptions paddingOptions) : ITemplateSymbol
{
    public void Render(Template template)
    {
        var accumulator = new PaddingAccumulator(template)
        {
            DefaultPaddingRenderOptions = paddingOptions
        };

        using var _ = template.BeginOptionsScope(IndentationFormat.CurrentLine);
        using (accumulator.BeginAmbientScope())
        {
            var hasItem = false;
            ITemplateSymbol? separator =
                string.IsNullOrEmpty(options.Separator) ? null : new LiteralSymbol(options.Separator);
            foreach (var item in source)
            {
                if (hasItem)
                {
                    if (separator != null)
                    {
                        accumulator.Write(separator);
                    }

                    accumulator.NextRow();
                }

                callback(item);

                hasItem = true;
            }

            if (!hasItem && options.EmptyValue != null)
            {
                accumulator.Write(options.EmptyValue);
            }
        }

        template.Write(accumulator);
    }
}