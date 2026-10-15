using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Redner.Symbols;
using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates;

public partial class Template : ITemplateWriter
{
    void ITemplateWriter.WriteLiteral(string? value) => WriteLiteral(value);

    void ITemplateWriter.WriteLiteral(ReadOnlySpan<char> value) => WriteLiteral(value);

    void ITemplateWriter.Write<T>(T? value) where T : default => Write<T>(value);

    void ITemplateWriter.Write<T>(T? value, int alignment) where T : default => Write<T>(value, alignment);

    void ITemplateWriter.Write<T>(T? value, string? format) where T : default => Write<T>(value, 0, format);

    void ITemplateWriter.Write<T>(T? value, int alignment, string? format) where T : default =>
        Write<T>(value, alignment, format);

    void ITemplateWriter.Write(string? value) => Write(value);

    void ITemplateWriter.Write(string? value, int alignment) => Write(value, alignment);

    void ITemplateWriter.Write(ReadOnlySpan<char> value) => Write(value);

    void ITemplateWriter.Write(ReadOnlySpan<char> value, int alignment) => Write(value, alignment);

    void ITemplateWriter.Write(Action<Template> callback) => Write(new TemplateActionSymbol(callback));

    void ITemplateWriter.Write(Func<TemplateString> callback) => Write(new TemplateStringSymbol(callback));

    void ITemplateWriter.Write<T>(IEnumerable<T> values) => Write(values.Render(DefaultCollectionRenderOptions));
}