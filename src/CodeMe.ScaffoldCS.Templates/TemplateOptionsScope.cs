using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates;

public readonly struct TemplateOptionsScope : IDisposable
{
    private readonly Template _owner;
    private readonly object? _scopeValue;

    internal TemplateOptionsScope(Template owner, TemplateTextWriterOptions options)
    {
        _owner = owner;
        _scopeValue = options;
    }

    internal TemplateOptionsScope(Template owner, Action<Template> customDispose)
    {
        _owner = owner;
        _scopeValue = customDispose;
    }

    internal TemplateOptionsScope(Template owner, IDisposable customDispose)
    {
        _owner = owner;
        _scopeValue = customDispose;
    }

    public void Dispose()
    {
        switch (_scopeValue)
        {
            case TemplateTextWriterOptions options:
                _owner.EndOptionsScope(options);
                break;

            case Action<Template> disposeCallback:
                disposeCallback(_owner);
                break;

            case IDisposable disposable:
                disposable.Dispose();
                break;
        }
    }
}