// ReSharper disable UnusedMember.Global

#pragma warning disable IDE0130
namespace Tests;
#pragma warning restore IDE0130

public class EmptyDto
{
}

/// <summary>Here be comment.</summary>
internal class EmptyDtoWithComment
{
}

/// <summary>
/// Dto comment.
/// </summary>
public class DtoWithInt32Property
{
    /// <summary>
    /// Value comment.
    /// </summary>
    public int Value { get; set; } = 12;
}

public class DtoWithInitializedGuidProperty
{
    public Guid Value { get; set; } = Guid.NewGuid();
}

public interface IInterfaceWithNullableGuidProperty
{
    public Guid? Value { get; set; }
}

public struct StructWithNullableStringProperty
{
    public string? Value => null;
}

public record RecordWithEmptyDtoProperty(EmptyDto Value);

public record RecordWithInitializedStringProperty(string Value = "Hello there!");

/// <summary>
/// Enum comment.
/// </summary>
public enum Int32Enum
{
    /// <summary>
    /// Enum field comment.
    /// </summary>
    Normal,

    Value1,

    Value2
}

public enum Int64Enum : long
{
    Normal = 0,

    Value2 = 2,

    Value3
}