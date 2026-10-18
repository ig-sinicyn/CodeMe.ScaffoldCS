using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public readonly struct TemplateOptionsScope : IDisposable
{
    private readonly ITemplateValueWriter _owner;
    private readonly object? _scopeValue;

    public TemplateOptionsScope(ITemplateValueWriter owner, TemplateTextWriterOptions options)
    {
        _owner = owner;
        _scopeValue = options;
    }

    public TemplateOptionsScope(ITemplateValueWriter owner, Action customDispose)
    {
        _owner = owner;
        _scopeValue = customDispose;
    }

    public TemplateOptionsScope(ITemplateValueWriter owner, IDisposable customDispose)
    {
        _owner = owner;
        _scopeValue = customDispose;
    }

    public void Dispose()
    {
        switch (_scopeValue)
        {
            case TemplateTextWriterOptions options:
                _owner.SetOptions(options);
                break;

            case Action disposeCallback:
                disposeCallback();
                break;

            case IDisposable disposable:
                disposable.Dispose();
                break;
        }
    }
}