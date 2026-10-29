using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

public record PaddingRenderOptions(
    PaddingMode Padding = PaddingMode.Auto,
    char PaddingChar = ' ',
    int MinLength = 0)
{
    public static readonly PaddingRenderOptions Default = new();

    public static readonly PaddingRenderOptions NoPadding = new(PaddingMode.None);

    public static readonly PaddingRenderOptions LeftPadding = new(PaddingMode.Left);

    public static readonly PaddingRenderOptions RightPadding = new(PaddingMode.Right);

    public static readonly PaddingRenderOptions CenterPadding = new(PaddingMode.Center);
}