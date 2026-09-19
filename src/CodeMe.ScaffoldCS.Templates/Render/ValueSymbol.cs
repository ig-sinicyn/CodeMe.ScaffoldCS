using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

public readonly struct ValueSymbol<T>(T value, int alignment = 0, string? format = null) : ITemplateSymbol
{
    public void Render(Template template)
    {
        var hasFormat = alignment != 0 || format != null;
        if (hasFormat)
        {
            template.Write(value, alignment, format);
        }
        else
        {
            template.Write(value);
        }
    }
}