namespace CodeMe.ScaffoldCS.Templates.Internals.Padding;

public interface IPaddingColumn
{
    int Index { get; }

    int Width { get; }

    void ObserveValueWidth(int width);
}