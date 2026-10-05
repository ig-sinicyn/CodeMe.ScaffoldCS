using System.Text;
using CodeMe.ScaffoldCS.Templates.Infrastructure;

namespace CodeMe.ScaffoldCS.Templates.Output;

internal sealed class TemplateTextWriter : IDisposable
{
    private readonly TextWriter _output;
    private readonly bool _leaveOutputOpen;
    private readonly StringBuilder _currentLine;
    private StringBuilder.AppendInterpolatedStringHandler _currentLineFormatter;
    private CurrentLineState _currentLineState;
    private CurrentLineHandlingMode _currentLineHandlingMode;

    private CurrentLineHandlingMode _nextLineHandlingMode;

    public TemplateTextWriter(TextWriter output, TemplateTextWriterOptions? options = null, bool leaveOpen = false)
    {
        _output = output;
        _leaveOutputOpen = leaveOpen;
        _currentLine = new StringBuilder();
        Options = options ?? TemplateTextWriterOptions.Default;
    }

    public TemplateTextWriterOptions Options
    {
        get;
        set
        {
            var cultureChanged = !ReferenceEquals(value.Culture, field?.Culture);
            field = value;
            if (cultureChanged)
            {
                InitFormatter();
            }
        }
    }

    private void InitFormatter() =>
        _currentLineFormatter = new StringBuilder.AppendInterpolatedStringHandler(0, 0, _currentLine, Options.Culture);

    public ReadOnlyMemory<char> GetCurrentLineIndentation(IndentationFormat format)
    {
        switch (format)
        {
            case IndentationFormat.Empty:
                return Array.Empty<char>();
            case IndentationFormat.IncreasedIndentation:
                return string.Concat(Options.Indentation.Span, Options.SingleIndentation.Span).AsMemory();
            case IndentationFormat.CurrentIndentation:
                return Options.Indentation;
            case IndentationFormat.CurrentLine when _currentLineState != CurrentLineState.ValueAppended:
                return Options.Indentation;
            case IndentationFormat.CurrentLine:
                var currentLine = new char[_currentLine.Length];
                _currentLine.CopyTo(0, currentLine, _currentLine.Length);
                return currentLine;
            case IndentationFormat.CurrentLineWhitespace:
                char[] result;
                if (_currentLineState != CurrentLineState.ValueAppended)
                {
                    result = Options.Indentation.ToArray();
                }
                else
                {
                    result = new char[_currentLine.Length];
                    _currentLine.CopyTo(0, result, _currentLine.Length);
                }

                for (var i = 0; i < result.Length; i++)
                {
                    if (!char.IsWhiteSpace(result[i]))
                    {
                        result[i] = ' ';
                    }
                }

                return result;
            default:
                throw new ArgumentOutOfRangeException(nameof(format), format, null);
        }
    }

    public TemplateTextWriter SetCurrentLineHandling(CurrentLineHandlingMode lineHandling)
    {
        if (_currentLineState is CurrentLineState.CrAppended
            or CurrentLineState.LfAppended
            or CurrentLineState.CrLfAppended)
        {
            _nextLineHandlingMode = lineHandling;
        }
        else
        {
            _currentLineHandlingMode = lineHandling;
        }

        return this;
    }

    public TemplateTextWriter ClearLine()
    {
        ClearLineCore();
        return this;
    }

    public TemplateTextWriter Append<T>(T? value)
    {
        if (value is null)
        {
            return AppendCore([]);
        }

        if (value is string @string)
        {
            return AppendCore(@string);
        }

        if (value is ReadOnlyMemory<char> memory)
        {
            return AppendCore(memory.Span);
        }

        BeforeAppend(CurrentLineState.ValueAppended);
        _currentLineFormatter.AppendFormatted(value);
        AfterAppend(CurrentLineState.ValueAppended);

        return this;
    }

    public TemplateTextWriter Append<T>(T? value, int alignment, string? format)
    {
        if (value is null)
        {
            return AppendCore([], alignment);
        }

        if (value is string @string)
        {
            return AppendCore(@string, alignment);
        }

        if (value is ReadOnlyMemory<char> memory)
        {
            return AppendCore(memory.Span, alignment);
        }

        BeforeAppend(CurrentLineState.ValueAppended);
        _currentLineFormatter.AppendFormatted(value, alignment, format);
        AfterAppend(CurrentLineState.ValueAppended);

        return this;
    }

    public TemplateTextWriter AppendLine() =>
        AppendCore(GetNewLine());

