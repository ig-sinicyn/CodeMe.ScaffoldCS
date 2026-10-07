using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates.UnitTests.Render;

public class NameRenderTests
{
    [Theory]
    [InlineData(null, "", null)]
    [InlineData(null, "Prefix", null)]
    [InlineData("", "", "")]
    [InlineData("", "Prefix", "")]
    [InlineData("Value", "", "Value")]
    [InlineData("Value", "Prefix", "Value")]
    [InlineData("Prefix", "Prefix", "")]
    [InlineData("PrefixPrefix", "Prefix", "Prefix")]
    [InlineData("PrefixValue", "Prefix", "Value")]
    [InlineData("ValuePrefix", "Prefix", "ValuePrefix")]
    public void TrimPrefix_ShouldBeExpected(string? input, string prefix, string? expected) =>
        input.TrimPrefix(prefix).Should().Be(expected);

    [Theory]
    [InlineData(null, "", null)]
    [InlineData(null, "Suffix", null)]
    [InlineData("", "", "")]
    [InlineData("", "Suffix", "")]
    [InlineData("Value", "", "Value")]
    [InlineData("Value", "Suffix", "Value")]
    [InlineData("Suffix", "Suffix", "")]
    [InlineData("SuffixSuffix", "Suffix", "Suffix")]
    [InlineData("SuffixValue", "Suffix", "SuffixValue")]
    [InlineData("ValueSuffix", "Suffix", "Value")]
    public void TrimSuffix_ShouldBeExpected(string? input, string suffix, string? expected) =>
        input.TrimSuffix(suffix).Should().Be(expected);

    [Theory]
    [InlineData(null, new string[0], null)]
    [InlineData(null, new[] { "" }, null)]
    [InlineData(null, new[] { "Prefix" }, null)]
    [InlineData("", new string[0], "")]
    [InlineData("", new[] { "" }, "")]
    [InlineData("", new[] { "Prefix" }, "")]
    [InlineData("Value", new string[0], "Value")]
    [InlineData("Value", new[] { "" }, "Value")]
    [InlineData("Value", new[] { "Prefix" }, "Value")]
    [InlineData("PrefixPrefix", new[] { "Prefix" }, "Prefix")]
    [InlineData("PrefixValue", new[] { "Prefix" }, "Value")]
    [InlineData("ValuePrefix", new[] { "Prefix" }, "ValuePrefix")]
    [InlineData("Prefix1Prefix2", new[] { "Prefix1", "Prefix2" }, "Prefix2")]
    [InlineData("Prefix1Prefix2", new[] { "Pre", "Prefix1", "Prefix2" }, "fix1Prefix2")]
    [InlineData("Prefix1Prefix2Value", new[] { "Prefix1", "Prefix2" }, "Prefix2Value")]
    [InlineData("Prefix1Prefix2Value", new[] { "Prefix2", "Prefix1" }, "Prefix2Value")]
    [InlineData("ValuePrefix1Prefix2", new[] { "Prefix1", "Prefix2" }, "ValuePrefix1Prefix2")]
    [InlineData("ValuePrefix1Prefix2", new[] { "Prefix2", "Prefix1" }, "ValuePrefix1Prefix2")]
    public void TrimPrefixArray_ShouldBeExpected(string? input, string[] prefixes, string? expected) =>
        input.TrimPrefix(prefixes).Should().Be(expected);

