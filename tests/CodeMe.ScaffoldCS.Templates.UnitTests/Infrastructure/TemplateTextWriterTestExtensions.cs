using System.Globalization;
using AwesomeAssertions.Execution;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure;

internal static class TemplateTextWriterTestExtensions
{
    public static TemplateTextWriter WithCulture(this TemplateTextWriter templateWriter, string cultureName)
    {
        templateWriter.Options = templateWriter.Options with { Culture = new CultureInfo(cultureName) };
        return templateWriter;
    }

    public static TemplateTextWriter WithIndentation(this TemplateTextWriter templateWriter, string indentation)
    {
        templateWriter.Options = templateWriter.Options with { Indentation = indentation.AsMemory() };
        return templateWriter;
    }

    public static TemplateTextWriter WithNewLineFormat(this TemplateTextWriter templateWriter, NewLineFormat format)
    {
        templateWriter.Options = templateWriter.Options with { NewLineFormat = format };
        return templateWriter;
    }

    public static TemplateTextWriter WithIndentationOnEmptyLines(this TemplateTextWriter templateWriter, bool enabled)
    {
        templateWriter.Options = templateWriter.Options with { AppendIndentationOnEmptyLines = enabled };
        return templateWriter;
    }

    public static TemplateTextWriter WithTrimLineEnd(this TemplateTextWriter templateWriter, bool enabled)
    {
        templateWriter.Options = templateWriter.Options with { TrimLineEnd = enabled };
        return templateWriter;
    }

    public static TemplateTextWriter WithNormalizeNewLines(this TemplateTextWriter templateWriter, bool enabled)
    {
        templateWriter.Options = templateWriter.Options with { NormalizeNewLines = enabled };
        return templateWriter;
    }

    public static string GetOutput(this TemplateTextWriter templateWriter)
    {
        var output = (StringWriter)templateWriter.GetTestAccessor().Output;

        return output.ToString();
    }

    public static TemplateTextWriter ShouldBe(
        this TemplateTextWriter templateWriter,
        string rendered,
        string? beforeClose = null)
    {
        var testAccessor = templateWriter.GetTestAccessor();
        var output = (StringWriter)testAccessor.Output;

        var capturedBeforeClose = output.ToString();
        templateWriter.Close();

        using var _ = new AssertionScope();
        output.ToString().Should().Be(rendered);
        if (beforeClose != null)
        {
            capturedBeforeClose.Should().Be(capturedBeforeClose);
        }

        testAccessor.CurrentLine.Should().Be("");
        testAccessor.CurrentLineState.Should().Be(CurrentLineState.Closed);

        return templateWriter;
    }
}