    public TemplateTextWriter Append(string? value) =>
        AppendCore(value);

    public TemplateTextWriter Append(string? value, int alignment) =>
        AppendCore(value, alignment);

    public TemplateTextWriter Append(ReadOnlyMemory<char> value) =>
        AppendCore(value.Span);

    public TemplateTextWriter Append(ReadOnlyMemory<char> value, int alignment) =>
        AppendCore(value.Span, alignment);

    public TemplateTextWriter Append(ReadOnlySpan<char> value) =>
        AppendCore(value);

    public TemplateTextWriter Append(ReadOnlySpan<char> value, int alignment) =>
        AppendCore(value, alignment);

    private TemplateTextWriter AppendCore(ReadOnlySpan<char> value, int alignment)
    {
        // Align to right. Newlines are ignored.
        if (alignment > value.Length)
        {
            BeforeAppend(CurrentLineState.ValueAppended);
            _currentLine.Append(' ', alignment - value.Length);
            AfterAppend(CurrentLineState.ValueAppended);
        }

        // Append value with newline handling.
        AppendCore(value);

        // Align to left. Newlines are ignored
        if (-alignment > value.Length)
        {
            BeforeAppend(CurrentLineState.ValueAppended);
            _currentLine.Append(' ', -alignment - value.Length);
            AfterAppend(CurrentLineState.ValueAppended);
        }

        return this;
    }

    private TemplateTextWriter AppendCore(ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
        {
            return this;
        }

        var index = 0;
        while (index < value.Length)
        {
            switch (value[index])
            {
                case '\r':
                    BeforeAppend(CurrentLineState.CrAppended);
                    _currentLine.Append(value[index]);
                    index++;
                    AfterAppend(CurrentLineState.CrAppended);
                    break;

                case '\n':
                    var nextLineState = _currentLineState == CurrentLineState.CrAppended
                        ? CurrentLineState.CrLfAppended
                        : CurrentLineState.LfAppended;
                    BeforeAppend(nextLineState);
                    _currentLine.Append(value[index]);
                    index++;
                    AfterAppend(nextLineState);
                    break;

                default:
                    BeforeAppend(CurrentLineState.ValueAppended);
                    var lineEndIndex = value.NextIndexOfAny(index, '\r', '\n');
                    var nextIndex = lineEndIndex < 0 ? value.Length : lineEndIndex;
                    _currentLine.Append(value[index..nextIndex]);
                    index = nextIndex;
                    AfterAppend(CurrentLineState.ValueAppended);
                    break;
            }
        }

        return this;
    }

    public void Close()
    {
        if (_currentLineState == CurrentLineState.Closed)
        {
            return;
        }

        BeforeClose();
        if (!_leaveOutputOpen)
        {
            _output.Dispose();
        }

        AfterClose();
    }

    public void Dispose() => Close();

    private void BeforeAppend(CurrentLineState nextLineState)
    {
        AssertNotClosed();

        var startsNewLine = _currentLineState switch
        {
            CurrentLineState.CrAppended when nextLineState is CurrentLineState.CrLfAppended => false,
            CurrentLineState.CrAppended => true,
            CurrentLineState.LfAppended => true,
            CurrentLineState.CrLfAppended => true,
            _ => false
        };
        if (startsNewLine)
        {
            NormalizeCurrentLineEnd();
            FlushCoreAndResetState();
        }

        var indentation = Options.Indentation;
        if (_currentLineState is CurrentLineState.Empty && !indentation.IsEmpty)
        {
            if (nextLineState == CurrentLineState.ValueAppended)
            {
                _currentLine.Append(indentation);
                _currentLineState = CurrentLineState.ValueAppended;
            }

            if (nextLineState is CurrentLineState.CrAppended or CurrentLineState.LfAppended
                    or CurrentLineState.CrLfAppended
                && Options.AppendIndentationOnEmptyLines)
            {
                _currentLine.Append(indentation);
                _currentLineState = CurrentLineState.ValueAppended;
            }
        }

        if (_currentLineState is CurrentLineState.ValueAppended
            && nextLineState is CurrentLineState.CrAppended or CurrentLineState.LfAppended
                or CurrentLineState.CrLfAppended)
        {
            TrimCurrentLineEnd();
        }
    }

    private void AfterAppend(CurrentLineState nextLineState) => _currentLineState = nextLineState;

