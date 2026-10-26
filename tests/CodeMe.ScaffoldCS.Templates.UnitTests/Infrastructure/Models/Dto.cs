namespace CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure.Models;

public record Dto(string Namespace, string Name, string Comment, IReadOnlyCollection<Property> Properties);