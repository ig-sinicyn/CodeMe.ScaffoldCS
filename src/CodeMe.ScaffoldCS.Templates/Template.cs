using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates;

public sealed partial class Template : IDisposable
{
    private readonly TemplateTextWriter _textWriter;
    private readonly OptionsState _optionsState;
    private readonly ConditionBranchState _branchState;
    private readonly BufferState _bufferState;

    public Template(TextWriter output, TemplateTextWriterOptions? options = null, bool leaveOpen = false)
    {
        options ??= TemplateTextWriterOptions.Default;
        _textWriter = new TemplateTextWriter(output, options, leaveOpen);
        _optionsState = new OptionsState(options);
        _branchState = new ConditionBranchState();
        _bufferState = new BufferState();
        ValidateOnClose = true;
        DefaultCollectionRenderOptions = CollectionRenderOptions.Default;
        DefaultPaddingOptions = PaddingRenderOptions.Default;
    }

    public bool ValidateOnClose { get; set; }

    public CollectionRenderOptions DefaultCollectionRenderOptions { get; set; }

    public PaddingRenderOptions DefaultPaddingOptions { get; set; }

    public void Dispose()
    {
        if (ValidateOnClose)
        {
            if (!_optionsState.IsEmpty)
            {
                throw new InvalidOperationException(
                    "There is unclosed options scope. Please dispose all open scopes"
                    + $" or disable the {nameof(ValidateOnClose)} option.");
            }

            if (!_branchState.IsEmpty)
            {
                throw new InvalidOperationException(
                    "There is unclosed 'if' or 'else' branch. Please fix the template content"
                    + $" or disable the {nameof(ValidateOnClose)} option.");
            }

            if (!_bufferState.IsEmpty)
            {
                throw new InvalidOperationException(
                    "There is unclosed buffering (padding) scope. Please dispose all open scopes"
                    + $" or disable the {nameof(ValidateOnClose)} option.");
            }
        }

        _textWriter.Dispose();
        _optionsState.Clear();
        _branchState.Clear();
        _bufferState.Clear();
    }

    public void Close() => Dispose();

    public IDisposable BeginAmbientScope() => AmbientTemplate.BeginTemplateScope(this);
}