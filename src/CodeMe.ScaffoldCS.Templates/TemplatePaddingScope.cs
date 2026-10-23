using CodeMe.ScaffoldCS.Templates.Internals.Padding;

namespace CodeMe.ScaffoldCS.Templates;

public readonly struct TemplatePaddingScope(Template owner, IPaddingGroup? padding, bool stopBuffering)
    : IDisposable
{
    public void Dispose() => owner.EndPaddingScope(padding, stopBuffering);
}