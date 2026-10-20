using System.Collections;
using CodeMe.ScaffoldCS.Templates.Infrastructure;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render;
using CodeMe.ScaffoldCS.Templates.Render.PartSymbols;
using CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

namespace CodeMe.ScaffoldCS.Templates;

public partial class Template
{
    private readonly List<ITemplateValueSymbol> _buffer = new();

    public Template SetCurrentLineHandling(CurrentLineHandlingMode lineHandling)
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new LineHandlingSymbol(lineHandling));
            return this;
        }

        _textWriter.SetCurrentLineHandling(lineHandling);
        return this;
    }

    public Template ClearLine()
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new ClearLineSymbol());
            return this;
        }

        _textWriter.ClearLine();
        return this;
    }

    public Template WriteLine()
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new NewLineSymbol());
            return this;
        }

        _textWriter.AppendLine();
        return this;
    }

    public Template WriteLiteral(string? value)
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new LiteralSymbol(value));
            return this;
        }

        _textWriter.Append(value);
        return this;
    }

    public Template WriteLiteral(ReadOnlyMemory<char> value)
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new LiteralSymbol(value));
            return this;
        }

        _textWriter.Append(value);
        return this;
    }

    public Template WriteLiteral(ReadOnlySpan<char> value)
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new LiteralSymbol(value.ToArray()));
            return this;
        }

        _textWriter.Append(value);
        return this;
    }

    public Template Write(string? value)
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new ValueSymbol<string?>(value));
            return this;
        }

        if (value.IsMultiline())
        {
            using var _ = this.BeginMultilineStringScope();
            _textWriter.Append(value);
            return this;
        }

        _textWriter.Append(value);
        return this;
    }

    public Template Write(string? value, int alignment)
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new ValueSymbol<string?>(value, alignment));
            return this;
        }

        if (value.IsMultiline())
        {
            using var _ = this.BeginMultilineStringScope();
            _textWriter.Append(value);
            return this;
        }

        _textWriter.Append(value, alignment);
        return this;
    }

    public Template Write(ReadOnlyMemory<char> value)
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new ValueSymbol<ReadOnlyMemory<char>>(value));
            return this;
        }

        if (value.Span.IsMultiline())
        {
            using var _ = this.BeginMultilineStringScope();
            _textWriter.Append(value);
            return this;
        }

        _textWriter.Append(value);
        return this;
    }

    public Template Write(ReadOnlyMemory<char> value, int alignment)
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new ValueSymbol<ReadOnlyMemory<char>>(value, alignment));
            return this;
        }

        if (value.Span.IsMultiline())
        {
            using var _ = this.BeginMultilineStringScope();
            _textWriter.Append(value);
            return this;
        }

        _textWriter.Append(value, alignment);
        return this;
    }

    public Template Write(ReadOnlySpan<char> value)
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new ValueSymbol<string?>(value.ToString()));
            return this;
        }

        if (value.IsMultiline())
        {
            using var _ = this.BeginMultilineStringScope();
            _textWriter.Append(value);
            return this;
        }

        _textWriter.Append(value);
        return this;
    }

    public Template Write(ReadOnlySpan<char> value, int alignment)
    {
        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new ValueSymbol<string?>(value.ToString(), alignment));
            return this;
        }

        if (value.IsMultiline())
        {
            using var _ = this.BeginMultilineStringScope();
            _textWriter.Append(value);
            return this;
        }

        _textWriter.Append(value, alignment);
        return this;
    }

    public Template Write<T>(T? value)
    {
        if (value is ITemplateControlSymbol)
        {
            ((ITemplateControlSymbol)value).Render(this);
            return this;
        }

        if (value is string s)
        {
            Write(s);
            return this;
        }

        if (value is ReadOnlyMemory<char> c)
        {
            Write(c);
            return this;
        }

        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (value is ITemplateValueSymbol && IsBuffered)
        {
            if (IsBuffered)
            {
                _buffer.Add((ITemplateValueSymbol)value);
            }
            else
            {
                ((ITemplateValueSymbol)value).Render(this);
            }

            return this;
        }

        if (value is ITemplateSymbol)
        {
            ((ITemplateSymbol)value).Render(this);
            return this;
        }

        if (value is IInterpolatedString)
        {
            return this;
        }

        if (value is IEnumerable)
        {
            ((IEnumerable)value)
                .Cast<object>()
                .Render(DefaultCollectionRenderOptions)
                .Render(this);
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new ValueSymbol<T?>(value));
        }
        else
        {
            _textWriter.Append(value);
        }

        return this;
    }

    public Template Write<T>(T? value, int alignment, string? format = null)
    {
        if (value is ITemplateControlSymbol)
        {
            ((ITemplateControlSymbol)value).Render(this);
            return this;
        }

        if (value is string s)
        {
            Write(s, alignment);
            return this;
        }

        if (value is ReadOnlyMemory<char> c)
        {
            Write(c, alignment);
            return this;
        }

        if (!_branchState.ShouldWrite)
        {
            return this;
        }

        if (value is ITemplateValueSymbol && IsBuffered)
        {
            if (IsBuffered)
            {
                _buffer.Add((ITemplateValueSymbol)value);
            }
            else
            {
                ((ITemplateValueSymbol)value).Render(this);
            }

            return this;
        }

        if (value is ITemplateSymbol)
        {
            ((ITemplateSymbol)value).Render(this);
            return this;
        }

        if (value is IInterpolatedString)
        {
            return this;
        }

        if (value is IEnumerable)
        {
            ((IEnumerable)value)
                .Cast<object>()
                .Render(
                    DefaultCollectionRenderOptions with
                    {
                        Alignment = alignment,
                        Format = format
                    })
                .Render(this);
            return this;
        }

        if (IsBuffered)
        {
            _buffer.Add(new ValueSymbol<T?>(value));
        }
        else
        {
            _textWriter.Append(value, alignment, format);
        }

        return this;
    }

    public Template Write(Action<Template> callback) => Write(new TemplateActionSymbol(callback));

    public Template Write(Func<TemplateString> callback) => Write(new TemplateStringSymbol(callback));

    public Template Write(IEnumerable values) => Write(values.Cast<object>().Render(DefaultCollectionRenderOptions));

    public Template Write<T>(IEnumerable<T> values) => Write(values.Render(DefaultCollectionRenderOptions));
}