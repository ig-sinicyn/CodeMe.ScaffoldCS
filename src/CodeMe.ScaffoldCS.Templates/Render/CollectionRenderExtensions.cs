using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

public static class CollectionRenderExtensions
{
    public static ITemplateValueSymbol Render<T>(this IEnumerable<T> value, CollectionRenderOptions? options = null) =>
        new CollectionSymbol<T>(value, null, options ?? CollectionRenderOptions.Default);

    public static ITemplateValueSymbol Render<T>(
        this IEnumerable<T> value,
        Func<T, TemplateString> format,
        CollectionRenderOptions? options = null) =>
        new CollectionSymbol<T>(value, format, options ?? CollectionRenderOptions.Default);

    public static ITemplateValueSymbol RenderCommaSeparated<T>(this IEnumerable<T> value) =>
        new CollectionSymbol<T>(value, null, CollectionRenderOptions.CommaSeparated);

    public static ITemplateValueSymbol RenderCommaSeparated<T>(
        this IEnumerable<T> value,
        Func<T, TemplateString> format) =>
        new CollectionSymbol<T>(value, format, CollectionRenderOptions.CommaSeparated);

    public static ITemplateValueSymbol RenderEmptyLineSeparated<T>(this IEnumerable<T> value) =>
        new CollectionSymbol<T>(value, null, CollectionRenderOptions.EmptyLineSeparated);

    public static ITemplateValueSymbol RenderEmptyLineSeparated<T>(
        this IEnumerable<T> value,
        Func<T, TemplateString> format) =>
        new CollectionSymbol<T>(value, format, CollectionRenderOptions.EmptyLineSeparated);
}