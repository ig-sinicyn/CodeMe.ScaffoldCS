namespace CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure.Models;

public record Property(string Name, string Type, string? Comment, bool IsNullable = false, string? Default = null);