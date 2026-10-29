using CodeMe.ScaffoldCS.Templates.Render;
using CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure;
using CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure.Models;

namespace CodeMe.ScaffoldCS.Templates.UnitTests;

using static SymbolsCS;

public class TemplateTests
{
    private static readonly Dto _testDto = new(
        "TestModels",
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
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $$"""
            {{
                COMMENT(
                    "This template should render DTO as C# class")
            }}
            using System;
            
            namespace {{dto.Namespace}};

            {{IF(dto.Comment)}}
            /// <summary>
            /// {{dto.Comment}}.
            /// </summary>
            {{ENDIF}}
            public class {{dto.Name}}
            {
                {{dto.Properties.RenderEmptyLineSeparated(RenderProperty)}}
            }
            """);

        // Assert
        // language=c#
        template.ShouldBe(
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
    public void Write_ShouldBeExpected()
    {
        // Arrange
        var name = "user";
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write($"Hello, {name}!");

        // Assert
        template.ShouldBe("Hello, user!");
    }

    [Fact]
    public void TestScenario_EmptyProperties_ShouldBeExpected()
    {
        // Arrange
        var dto = _testDto with
        {
            Properties = []
        };
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $$"""
            using System;
            
            namespace {{dto.Namespace}};
            
            {{IF(dto.Comment)}}/// <summary>
            /// {{dto.Comment}}.
            /// </summary>{{ENDIF}}
            public class {{dto.Name}}
            {
                {{dto.Properties.RenderEmptyLineSeparated(RenderProperty)}}
            }
            """);

        // Assert
        // language=c#
        template.ShouldBe(
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
    public void TestScenario_EmptyProperties2_ShouldBeExpected()
    {
        // Arrange
        var dto = _testDto with
        {
            Properties = []
        };
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $$"""
            {
                {{dto.Properties.RenderEmptyLineSeparated(RenderProperty)}}
            }
            """);

        // Assert
        // language=c#
        template.ShouldBe(
            """
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
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $$"""
            using System;
            
            namespace {{dto.Namespace}};
            
            {{IF(dto.Comment)}}/// <summary>
            /// {{dto.Comment}}.
            /// </summary>{{ENDIF}}
            public class {{dto.Name}}
            {
                {{dto.Properties.RenderEmptyLineSeparated(RenderProperty)}}
            }
            """);

        // Assert
        // language=c#
        template.ShouldBe(
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
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $$"""
            using System;
            
            namespace {{dto.Namespace}};
            
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
        // language=c#
        template.ShouldBe(
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
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
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
        // language=c#
        template.ShouldBe(
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
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $"""
            True={IIF(1, 1)}.
            False={IIF(0, 1)}.
            True={IIF(1, 1, 2)}.
            False={IIF(0, 1, 2)}.
            """);

        // Assert
        template.ShouldBe(
            """
            True=1.
            False=.
            True=1.
            False=2.
            """);
    }

    [Fact]
    public void ClearLine_ShouldBeExpected()
    {
        // Arrange
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $"""
            111{CL}222
            333{CL}
            {CL}444
            """);

        // Assert
        template.ShouldBe(
            """
            222
            
            444
            """);
    }

    [Fact]
    public void Iif_True_Template_ShouldBeExpected()
    {
        // Arrange
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $"""
            True={IIF(1, $"Hi, '{_testDto.Name}'")}!
            False={IIF(0, $"Hi, '{_testDto.Name}'")}!
            True={IIF(1, $"Hi, '{_testDto.Name}'", RL)}!
            False={IIF(0, $"Hi, '{_testDto.Name}'", RL)}!
            """);

        // Assert
        template.ShouldBe(
            """
            True=Hi, 'TestDto'!
            False=!
            True=Hi, 'TestDto'!
            
            """);
    }

    [Fact]
    public void RemoveLine_ShouldBeExpected()
    {
        // Arrange
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $"""
            {RL}
            """);

        // Assert
        template.ShouldBe("");
    }

    [Fact]
    public void MultipleRemoveLine_ShouldBeExpected()
    {
        // Arrange
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $"""
            {RL}
            {RL}
            {RL}
            """);

        // Assert
        template.ShouldBe("");
    }

    [Fact]
    public void Collection_ShouldBeExpected()
    {
        // Arrange
        var data = new[] { 1, 2, 3 };
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();

        // Act
        template.Write(
            $"""
            * {data.Render()}           
            """);

        // Assert
        template.ShouldBe(
            """
            * 1
            * 2
            * 3
            """);
    }

    [Fact]
    public void CollectionEmpty_ShouldBeExpected()
    {
        // Arrange
        var data = Array.Empty<int>();
        var template = CreateTemplate();
        using var _ = template.BeginAmbientScope();
        var collectionOptions = CollectionRenderOptions.Multiline with
        {
            EmptyValue = RL
        };

        // Act
        template.Write(
            $"""
            * {data.Render(collectionOptions)}           
            """);

        // Assert
        template.ShouldBe("");
    }

    private static TemplateString RenderProperty(Property property) =>
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