namespace CodeMe.ScaffoldCS.Templates.Render.Padding;

public class PaddingColumn(int index) : IPaddingColumn
{
    public int Index => index;
    public int ColumnWidth { get; private set; }

    public void ObserveValueWidth(int width) => ColumnWidth = Math.Max(width, ColumnWidth);
}