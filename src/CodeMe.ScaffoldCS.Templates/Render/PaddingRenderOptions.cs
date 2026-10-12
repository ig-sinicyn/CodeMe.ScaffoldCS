using CodeMe.ScaffoldCS.Templates.Internals.Padding;

namespace CodeMe.ScaffoldCS.Templates.Render;

public record PaddingRenderOptions(
    PaddingMode Padding = PaddingMode.Auto,
    char PaddingChar = ' ',
    int MinLength = 0,
    string? Format = null)
{
    public static readonly PaddingRenderOptions Default = new();
}