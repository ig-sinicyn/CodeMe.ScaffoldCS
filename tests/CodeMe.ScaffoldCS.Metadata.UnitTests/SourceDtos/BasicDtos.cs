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
public class DtoWithIntProperty
{
    /// <summary>
    /// Value comment.
    /// </summary>
    public int Value { get; set; } = 12;
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
    Normal,

    Value1,

    Value2
}