using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public class OptionsState(TemplateTextWriterOptions defaultOptions)
{
    private readonly Stack<TemplateTextWriterOptions> _optionsStack = new();
    private readonly TemplateTextWriterOptions _defaultOptions = defaultOptions;

    public bool IsEmpty => _optionsStack.Count == 0;

    public TemplateTextWriterOptions Options { get; set; } = defaultOptions;

    public void Clear()
    {
        _optionsStack.Clear();
        Options = _defaultOptions;
    }

    public void BeginOptionsScope(TemplateTextWriterOptions options)
    {
        _optionsStack.Push(options);
        Options = options;
    }

    public void CompleteOptionsScope() => Options = _optionsStack.Pop();
}