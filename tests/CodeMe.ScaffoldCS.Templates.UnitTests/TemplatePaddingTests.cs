using CodeMe.ScaffoldCS.Templates.Render;
using CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure;
using CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure.Models;

namespace CodeMe.ScaffoldCS.Templates.UnitTests;

using static SymbolsCS;

public class TemplatePaddingTests
{
    private static readonly Dto _testDto = new(
        "test_models",
        "test_dto",
        "Test details",
        [
            new Property("id", "BIGINT", "Identifier"),
            new Property("name", "VARCHAR(255)", "Dto name details"),
            new Property("created_at", "TIMESTAMPTZ", null, Default: "NOW()"),
            new Property("updated_at", "TIMESTAMPTZ", null, true)
        ]);

    [Fact]
    public void TestScenario_ShouldBeExpected()
    {
        // Arrange
        var dto = _testDto;
        var writer = CreateTemplate();
        using var _ = writer.BeginAmbientScope();

        // Act
        writer.Write(
            $"""
            {CENTER($" {dto.Name} ", '-', 50)}
            CREATE TABLE {dto.Namespace}.{dto.Name} (
                {dto.Properties.RenderPaddedCommaSeparated(RenderProperty)}
            );
            
            {t => RenderComments(t, dto)}
            """);

        // Assert
        // language=sql
        writer.ShouldBe(
            """
            --------------- test_dto ---------------
            CREATE TABLE test_models.test_dto (
                id         BIGINT       GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                name       VARCHAR(255) NOT NULL,
                created_at TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
                updated_at TIMESTAMPTZ  NULL
            );
            
            COMMENT ON TABLE  test_models.test_dto      IS 'Test details';
            COMMENT ON COLUMN test_models.test_dto.id   IS 'Identifier';
            COMMENT ON COLUMN test_models.test_dto.name IS 'Dto name details';
            """);
    }

    private static TemplateString RenderProperty(Property x) =>
        x.Name == "id"
            ? (TemplateString)$"{x.Name} {x.Type} GENERATED ALWAYS AS IDENTITY PRIMARY KEY"
            : $"{x.Name} {x.Type} {IIF(x.IsNullable, "NULL", "NOT NULL")}{IIF(x.Default, $" DEFAULT {x.Default}")}";

    private static void RenderComments(Template template, Dto dto)
    {
        using var _ = template.BeginPaddingScope("comments");

        if (Conditions.HasValue(dto.Comment))
        {
            template.Write($"COMMENT ON {"TABLE"} {$"{dto.Namespace}.{dto.Name}"} IS {$"'{dto.Comment}';"}");
        }

        var columnComments = dto.Properties
            .WhereHasValue(x => x.Comment)
            .RenderPadded(
                x => $"COMMENT ON {"COLUMN"} {$"{dto.Namespace}.{dto.Name}.{x.Name}"} IS {$"'{x.Comment}';"}",
                paddingGroupName: "comments");
        template.WriteLine().Write(columnComments);
    }

    private static Template CreateTemplate()
    {
        var template = new Template(new StringWriter());
        return template;
    }
}