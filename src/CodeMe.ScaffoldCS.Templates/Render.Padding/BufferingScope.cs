namespace CodeMe.ScaffoldCS.Templates.Render.Padding;

public readonly struct BufferingScope(Template owner, bool rootScope) : IDisposable
{
    public void Dispose() => owner.EndBufferingScope(rootScope);
}