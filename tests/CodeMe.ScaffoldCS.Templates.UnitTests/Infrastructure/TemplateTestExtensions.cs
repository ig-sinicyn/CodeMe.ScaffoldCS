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
        string rendered)
    {
        var testAccessor = writer.GetTestAccessor();
        var output = (StringWriter)testAccessor.Output;
        writer.Close();

        using var _ = new AssertionScope();
        var current = output.ToString();
        current.Should().Be(rendered);
        testAccessor.BranchesStack.Count.Should().Be(0);
        testAccessor.OptionsStack.Count.Should().Be(0);

        return writer;
    }
}