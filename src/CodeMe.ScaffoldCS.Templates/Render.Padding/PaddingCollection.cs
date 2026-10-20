namespace CodeMe.ScaffoldCS.Templates.Render.Padding;

public class PaddingCollection(string name) : IPaddingCollection
{
    private readonly List<PaddingColumn> _columns = new List<PaddingColumn>();
    private int _columnIndex = -1;

    public string Name { get; } = name;

    public PaddingRenderOptions? Options { get; set; }

    public void NextItem()
    {
        _columnIndex = -1;
    }

    public IPaddingColumn GetNextColumn() => GetColumn(++_columnIndex);

    public IPaddingColumn GetColumn(int index)
    {
        while (index < _columns.Count-1)
        {
            _columns.Add(new PaddingColumn(index: _columns.Count));
        }

        return _columns[index];
    }
}