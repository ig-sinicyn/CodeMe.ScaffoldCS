using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public class OptionsState(TemplateTextWriterOptions defaultOptions)
{
    private readonly Stack<TemplateTextWriterOptions> _optionsStack = new();
    private readonly TemplateTextWriterOptions _defaultOptions = defaultOptions;

    public bool IsEmpty => _optionsStack.Count == 0;

    public TemplateTextWriterOptions CurrentOptions { get; set; } = defaultOptions;

    public void Clear()
    {
        _optionsStack.Clear();
        CurrentOptions = _defaultOptions;
    }

    public TemplateTextWriterOptions BeginOptionsScope(TemplateTextWriterOptions options)
    {
        _optionsStack.Push(CurrentOptions);
        CurrentOptions = options;
        return options;
    }

    public TemplateTextWriterOptions CompleteOptionsScope() => CurrentOptions = _optionsStack.Pop();
}