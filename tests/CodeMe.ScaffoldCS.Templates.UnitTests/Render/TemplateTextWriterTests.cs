using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates.UnitTests.Render;

public class NameRenderTests
{
    [Theory]
    [InlineData(null, "", null)]
    [InlineData(null, "Test", null)]
    [InlineData("", "", "")]
    [InlineData("", "Test", "")]
    [InlineData("Test", "", "Test")]
    [InlineData("Test", "Test", "")]
    [InlineData("TestTest", "Test", "Test")]
    [InlineData("TestMe", "Test", "Me")]
    public void TestScenario_ShouldBeExpected(string? input, string prefix, string? expected) =>
        input.TrimPrefix(prefix).Should().Be(expected);
}