using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates;

public sealed partial class Template : IDisposable
{
    private readonly TemplateTextWriter _textWriter;
    private readonly ConditionBranchState _branchState;

    public Template(TextWriter output, TemplateTextWriterOptions? options = null, bool leaveOpen = false)
    {
        _textWriter = new TemplateTextWriter(output, options, leaveOpen);
        _branchState = new ConditionBranchState();
        ShouldWrite = true;
        ValidateOnClose = true;
        DefaultCollectionRenderOptions = CollectionRenderOptions.Default;
        DefaultPaddingOptions = PaddingRenderOptions.Default;
    }

    public bool ShouldWrite { get; private set; }

    public bool ValidateOnClose { get; set; }

    public CollectionRenderOptions DefaultCollectionRenderOptions { get; set; }

    public PaddingRenderOptions DefaultPaddingOptions { get; set; }

    public void Dispose()
    {
        if (ValidateOnClose)
        {
            if (_branchState.CurrentBranch != ConditionBranch.None)
            {
                throw new InvalidOperationException(
                    "There is unclosed 'if' or 'else' branch. Please fix the template content"
                    + $" or disable the {nameof(ValidateOnClose)} option.");
            }

            if (IsBuffered)
            {
                throw new InvalidOperationException(
                    "There is unclosed buffering scope. Please dispose all open scopes"
                    + $" or disable the {nameof(ValidateOnClose)} option.");
            }

            if (CurrentPadding != null)
            {
                throw new InvalidOperationException(
                    "There is unclosed padding scope. Please dispose all open scopes"
                    + $" or disable the {nameof(ValidateOnClose)} option.");
            }
        }

        _textWriter.Dispose();
        _paddings.Clear();
    }

    public void Close() => Dispose();

    public IDisposable BeginAmbientScope() => AmbientTemplate.BeginTemplateScope(this);
}