namespace CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure.Models;

public record Class(string Namespace, string Name, string Comment, IReadOnlyCollection<Property> Properties);