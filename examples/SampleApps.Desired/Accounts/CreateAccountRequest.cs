namespace SampleApps.Desired.Accounts;

/// <summary>
/// Account details (create request).
/// </summary>
/// <param name="Login">User login.</param>
/// <param name="Name">User name.</param>
public record CreateAccountRequest(string Login, string Name);