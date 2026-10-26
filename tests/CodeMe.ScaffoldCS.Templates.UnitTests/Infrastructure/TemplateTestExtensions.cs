using AwesomeAssertions.Execution;

namespace CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure;

internal static class TemplateTestExtensions
{
    public static string GetOutput(this Template template)
    {
        var output = (StringWriter)template.GetTestAccessor().Output;

        return output.ToString();
    }

    public static Template ShouldBe(
        this Template template,
        string expected)
    {
        var testAccessor = template.GetTestAccessor();
        var output = (StringWriter)testAccessor.Output;
        template.Close();

        using var _ = new AssertionScope();
        var rendered = output.ToString();
        rendered.Should().Be(expected);

        return template;
    }
}