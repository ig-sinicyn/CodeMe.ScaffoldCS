using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public class PaddingGroup(string name, PaddingRenderOptions options, List<PaddingColumn> sharedColumns)
{
    private int _columnIndex;

    public string Name { get; } = name;

    public PaddingRenderOptions Options { get; set; } = options;

    public void NextItem() => _columnIndex = 0;

    public PaddingColumn GetNextColumn() => GetColumn(_columnIndex++);

    public PaddingColumn GetColumn(int index)
    {
        while (index >= sharedColumns.Count)
        {
            sharedColumns.Add(new PaddingColumn(index: sharedColumns.Count));
        }

        return sharedColumns[index];
    }
}