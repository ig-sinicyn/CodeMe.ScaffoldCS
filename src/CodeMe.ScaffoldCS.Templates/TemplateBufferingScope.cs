namespace CodeMe.ScaffoldCS.Templates;

public readonly struct TemplateBufferingScope : IDisposable
{
    private readonly Template _owner;
    private readonly bool _stopBuffering;

    internal TemplateBufferingScope(Template owner, bool stopBuffering)
    {
        _owner = owner;
        _stopBuffering = stopBuffering;
    }

    public void Dispose() => _owner.EndBufferingScope(_stopBuffering);
}