using System.Collections;
using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Infrastructure;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates;

public partial class Template
{
    public Template SetLineHandling(CurrentLineHandlingMode lineHandling)
    {
        if (ShouldWrite)
        {
            _writer.SetCurrentLineHandling(lineHandling);
        }

        return this;
    }

    public Template ClearLine()
    {
        if (ShouldWrite)
        {
            _writer.ClearLine();
        }

        return this;
    }

    public Template WriteLine()
    {
        if (ShouldWrite)
        {
            _writer.AppendLine();
        }

        return this;
    }

    public Template WriteLiteral(string? value)
    {
        if (ShouldWrite)
        {
            _writer.Append(value);
        }

        return this;
    }

    public Template Write(string? value)
    {
        Write(value.AsSpan());
        return this;
    }

    public Template Write(string? value, int alignment)
    {
        Write(value.AsSpan(), alignment);
        return this;
    }

    public Template WriteLine(string? value)
    {
        Write(value);
        WriteLine();

        return this;
    }

    public Template Write(ReadOnlyMemory<char> value)
    {
        Write(value.Span);

        return this;
    }

    public Template Write(ReadOnlyMemory<char> value, int alignment)
    {
        Write(value.Span, alignment);

        return this;
    }

    public Template WriteLine(ReadOnlyMemory<char> value)
    {
        WriteLine(value.Span);

        return this;
    }

    public Template Write(ReadOnlySpan<char> value)
    {
        if (ShouldWrite)
        {
            if (value.IsMultiline())
            {
                using var _ = BeginOptionsScope(IndentationFormat.CurrentLine);
                _writer.Append(value);
            }
            else
            {
                _writer.Append(value);
            }
        }

        return this;
    }

    public Template Write(ReadOnlySpan<char> value, int alignment)
    {
        if (ShouldWrite)
        {
            _writer.Append(value, alignment);
        }

        return this;
    }

    public Template WriteLine(ReadOnlySpan<char> value)
    {
        Write(value);
        WriteLine();

        return this;
    }

    public Template Write(IEnumerable value)
    {
        if (ShouldWrite)
        {
            value.Cast<object>().Render(DefaultCollectionRenderOptions).Render(this);
        }

        return this;
    }

    public Template Write(IEnumerable value, int alignment, string? format = null)
    {
        if (ShouldWrite)
        {
            var options = DefaultCollectionRenderOptions with
            {
                Alignment = alignment,
                Format = format
            };
            value.Cast<object>().Render(options).Render(this);
        }

        return this;
    }

    // TODO: dictionary symbol
    public Template Write<T>(IEnumerable<T> value)
    {
        if (ShouldWrite)
        {
            value.Render(DefaultCollectionRenderOptions).Render(this);
        }

        return this;
    }

    public Template Write<T>(IEnumerable<T> value, int alignment, string? format = null)
    {
        if (ShouldWrite)
        {
            var options = DefaultCollectionRenderOptions with
            {
                Alignment = alignment,
                Format = format
            };
            value.Render(options).Render(this);
        }

        return this;
    }

    public Template Write([InterpolatedStringHandlerArgument("")] TemplatePart value)
    {
        ((ITemplateScope)value).DisposeIfSame(this);

        return this;
    }

    public Template WriteLine([InterpolatedStringHandlerArgument("")] TemplatePart value)
    {
        Write(value);
        WriteLine();

        return this;
    }

    internal Template CompleteScope<TScoped>(TScoped value)
        where TScoped : ITemplateScope
    {
        value.DisposeIfSame(this);

        return this;
    }

    public Template Write(ITemplateSymbol value)
    {
        if (ShouldWrite || value is IControlTemplateSymbol)
        {
            value.Render(this);
        }

        return this;
    }

    public Template Write<T>(T? value)
    {
        if (value is null)
        {
            Write((string?)null);
            return this;
        }

        if (value is string @string)
        {
            Write(@string);
            return this;
        }

        if (value is ReadOnlyMemory<char> memory)
        {
            Write(memory);
            return this;
        }

        if (value is ITemplateScope)
        {
            ((ITemplateScope)value).DisposeIfSame(this);
            return this;
        }

        if (value is ITemplateSymbol)
        {
            if (ShouldWrite || value is IControlTemplateSymbol)
            {
                ((ITemplateSymbol)value).Render(this);
            }

            return this;
        }

        if (value is IEnumerable enumerable)
        {
            Write(enumerable);
            return this;
        }

        if (ShouldWrite)
        {
            _writer.Append(value);
        }

        return this;
    }

    public Template Write<T>(T? value, int alignment, string? format = null)
    {
        if (value is null)
        {
            Write((string?)null);
            return this;
        }

        if (value is string @string)
        {
            Write(@string, alignment);
            return this;
        }

        if (value is ReadOnlyMemory<char> memory)
        {
            Write(memory, alignment);
            return this;
        }

        if (value is TemplatePart templatePart)
        {
            Write(templatePart);
            return this;
        }

        if (value is ITemplateSymbol)
        {
            if (ShouldWrite || value is IControlTemplateSymbol)
            {
                ((ITemplateSymbol)value).Render(this);
            }

            return this;
        }

        if (value is IEnumerable enumerable)
        {
            Write(enumerable, alignment, format);
            return this;
        }

        if (ShouldWrite)
        {
            _writer.Append(value, alignment, format);
        }

        return this;
    }

    public Template WriteLine<T>(T? value)
    {
        Write(value);
        WriteLine();
        return this;
    }
}