    private void BeforeClose()
    {
        AssertNotClosed();

        if (_currentLineState is CurrentLineState.CrAppended or CurrentLineState.LfAppended
            or CurrentLineState.CrLfAppended)
        {
            if (Options.LastLineHandlingMode == LastLineHandlingMode.TrimNewLine)
            {
                RemoveCurrentLineEnd();
            }
            else
            {
                NormalizeCurrentLineEnd();
            }
        }

        if (_currentLineState is CurrentLineState.ValueAppended)
        {
            TrimCurrentLineEnd();
            if (Options.LastLineHandlingMode == LastLineHandlingMode.EnsureNewLine)
            {
                AppendLine();
            }
        }

        if (_currentLineState is CurrentLineState.Empty)
        {
            if (Options.LastLineHandlingMode == LastLineHandlingMode.EnsureNewLine)
            {
                AppendLine();
            }
        }

        FlushCoreAndResetState();
    }

    private void AfterClose() => _currentLineState = CurrentLineState.Closed;

    private void ClearLineCore()
    {
        AssertNotClosed();

        if (_currentLineState is CurrentLineState.ValueAppended)
        {
            _currentLine.Clear();
            _currentLineState = CurrentLineState.Empty;
        }
    }

    private void AssertNotClosed()
    {
        if (_currentLineState == CurrentLineState.Closed)
        {
            throw new ObjectDisposedException("Cannot append text to closed template");
        }
    }

    private string GetNewLine() =>
        Options.NewLineFormat switch
        {
            NewLineFormat.Auto => Environment.NewLine,
            NewLineFormat.CrLf => "\r\n",
            NewLineFormat.Lf => "\n",
            NewLineFormat.Cr => "\r",
            _ => throw new InvalidOperationException($"Unknown new line format {Options.NewLineFormat}")
        };

    private void TrimCurrentLineEnd()
    {
        if (!Options.TrimLineEnd)
        {
            return;
        }

        var newLength = 0;
        for (var i = _currentLine.Length - 1; i >= 0; i--)
        {
            if (!char.IsWhiteSpace(_currentLine[i]))
            {
                newLength = i + 1;
                break;
            }
        }

        if (_currentLine.Length != newLength)
        {
            _currentLine.Length = newLength;
        }
    }

    private void NormalizeCurrentLineEnd()
    {
        if (!Options.NormalizeNewLines)
        {
            return;
        }

        RemoveCurrentLineEnd();
        _currentLine.Append(GetNewLine());
    }

    private void RemoveCurrentLineEnd()
    {
        switch (_currentLine)
        {
            case [.., '\r', '\n']:
                _currentLine.Length -= 2;
                break;
            case [.., '\r']:
                _currentLine.Length -= 1;
                break;
            case [.., '\n']:
                _currentLine.Length -= 1;
                break;
        }
    }

    private void FlushCoreAndResetState()
    {
        AssertNotClosed();

        if (ShouldWriteToOutput())
        {
            _output.Write(_currentLine);
        }

        _currentLine.Clear();
        _currentLineState = CurrentLineState.Empty;
        _currentLineHandlingMode = _nextLineHandlingMode;
        _nextLineHandlingMode = CurrentLineHandlingMode.Normal;
    }

    private bool ShouldWriteToOutput() =>
        _currentLineHandlingMode switch
        {
            CurrentLineHandlingMode.Normal => _currentLine.Length > 0,
            CurrentLineHandlingMode.IgnoreIfEmpty => !CurrentLineIsEmpty(),
            CurrentLineHandlingMode.IgnoreIfWhiteSpace => !CurrentLineIsWhiteSpace(),
            CurrentLineHandlingMode.Ignore => false,
            _ => throw new ArgumentOutOfRangeException()
        };

    private bool CurrentLineIsEmpty() =>
        _currentLine switch
        {
            [] => true,
            ['\r', '\n'] => true,
            ['\r'] => true,
            ['\n'] => true,
            _ => false
        };

    private bool CurrentLineIsWhiteSpace()
    {
        for (var i = _currentLine.Length - 1; i >= 0; i--)
        {
            if (!char.IsWhiteSpace(_currentLine[i]))
            {
                return false;
            }
        }

        return true;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(TemplateTextWriter owner)
    {
        public TextWriter Output => owner._output;

        public string CurrentLine => owner._currentLine.ToString();

        public CurrentLineState CurrentLineState => owner._currentLineState;

        public CurrentLineHandlingMode CurrentLineHandling => owner._currentLineHandlingMode;

        public CurrentLineHandlingMode NextLineHandling => owner._nextLineHandlingMode;
    }
}