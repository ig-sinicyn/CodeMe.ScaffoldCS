using CodeMe.ScaffoldCS.Templates.Internals.Padding;

namespace CodeMe.ScaffoldCS.Templates.Render;

public class PaddingSymbol<T>(T value, PaddingMode padding = PaddingMode.Auto, int minLength = 0, string? format = null)
    : IPaddingSymbol
{
    public void Render(Template template)
    {
    }
}