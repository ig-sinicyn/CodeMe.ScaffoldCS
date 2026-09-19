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
        string rendered)
    {
        var testAccessor = template.GetTestAccessor();
        var output = (StringWriter)testAccessor.Output;
        template.Close();

        using var _ = new AssertionScope();
        var current = output.ToString();
        current.Should().Be(rendered);
        testAccessor.BranchesStack.Count.Should().Be(0);
        testAccessor.OptionsStack.Count.Should().Be(0);

        return template;
    }
}