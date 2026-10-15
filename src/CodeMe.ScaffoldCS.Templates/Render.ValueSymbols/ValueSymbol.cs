using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct ValueSymbol<T>(T value, int alignment = 0, string? format = null) : ITemplateValueSymbol
{
    public void Render(ITemplateWriter writer)
    {
        var hasFormat = alignment != 0 || format != null;
        if (hasFormat)
        {
            writer.Write(value, alignment, format);
        }
        else
        {
            writer.Write(value);
        }
    }
}