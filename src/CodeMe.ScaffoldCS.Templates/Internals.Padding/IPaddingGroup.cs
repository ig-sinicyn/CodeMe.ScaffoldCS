using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates.Internals.Padding;

public interface IPaddingGroup
{
    string Name { get; }

    PaddingRenderOptions Options { get; }

    void NextItem();

    IPaddingColumn GetNextColumn();

    IPaddingColumn GetColumn(int index);
}