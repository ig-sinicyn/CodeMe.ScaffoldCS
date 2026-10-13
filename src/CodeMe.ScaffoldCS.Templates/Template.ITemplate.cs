using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Redner.Symbols;
using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates;

public partial class Template : ITemplate
{
    void ITemplate.WriteLiteral(string? value) => WriteLiteral(value);

    void ITemplate.WriteLiteral(ReadOnlySpan<char> value) => WriteLiteral(value);

    void ITemplate.Write<T>(T? value) where T : default => Write<T>(value);

    void ITemplate.Write<T>(T? value, int alignment) where T : default => Write<T>(value, alignment);

    void ITemplate.Write<T>(T? value, string? format) where T : default => Write<T>(value, 0, format);

    void ITemplate.Write<T>(T? value, int alignment, string? format) where T : default =>
        Write<T>(value, alignment, format);

    void ITemplate.Write(string? value) => Write(value);

    void ITemplate.Write(string? value, int alignment) => Write(value, alignment);

    void ITemplate.Write(ReadOnlySpan<char> value) => Write(value);

    void ITemplate.Write(ReadOnlySpan<char> value, int alignment) => Write(value, alignment);

    void ITemplate.Write(Action<Template> callback) => Write(new TemplateActionSymbol(callback));

    void ITemplate.Write(Func<TemplatePart> callback) => Write(new TemplatePartSymbol(callback));

    void ITemplate.Write<T>(IEnumerable<T> values) => Write(values.Render(DefaultCollectionRenderOptions));
}