using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates.Internals.Padding;

public class PaddingGroup(string name, PaddingRenderOptions options) : IPaddingGroup
{
    private readonly List<PaddingColumn> _columns = new();
    private int _columnIndex;

    public string Name { get; } = name;

    public PaddingRenderOptions Options { get; set; } = options;

    public void NextItem() => _columnIndex = 0;

    public IPaddingColumn GetNextColumn() => GetColumn(_columnIndex++);

    public IPaddingColumn GetColumn(int index)
    {
        while (index >= _columns.Count)
        {
            _columns.Add(new PaddingColumn(index: _columns.Count));
        }

        return _columns[index];
    }
}