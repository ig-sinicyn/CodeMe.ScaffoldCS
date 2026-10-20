namespace CodeMe.ScaffoldCS.Templates.Render.Padding;

public interface IPaddingCollection
{
    string Name { get; }

    PaddingRenderOptions? Options { get; }

    void NextItem();

    IPaddingColumn GetNextColumn();

    IPaddingColumn GetColumn(int index);
}