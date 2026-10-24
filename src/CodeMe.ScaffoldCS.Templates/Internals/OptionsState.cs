using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public class OptionsState(TemplateTextWriterOptions defaultOptions)
{
    private readonly Stack<TemplateTextWriterOptions> _optionsStack = new();

    public bool IsEmpty => _optionsStack.Count == 0;

    public TemplateTextWriterOptions Options { get; private set; } = defaultOptions;

    public void BeginOptionsScope(TemplateTextWriterOptions options)
    {
        _optionsStack.Push(options);
        Options = options;
    }

    public void CompleteOptionsScope() => Options = _optionsStack.Pop();
}