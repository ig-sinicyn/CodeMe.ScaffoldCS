namespace CodeMe.ScaffoldCS.Templates;

public readonly struct TemplatePaddingScope : IDisposable
{
    private readonly Template _owner;
    private readonly IPaddingGroup? _padding;
    private readonly bool _stopBuffering;

    internal TemplatePaddingScope(Template owner, IPaddingGroup? padding, bool stopBuffering)
    {
        _owner = owner;
        _padding = padding;
        _stopBuffering = stopBuffering;
    }

    public void Dispose() => _owner.EndPaddingScope(_padding, _stopBuffering);
}