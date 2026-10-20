namespace CodeMe.ScaffoldCS.Templates.Render.Padding;

public interface IPaddingColumn
{
    int Index { get; }

    int ColumnWidth { get; }

    void ObserveValueWidth(int width);
}