using CodeMe.ScaffoldCS.Templates.Infrastructure;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

namespace CodeMe.ScaffoldCS.Templates.Render.Writers;

internal class DirectTemplatePartWriter(TemplateTextWriter output, ConditionBranchState branchState)
    : ITemplatePartWriter
{
    public TemplateOptionsScope BeginOptionsScope(IndentationFormat indentation)
    {
        var oldOptions = output.Options;
        output.Options = output.CaptureOptions(indentation);
        return new TemplateOptionsScope(this, oldOptions);
    }

    public void SetOptions(TemplateTextWriterOptions options)
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.Options = options;
    }

    public void SetCurrentLineHandling(CurrentLineHandlingMode lineHandling)
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.SetCurrentLineHandling(lineHandling);
    }

    public void ClearLine()
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.ClearLine();
    }

    public void WriteLine()
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.AppendLine();
    }

    public void WriteLiteral(string? value)
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.Append(value);
    }

    public void WriteLiteral(ReadOnlySpan<char> value)
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.Append(value);
    }

    public void Write(string? value) => Write(value.AsSpan());

    public void Write(string? value, int alignment) => Write(value.AsSpan(), alignment);

    public void Write(ReadOnlySpan<char> value)
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        if (value.IsMultiline())
        {
            using var _ = this.BeginMultilineStringScope();
            output.Append(value);
        }
        else
        {
            output.Append(value);
        }
    }

    public void Write(ReadOnlySpan<char> value, int alignment)
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        if (value.IsMultiline())
        {
            using var _ = this.BeginMultilineStringScope();
            output.Append(value, alignment);
        }
        else
        {
            output.Append(value, alignment);
        }
    }

    public void Write<T>(T? value)
    {
        if (value is ITemplateControlSymbol)
        {
            ((ITemplateControlSymbol)value).Render(this);
        }

        if (value is string s)
        {
            Write(s);
            return;
        }

        if (!branchState.ShouldWrite)
        {
            return;
        }

        if (value is ITemplatePartSymbol)
        {
            ((ITemplatePartSymbol)value).Render(this);
            return;
        }

        output.Append(value);
    }

    public void Write<T>(T? value, int alignment, string? format = null)
    {
        if (value is ITemplateControlSymbol)
        {
            ((ITemplateControlSymbol)value).Render(this);
        }

        if (value is string s)
        {
            Write(s);
            return;
        }

        if (!branchState.ShouldWrite)
        {
            return;
        }

        if (value is ITemplatePartSymbol)
        {
            ((ITemplatePartSymbol)value).Render(this);
            return;
        }

        output.Append(value, alignment, format);
    }

    public void BeginConditionBranch(ConditionBranch branch, bool condition) =>
        branchState.BeginBranch(branch, condition);

    public void Write(Action<ITemplatePartWriter> callback) => Write(new TemplateActionSymbol(callback));

    public void Write(Func<TemplateString> callback) => Write(new TemplateStringSymbol(callback));

    public void Write<T>(IEnumerable<T> values) => throw new NotImplementedException();
}