namespace CodeMe.ScaffoldCS.Templates.Internals.Padding;

public class PaddingColumn(int index) : IPaddingColumn
{
    public int Index => index;

    public int Width { get; private set; }

    public void ObserveValueWidth(int width) => Width = Math.Max(width, Width);
}