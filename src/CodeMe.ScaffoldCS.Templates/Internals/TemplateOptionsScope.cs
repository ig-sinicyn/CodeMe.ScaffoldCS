using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public readonly struct TemplateOptionsScope(ITemplatePartWriter owner, TemplateTextWriterOptions options) : IDisposable
{
    public void Dispose() => owner.Options = options;
}