namespace CodeMe.ScaffoldCS.Templates;

public readonly struct TemplateBufferingScope(Template owner, bool stopBuffering) : IDisposable
{
    public void Dispose() => owner.EndBufferingScope(stopBuffering);
}