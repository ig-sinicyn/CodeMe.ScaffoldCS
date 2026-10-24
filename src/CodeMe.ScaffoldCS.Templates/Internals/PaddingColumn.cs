namespace CodeMe.ScaffoldCS.Templates.Internals;

public class PaddingColumn(int index)
{
    public int Index => index;

    public int Width { get; private set; }

    public void ObserveValueWidth(int width) => Width = Math.Max(width, Width);
}