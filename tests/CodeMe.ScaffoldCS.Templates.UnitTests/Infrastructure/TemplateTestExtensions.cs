using AwesomeAssertions.Execution;

namespace CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure;

internal static class TemplateTestExtensions
{
    public static string GetOutput(this Template writer)
    {
        var output = (StringWriter)writer.GetTestAccessor().Output;

        return output.ToString();
    }

    public static Template ShouldBe(
        this Template writer,
        string expected)
    {
        var testAccessor = writer.GetTestAccessor();
        var output = (StringWriter)testAccessor.Output;
        writer.Close();

        using var _ = new AssertionScope();
        var rendered = output.ToString();
        rendered.Should().Be(expected);

        return writer;
    }
}