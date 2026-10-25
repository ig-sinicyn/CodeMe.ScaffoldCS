namespace CodeMe.ScaffoldCS.Templates;

public readonly struct TemplateOptionsScope : IDisposable
{
    private readonly Template _owner;

    internal TemplateOptionsScope(Template owner)
    {
        _owner = owner;
    }

    public void Dispose() => _owner.CompleteOptionsScope();
}