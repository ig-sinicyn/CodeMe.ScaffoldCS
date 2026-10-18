using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render.PartSymbols;
using CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

namespace CodeMe.ScaffoldCS.Templates.Render.Writers;

internal class AccumulatedTemplatePartWriter(
    List<ITemplateValueSymbol> output,
    ConditionBranchState branchState) : ITemplatePartWriter
{
    public TemplateOptionsScope BeginOptionsScope(IndentationFormat indentation)
    {
        var startSymbol = new TemplateOptionsScopeStartSymbol(indentation);
        output.Add(startSymbol);
        return new TemplateOptionsScope(this, () => output.Add(startSymbol.EndSymbol));
    }

    public void SetOptions(TemplateTextWriterOptions options)
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.Add(new TemplateOptionsSymbol(options));
    }

    public void SetCurrentLineHandling(CurrentLineHandlingMode lineHandling)
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.Add(new LineHandlingSymbol(lineHandling));
    }

    public void ClearLine()
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.Add(new ClearLineSymbol());
    }

    public void WriteLine()
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.Add(new NewLineSymbol());
    }

    public void WriteLiteral(string? value)
    {
        if (!branchState.ShouldWrite)
        {
            return;
        }

        output.Add(new LiteralSymbol(value));
    }

    public void WriteLiteral(ReadOnlySpan<char> value) =>
        WriteLiteral(value.ToString());

    public void Write(string? value) => Write<string?>(value);

    public void Write(string? value, int alignment) => Write<string?>(value, alignment);

    public void Write(ReadOnlySpan<char> value) => Write(value.ToString());

    public void Write(ReadOnlySpan<char> value, int alignment) => Write(value.ToString(), alignment);

    public void Write<T>(T? value)
    {
        if (value is ITemplateControlSymbol)
        {
            ((ITemplateControlSymbol)value).Render(this);
            return;
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

        if (value is ITemplateValueSymbol symbol)
        {
            output.Add(symbol);
            return;
        }

        if (value is IInterpolatedString)
        {
            return;
        }

        output.Add(new ValueSymbol<T?>(value));
    }

    public void Write<T>(T? value, int alignment, string? format = null)
    {
        if (value is ITemplateControlSymbol)
        {
            ((ITemplateControlSymbol)value).Render(this);
            return;
        }

        if (value is string s)
        {
            Write(s, alignment);
            return;
        }

        if (!branchState.ShouldWrite)
        {
            return;
        }

        if (value is ITemplateValueSymbol symbol)
        {
            output.Add(symbol);
            return;
        }

        if (value is IInterpolatedString)
        {
            return;
        }

        output.Add(new ValueSymbol<T?>(value, alignment, format));
    }

    public void BeginConditionBranch(ConditionBranch branch, bool condition) =>
        branchState.BeginBranch(branch, condition);

    public void Write(Action<ITemplatePartWriter> callback) => Write(new TemplateActionSymbol(callback));

    public void Write(Func<TemplateString> callback) => Write(new TemplateStringSymbol(callback));

    public void Write<T>(IEnumerable<T> values) => throw new NotImplementedException();
}