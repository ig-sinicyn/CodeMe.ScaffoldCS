namespace SampleApps.Desired.Contracts;

/// <summary>
/// Account details.
/// </summary>
/// <param name="Id">User Id.</param>
/// <param name="Login">User login.</param>
/// <param name="Name">User name.</param>
public record CreateAccountRequest(Guid Id, string Login, string Name);