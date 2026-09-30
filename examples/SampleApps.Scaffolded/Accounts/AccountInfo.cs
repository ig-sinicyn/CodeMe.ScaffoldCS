namespace SampleApps.Desired.Contracts;

/// <summary>
/// Account details.
/// </summary>
/// <param name="Id">User Id.</param>
/// <param name="Login">User login.</param>
/// <param name="Name">User name.</param>
/// <param name="CreatedAt">Creation date.</param>
/// <param name="UpdatedAt">Update date.</param>
public record AccountInfo(Guid Id, string Login, string Name, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);