    [Theory]
    [InlineData(null, new string[0], null)]
    [InlineData(null, new[] { "" }, null)]
    [InlineData(null, new[] { "Suffix" }, null)]
    [InlineData("", new string[0], "")]
    [InlineData("", new[] { "" }, "")]
    [InlineData("", new[] { "Suffix" }, "")]
    [InlineData("Value", new string[0], "Value")]
    [InlineData("Value", new[] { "" }, "Value")]
    [InlineData("Value", new[] { "Suffix" }, "Value")]
    [InlineData("SuffixSuffix", new[] { "Suffix" }, "Suffix")]
    [InlineData("SuffixValue", new[] { "Suffix" }, "SuffixValue")]
    [InlineData("ValueSuffix", new[] { "Suffix" }, "Value")]
    [InlineData("Suffix1Suffix2", new[] { "Suffix1", "Suffix2" }, "Suffix1")]
    [InlineData("Suffix1Suffix2", new[] { "fix2", "Suffix1", "Suffix2" }, "Suffix1Suf")]
    [InlineData("Suffix1Suffix2Value", new[] { "Suffix1", "Suffix2" }, "Suffix1Suffix2Value")]
    [InlineData("Suffix1Suffix2Value", new[] { "Suffix2", "Suffix1" }, "Suffix1Suffix2Value")]
    [InlineData("ValueSuffix1Suffix2", new[] { "Suffix1", "Suffix2" }, "ValueSuffix1")]
    [InlineData("ValueSuffix1Suffix2", new[] { "Suffix2", "Suffix1" }, "ValueSuffix1")]
    public void TrimSuffixArray_ShouldBeExpected(string? input, string[] suffixes, string? expected) =>
        input.TrimSuffix(suffixes).Should().Be(expected);

    [Theory]
    [InlineData(null, new string[0], null)]
    [InlineData(null, new[] { "" }, null)]
    [InlineData(null, new[] { "Prefix" }, null)]
    [InlineData("", new string[0], "")]
    [InlineData("", new[] { "" }, "")]
    [InlineData("", new[] { "Prefix" }, "")]
    [InlineData("Value", new string[0], "Value")]
    [InlineData("Value", new[] { "" }, "Value")]
    [InlineData("Value", new[] { "Prefix" }, "Value")]
    [InlineData("PrefixPrefix", new[] { "Prefix" }, "Prefix")]
    [InlineData("PrefixValue", new[] { "Prefix" }, "Value")]
    [InlineData("ValuePrefix", new[] { "Prefix" }, "ValuePrefix")]
    [InlineData("Prefix1Prefix2", new[] { "Prefix1", "Prefix2" }, "")]
    [InlineData("Prefix1Prefix2", new[] { "Pre", "Pref", "Prefix1", "Prefix2" }, "")]
    [InlineData("Prefix1Prefix2Value", new[] { "Prefix1", "Prefix2" }, "Value")]
    [InlineData("Prefix1Prefix2Value", new[] { "Prefix2", "Prefix1" }, "Value")]
    [InlineData("ValuePrefix1Prefix2", new[] { "Prefix1", "Prefix2" }, "ValuePrefix1Prefix2")]
    [InlineData("ValuePrefix1Prefix2", new[] { "Prefix2", "Prefix1" }, "ValuePrefix1Prefix2")]
    [InlineData("Prefix1Prefix1Value", new[] { "Prefix1", "Prefix2" }, "Prefix1Value")]
    public void TrimAllPrefixes_ShouldBeExpected(string? input, string[] prefixes, string? expected) =>
        input.TrimAllPrefixes(prefixes).Should().Be(expected);

    [Theory]
    [InlineData(null, new string[0], null)]
    [InlineData(null, new[] { "" }, null)]
    [InlineData(null, new[] { "Suffix" }, null)]
    [InlineData("", new string[0], "")]
    [InlineData("", new[] { "" }, "")]
    [InlineData("", new[] { "Suffix" }, "")]
    [InlineData("Value", new string[0], "Value")]
    [InlineData("Value", new[] { "" }, "Value")]
    [InlineData("Value", new[] { "Suffix" }, "Value")]
    [InlineData("SuffixSuffix", new[] { "Suffix" }, "Suffix")]
    [InlineData("SuffixValue", new[] { "Suffix" }, "SuffixValue")]
    [InlineData("ValueSuffix", new[] { "Suffix" }, "Value")]
    [InlineData("Suffix1Suffix2", new[] { "Suffix1", "Suffix2" }, "")]
    [InlineData("Suffix1Suffix2", new[] { "fix2", "Suffix1", "Suffix2", "x2" }, "")]
    [InlineData("Suffix1Suffix2Value", new[] { "Suffix1", "Suffix2" }, "Suffix1Suffix2Value")]
    [InlineData("Suffix1Suffix2Value", new[] { "Suffix2", "Suffix1" }, "Suffix1Suffix2Value")]
    [InlineData("ValueSuffix1Suffix2", new[] { "Suffix1", "Suffix2" }, "Value")]
    [InlineData("ValueSuffix1Suffix2", new[] { "Suffix2", "Suffix1" }, "Value")]
    [InlineData("ValueSuffix1Suffix1", new[] { "Suffix2", "Suffix1" }, "ValueSuffix1")]
    public void TrimAllSuffixes_ShouldBeExpected(string? input, string[] suffixes, string? expected) =>
        input.TrimAllSuffixes(suffixes).Should().Be(expected);

