// ReSharper disable UnusedMember.Global

#pragma warning disable IDE0130
namespace Tests;
#pragma warning restore IDE0130

internal class EmptyDto
{
}

/// <summary>Here be comment.</summary>
public class EmptyDtoWithComment
{
}

/// <summary>Dto comment.</summary>
public class DtoWithIntProperty
{
    /// <summary>
    /// Value comment.
    /// </summary>
    public int Value { get; set; }
}

public class DtoWithNullableGuidProperty
{
    public Guid? Value { get; set; }
}

public class DtoWithNullableStringProperty
{
    public string? Value => null;
}