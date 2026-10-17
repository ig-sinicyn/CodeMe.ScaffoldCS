using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates;

public partial class Template : ITemplateValueWriter
{
    void ITemplateValueWriter.WriteLiteral(string? value) => WriteLiteral(value);

    void ITemplateValueWriter.WriteLiteral(ReadOnlySpan<char> value) => WriteLiteral(value);

    void ITemplateValueWriter.Write<T>(T? value) where T : default => Write<T>(value);

    void ITemplateValueWriter.Write<T>(T? value, int alignment) where T : default => Write<T>(value, alignment);

    void ITemplateValueWriter.Write<T>(T? value, string? format) where T : default => Write<T>(value, 0, format);

    void ITemplateValueWriter.Write<T>(T? value, int alignment, string? format) where T : default =>
        Write<T>(value, alignment, format);

    void ITemplateValueWriter.Write(string? value) => Write(value);

    void ITemplateValueWriter.Write(string? value, int alignment) => Write(value, alignment);

    void ITemplateValueWriter.Write(ReadOnlySpan<char> value) => Write(value);

    void ITemplateValueWriter.Write(ReadOnlySpan<char> value, int alignment) => Write(value, alignment);

    void ITemplateValueWriter.Write(Action<Template> callback) => Write(new TemplateActionSymbol(callback));

    void ITemplateValueWriter.Write(Func<TemplateString> callback) => Write(new TemplateStringSymbol(callback));

    void ITemplateValueWriter.Write<T>(IEnumerable<T> values) => Write(values.Render(DefaultCollectionRenderOptions));
}