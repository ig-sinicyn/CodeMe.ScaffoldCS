using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates;

public sealed partial class Template : IDisposable
{
    private readonly TemplateTextWriter _writer;

    public Template(TextWriter output, TemplateTextWriterOptions? options = null, bool leaveOpen = false)
    {
        _writer = new TemplateTextWriter(output, options, leaveOpen);
        ShouldWrite = true;
        ValidateOnClose = true;
        DefaultCollectionRenderOptions = CollectionRenderOptions.Default;
    }

    public bool ValidateOnClose { get; set; }

    public TemplateTextWriterOptions Options
    {
        get => _writer.Options;
        set => _writer.Options = value;
    }

    public CollectionRenderOptions DefaultCollectionRenderOptions { get; set; }

    public bool ShouldWrite { get; private set; }

    public void Dispose()
    {
        if (ValidateOnClose)
        {
            if (_branchesStack.Count > 0)
            {
                throw new InvalidOperationException(
                    "There is unclosed 'if' or 'else' branch. Please fix the template content"
                    + $" or disable the {nameof(ValidateOnClose)} option.");
            }

            if (_optionsStack.Count > 0)
            {
                throw new InvalidOperationException(
                    "There is unclosed options scope."
                    + $" Please dispose all {nameof(BeginOptionsScope)} scopes.");
            }
        }

        _writer.Dispose();
    }

    public void Close() => Dispose();

    public IDisposable BeginAmbientScope() => AmbientTemplate.BeginTemplateScope(this);
}