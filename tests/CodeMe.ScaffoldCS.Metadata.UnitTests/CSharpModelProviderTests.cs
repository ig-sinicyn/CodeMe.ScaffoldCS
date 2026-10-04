using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Metadata.CodeModel;
using CodeMe.ScaffoldCS.Metadata.UnitTests.Infrastructure;
using T = CodeMe.ScaffoldCS.Metadata.CodeModel.WellKnownTypes;
using static CodeMe.ScaffoldCS.Metadata.CodeModel.TypeInfo;

namespace CodeMe.ScaffoldCS.Metadata.UnitTests;

[Collection(nameof(CSharpMetadataCollection))]
public sealed class CSharpModelProviderTests
{
    private readonly ICSharpModelProvider _modelProvider;

    public CSharpModelProviderTests(CSharpMetadataFixture fixture)
    {
        _modelProvider = fixture.ModelProvider;
    }

    private DtoModel GetModel([CallerMemberName] string? caller = null)
    {
        var dtoName = caller!.Split('_', 2)[0];
        return _modelProvider.GetDtoByFile(WellKnownTestClasses.BasicDtos, dtoName);
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
    public void DtoWithInt32Property_ShouldBeExpected() => Assert(
        new DtoModel(
            "DtoWithInt32Property",
            "Tests",
            "Dto comment.",
            new DtoField("Value", T.Int32, "Value comment.")
            {
                DefaultValue = 12
            }));

    [Fact]
    public void DtoWithInitializedGuidProperty_ShouldBeExpected() => Assert(
        new DtoModel(
            "DtoWithInitializedGuidProperty",
            "Tests",
            new DtoField("Value", T.Guid)
            {
                DefaultValue = new DtoFieldInitializer("Guid.NewGuid()")
            }));

    [Fact]
    public void IInterfaceWithNullableGuidProperty_ShouldBeExpected() => Assert(
        new DtoModel(
            "IInterfaceWithNullableGuidProperty",
            "Tests",
            new DtoField("Value", T.Guid.ToNullable())));

    [Fact]
    public void StructWithNullableStringProperty_ShouldBeExpected() => Assert(
        new DtoModel(
            "StructWithNullableStringProperty",
            "Tests",
            new DtoField("Value", T.String.ToNullable())));

    [Fact]
    public void RecordWithEmptyDtoProperty_ShouldBeExpected() => Assert(
        new DtoModel(
            "RecordWithEmptyDtoProperty",
            "Tests",
            new DtoField("Value", Class("EmptyDto", "Tests"))));

    [Fact]
    public void RecordWithInitializedStringProperty_ShouldBeExpected() => Assert(
        new DtoModel(
            "RecordWithInitializedStringProperty",
            "Tests",
            new DtoField("Value", T.String)
            {
                DefaultValue = "Hello there!"
            }));

    [Fact]
    public void Int32Enum_ShouldBeExpected() => Assert(
        new DtoModel(
            new TypeName("Int32Enum", "Tests"),
            TypeRole.Enum,
            "Enum comment.",
            [
                new DtoField("Normal", T.Int32, "Enum field comment.")
                {
                    DefaultValue = 0
                },
                new DtoField("Value1", T.Int32)
                {
                    DefaultValue = 1
                },
                new DtoField("Value2", T.Int32)
                {
                    DefaultValue = 2
                }
            ]));

    [Fact]
    public void Int64Enum_ShouldBeExpected() => Assert(
        new DtoModel(
            new TypeName("Int64Enum", "Tests"),
            TypeRole.Enum,
            null,
            [
                new DtoField("Normal", T.Int64)
                {
                    DefaultValue = 0
                },
                new DtoField("Value2", T.Int64)
                {
                    DefaultValue = 2
                },
                new DtoField("Value3", T.Int64)
                {
                    DefaultValue = 3
                }
            ]));
}