    [Theory]
    [InlineData(null, new[] { "Test" }, null)]
    [InlineData("", new[] { "Test" }, "")]
    [InlineData("Test", new[] { "Test" }, "")]
    [InlineData("ValueTest", new[] { "Test" }, "Value")]
    [InlineData("ValueTest", new[] { "Value", "Test" }, "")]
    public void TrimSuffixes_ShouldBeExpected(string? input, string[] suffixes, string? expected) =>
        input.TrimSuffix(suffixes).Should().Be(expected);

    [Theory]
    [InlineData(null, "Test", "Test")]
    [InlineData("", "Test", "Test")]
    [InlineData("Test", "Test", "Test")]
    [InlineData("Value", "Test", "TestValue")]
    [InlineData("Value", "", "Value")]
    public void EnsurePrefix_ShouldBeExpected(string? input, string prefix, string expected) =>
        input.EnsurePrefix(prefix).Should().Be(expected);

    [Theory]
    [InlineData(null, "Test", "Test")]
    [InlineData("", "Test", "Test")]
    [InlineData("Test", "Test", "Test")]
    [InlineData("Value", "Test", "ValueTest")]
    [InlineData("Value", "", "Value")]
    public void EnsureSuffix_ShouldBeExpected(string? input, string suffix, string expected) =>
        input.EnsureSuffix(suffix).Should().Be(expected);

    [Theory]
    [InlineData(null, null)]
    [InlineData("Test", "test")]
    [InlineData("TestCase", "test_case")]
    [InlineData("MyTestCase", "my_test_case")]
    public void ToSnakeCase_ShouldBeExpected(string? input, string? expected) =>
        input.ToSnakeCase().Should().Be(expected);

    [Theory]
    [InlineData(null, null)]
    [InlineData("Test", "test")]
    [InlineData("TestCase", "test-case")]
    [InlineData("MyTestCase", "my-test-case")]
    public void ToKebabCase_ShouldBeExpected(string? input, string? expected) =>
        input.ToKebabCase().Should().Be(expected);

    [Theory]
    [InlineData(null, null)]
    [InlineData("test", "Test")]
    [InlineData("testCase", "TestCase")]
    [InlineData("test_case", "TestCase")]
    public void ToPascalCase_ShouldBeExpected(string? input, string? expected) =>
        input.ToPascalCase().Should().Be(expected);

    [Theory]
    [InlineData("test-case", new[] { '-', '_' }, "TestCase")]
    [InlineData("test_case", new[] { '-', '_' }, "TestCase")]
    [InlineData("my-test_case", new[] { '-', '_' }, "MyTestCase")]
    public void ToPascalCaseWithSeparators_ShouldBeExpected(string? input, char[] separators, string? expected) =>
        input.ToPascalCase(separators).Should().Be(expected);

    [Theory]
    [InlineData(null, null)]
    [InlineData("Test", "test")]
    [InlineData("URLValue", "urlValue")]
    [InlineData("MyURLValue", "myURLValue")]
    public void ToCamelCase_ShouldBeExpected(string? input, string? expected) =>
        input.ToCamelCase().Should().Be(expected);

    [Theory]
    [InlineData(null, null)]
    [InlineData("Test", "test")]
    [InlineData("Tests", "test")]
    [InlineData("People", "person")]
    public void ToSingular_ShouldBeExpected(string? input, string? expected) =>
        input.ToSingular().Should().Be(expected);

    [Theory]
    [InlineData(null, null)]
    [InlineData("Test", "tests")]
    [InlineData("Person", "people")]
    [InlineData("People", "people")]
    public void ToPlural_ShouldBeExpected(string? input, string? expected) =>
        input.ToPlural().Should().Be(expected);
}