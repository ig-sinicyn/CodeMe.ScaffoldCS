namespace CodeMe.ScaffoldCS.Templates.Render.Padding;

public readonly struct PaddingScope(Template owner, IPaddingCollection? padding, bool stopBuffering) : IDisposable
{
    public void Dispose() => owner.EndPaddingScope(padding, stopBuffering);
}