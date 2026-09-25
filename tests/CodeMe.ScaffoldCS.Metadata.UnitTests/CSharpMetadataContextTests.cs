using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Metadata.CodeModel;
using CodeMe.ScaffoldCS.Metadata.CSharp;
using CodeMe.ScaffoldCS.Metadata.UnitTests.Infrastructure;
using T = CodeMe.ScaffoldCS.Metadata.CodeModel.WellKnownTypes;

namespace CodeMe.ScaffoldCS.Metadata.UnitTests;

[Collection(nameof(CSharpMetadataCollection))]
public sealed class CSharpMetadataContextTests
{
    private readonly ICSharpMetadataContext _metadataContext;

    public CSharpMetadataContextTests(CSharpMetadataFixture fixture)
    {
        _metadataContext = fixture.MetadataContext;
    }

    private DtoModel GetModel([CallerMemberName] string? caller = null)
    {
        var dtoName = caller!.Split('_', 2)[0];
        return _metadataContext.GetDto(WellKnownTestClasses.BasicDtos, dtoName);
    }

    private void Assert(DtoModel expected, [CallerMemberName] string? caller = null) =>
        GetModel(caller).Should().BeEquivalentTo(expected);

    [Fact]
    public void EmptyDto_ShouldBeExpected() => Assert(
        new DtoModel("EmptyDto", "Tests", comment: null));

    [Fact]
    public void EmptyDtoWithComment_ShouldBeExpected() => Assert(
        new DtoModel("EmptyDtoWithComment", "Tests", "Here be comment."));

    [Fact]
    public void DtoWithIntProperty_ShouldBeExpected() => Assert(
        new DtoModel(
            "DtoWithIntProperty",
            "Tests",
            "Dto comment.",
            new DtoField("Value", T.Int32, "Value comment.")));

    [Fact]
    public void DtoWithNullableGuidProperty_ShouldBeExpected() => Assert(
        new DtoModel(
            "DtoWithNullableGuidProperty",
            "Tests",
            new DtoField("Value", T.Guid.ToNullable())));

    [Fact]
    public void DtoWithNullableStringProperty_ShouldBeExpected() => Assert(
        new DtoModel(
            "DtoWithNullableStringProperty",
            "Tests",
            new DtoField("Value", T.String.ToNullable())));
}