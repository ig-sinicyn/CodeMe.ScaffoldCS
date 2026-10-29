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
    [InlineData(null, "", "")]
    [InlineData(null, "Prefix", "Prefix")]
    [InlineData("", "", "")]
    [InlineData("", "Prefix", "Prefix")]
    [InlineData("Value", "", "Value")]
    [InlineData("Value", "Prefix", "PrefixValue")]
    public void EnsurePrefix_ShouldBeExpected(string? input, string prefix, string expected) =>
        input.EnsurePrefix(prefix).Should().Be(expected);

    [Theory]
    [InlineData(null, "", "")]
    [InlineData(null, "Suffix", "Suffix")]
    [InlineData("", "", "")]
    [InlineData("", "Suffix", "Suffix")]
    [InlineData("Value", "", "Value")]
    [InlineData("Value", "Suffix", "ValueSuffix")]
    public void EnsureSuffix_ShouldBeExpected(string? input, string suffix, string expected) =>
        input.EnsureSuffix(suffix).Should().Be(expected);
}