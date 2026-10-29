namespace SampleApps.Desired.Accounts;

/// <summary>
/// Account details (update request).
/// </summary>
/// <param name="Id">User Id.</param>
/// <param name="Login">User login.</param>
/// <param name="Name">User name.</param>
public record UpdateAccountRequest(Guid Id, string Login, string Name);