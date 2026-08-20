using CodeMe.ScaffoldCS.Templates.Render;
using CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure;
using CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure.Models;

namespace CodeMe.ScaffoldCS.Templates.UnitTests;

using static SymbolsCS;

public class TemplateTests
{
    private static readonly Class _testDto = new(
        "TestDto",
        "Test details",
        [
            new Property("Id", "long", "Identifier"),
            new Property("Name", "string", "Dto name\ndetails"),
            new Property("CreatedAt", "DateTimeOffset", null)
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
            $$"""
            using System;
            
            namespace TestModels;
            
            {{IF(dto.Comment)}}/// <summary>
            /// {{dto.Comment}}.
            /// </summary>{{ENDIF}}
            public class {{dto.Name}}
            {
                {{dto.Properties.RenderEmptyLineSeparated(RenderProperty)}}
            }
            """);

        // Assert
        writer.ShouldBe(
            """
            using System;
            
            namespace TestModels;

            /// <summary>
            /// Test details.
            /// </summary>
            public class TestDto
            {
                /// <summary>
                /// Identifier.
                /// </summary>
                public long Id { get; set; }

                /// <summary>
                /// Dto name
                /// details.
                /// </summary>
                public string Name { get; set; }

                public DateTimeOffset CreatedAt { get; set; }
            }
            """);
    }

    [Fact]
    public void TestScenario_EmptyProperties_ShouldBeExpected()
    {
        // Arrange
        var dto = _testDto with
        {
            Properties = []
        };
        var writer = CreateTemplate();
        using var _ = writer.BeginAmbientScope();

        // Act
        writer.Write(
            $$"""
            using System;
            
            namespace TestModels;
            
            {{IF(dto.Comment)}}/// <summary>
            /// {{dto.Comment}}.
            /// </summary>{{ENDIF}}
            public class {{dto.Name}}
            {
                {{dto.Properties.RenderEmptyLineSeparated(RenderProperty)}}
            }
            """);

        // Assert
        writer.ShouldBe(
            """
            using System;
            
            namespace TestModels;

            /// <summary>
            /// Test details.
            /// </summary>
            public class TestDto
            {
            }
            """);
    }

    [Fact]
    public void TestScenario_IfComment_ShouldBeExpected()
    {
        // Arrange
        var dto = _testDto with
        {
            Comment = "",
            Properties = []
        };
        var writer = CreateTemplate();
        using var _ = writer.BeginAmbientScope();

        // Act
        writer.Write(
            $$"""
            using System;
            
            namespace TestModels;
            
            {{IF(dto.Comment)}}/// <summary>
            /// {{dto.Comment}}.
            /// </summary>{{ENDIF}}
            public class {{dto.Name}}
            {
                {{dto.Properties.RenderEmptyLineSeparated(RenderProperty)}}
            }
            """);

        // Assert
        writer.ShouldBe(
            """
            using System;
            
            namespace TestModels;

            public class TestDto
            {
            }
            """);
    }

    [Fact]
    public void TestScenario_IfTrue_ShouldBeExpected()
    {
        // Arrange
        var dto = _testDto with
        {
            Properties = []
        };
        var writer = CreateTemplate();
        using var _ = writer.BeginAmbientScope();

        // Act
        writer.Write(
            $$"""
            using System;
            
            namespace TestModels;
            
            {{IF(dto.Comment)}}/// <summary>
            /// {{dto.Comment}}.
            /// </summary>{{ENDIF}}
            public class {{dto.Name}}
            {
                {{IF(dto.Properties)}}
                #region {{dto.Name}} properties
                {{dto.Properties.RenderEmptyLineSeparated(RenderProperty)}}
                #endregion
                {{ENDIF}}
            }
            """);

        // Assert
        writer.ShouldBe(
            """
            using System;

            namespace TestModels;

            /// <summary>
            /// Test details.
            /// </summary>
            public class TestDto
            {
            }
            """);
    }

    [Fact]
    public void TestScenario_IfFalse_ShouldBeExpected()
    {
        // Arrange
        var dto = _testDto;
        var writer = CreateTemplate();
        using var _ = writer.BeginAmbientScope();

        // Act
        writer.Write(
            $$"""
            using System;
            
            namespace TestModels;
            
            {{IF(dto.Comment)}}/// <summary>
            /// {{dto.Comment}}.
            /// </summary>{{ENDIF}}
            public class {{dto.Name}}
            {
                {{IF(dto.Properties)}}
                #region {{dto.Name}} properties
                {{dto.Properties.RenderEmptyLineSeparated(RenderProperty)}}
                #endregion
                {{ENDIF}}
            }
            """);

        // Assert
        writer.ShouldBe(
            """
            using System;

            namespace TestModels;

            /// <summary>
            /// Test details.
            /// </summary>
            public class TestDto
            {
                #region TestDto properties
                /// <summary>
                /// Identifier.
                /// </summary>
                public long Id { get; set; }

                /// <summary>
                /// Dto name
                /// details.
                /// </summary>
                public string Name { get; set; }

                public DateTimeOffset CreatedAt { get; set; }
                #endregion
            }
            """);
    }

    [Fact]
    public void Iif_True_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTemplate();
        using var _ = writer.BeginAmbientScope();

        // Act
        writer.Write(
            $"""
            True={IIF(1, 1)}.
            False={IIF(0, 1)}.
            True={IIF(1, 1, 2)}.
            False={IIF(0, 1, 2)}.
            """);

        // Assert
        writer.ShouldBe(
            """
            True=1.
            False=.
            True=1.
            False=2.
            """);
    }

    [Fact]
    public void Iif_True_Template_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTemplate();
        using var _ = writer.BeginAmbientScope();

        // Act
        writer.Write(
            $"""
            True={IIF(1, $"Hi, '{_testDto.Name}'")}!
            False={IIF(0, $"Hi, '{_testDto.Name}'")}!
            True={IIF(1, $"Hi, '{_testDto.Name}'", RL)}!
            False={IIF(0, $"Hi, '{_testDto.Name}'", RL)}!
            """);

        // Assert
        writer.ShouldBe(
            """
            True=Hi, 'TestDto'!
            False=!
            True=Hi, 'TestDto'!
            
            """);
    }

    [Fact]
    public void MultipleRemoveLine_ShouldBeExpected()
    {
        // Arrange
        var dto = _testDto;
        var writer = CreateTemplate();
        using var _ = writer.BeginAmbientScope();
        // Act
        writer.Write(
            $"""
            {RL}
            {RL}
            {RL}
            """);

        // Assert
        writer.ShouldBe(
            """
            
            """);
    }

    private static TemplatePart RenderProperty(Property property) =>
        $$"""
        {{IF(property.Comment)}}
        /// <summary>
        /// {{property.Comment}}.
        /// </summary>
        {{ENDIF}}
        public {{property.Type}} {{property.Name}} { get; set; }
        """;

    private static Template CreateTemplate()
    {
        var template = new Template(new StringWriter());
        return template;
    }
}