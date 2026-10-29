namespace CodeMe.ScaffoldCS.Templates;

public readonly struct TemplateBufferingScope : IDisposable
{
    private readonly Template _owner;

    internal TemplateBufferingScope(Template owner)
    {
        _owner = owner;
    }

    public void Dispose() => _owner.CompleteBufferingScope();
}