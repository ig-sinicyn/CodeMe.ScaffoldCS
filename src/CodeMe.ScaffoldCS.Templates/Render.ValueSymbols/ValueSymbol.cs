using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public readonly struct ValueSymbol<T>(T value, int alignment = 0, string? format = null) : ITemplateValueSymbol
{
    public void Render(ITemplateValueWriter writer)
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