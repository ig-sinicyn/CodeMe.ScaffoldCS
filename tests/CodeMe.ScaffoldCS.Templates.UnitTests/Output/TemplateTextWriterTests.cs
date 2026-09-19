using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.UnitTests.Infrastructure;

namespace CodeMe.ScaffoldCS.Templates.UnitTests.Output;

public class TemplateTextWriterTests
{
    [Fact]
    public void TestScenario_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ");

        // Act
        writer
            .Append(
                """
                
                Hello

                World
                """)
            .Append("\n ")
            .Append("\r\n");

        // Assert
        writer.ShouldBe(
            """
              //
              // Hello
              //
              // World
              //
            
            """);
    }

    [Fact]
    public void Empty_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Assert
        writer.ShouldBe("", beforeClose: "");
    }

    [Fact]
    public void Empty_WithIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ");

        // Assert
        writer.ShouldBe("", beforeClose: "");
    }

    [Fact]
    public void NormalText_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer.Append("Hello");

        // Assert
        writer.ShouldBe("Hello", beforeClose: "");
    }

    [Fact]
    public void NormalText_WithIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ");

        // Act
        writer.Append("Hello");

        // Assert
        writer.ShouldBe("  // Hello", beforeClose: "");
    }

    [Fact]
    public void NormalText_LeftPadding_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer
            .Append('\'')
            .Append("Hello", 8)
            .Append('\'');

        // Assert
        writer.ShouldBe("'   Hello'");
    }

    [Fact]
    public void NormalText_LeftPadding_WithIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ");

        // Act
        writer
            .Append('\'')
            .Append("Hello", 8)
            .Append('\'');

        // Assert
        writer.ShouldBe("  // '   Hello'");
    }

    [Fact]
    public void NormalText_RightPadding_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer
            .Append('\'')
            .Append("Hello", -8)
            .Append('\'');

        // Assert
        writer.ShouldBe("'Hello   '");
    }

    [Fact]
    public void NormalText_Multiline_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer.Append("Hello,\r\nworld!");

        // Assert
        writer.ShouldBe("Hello,\nworld!", beforeClose: "Hello,\n");
    }

    [Fact]
    public void NormalText_Multiline_WithIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf)
            .WithIndentation("  // ");

        // Act
        writer.Append("Hello,\r\nworld!");

        // Assert
        writer.ShouldBe("  // Hello,\n  // world!");
    }

    [Fact]
    public void NormalText_Multiline_LeftPadding_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer
            .Append('\'')
            .Append("A\r\nB", 8)
            .Append('\'');

        // Assert
        writer.ShouldBe("'    A\nB'");
    }

    [Fact]
    public void NormalText_Multiline_RightPadding_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer
            .Append('\'')
            .Append("A\r\nB", -8)
            .Append('\'');

        // Assert
        writer.ShouldBe("'A\nB    '");
    }

    [Fact]
    public void NormalText_MultilineLiteral_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer.Append(
            """
              Hello,

              world!
            """);

        // Assert
        writer.ShouldBe(
            """
              Hello,

              world!
            """,
            beforeClose:
            """
              Hello,
            
            
            """);
    }

    [Fact]
    public void NormalText_MultilineLiteral_WithIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ");

        // Act
        writer.Append(
            """
            Hello,

            world!

            """);

        // Assert
        writer.ShouldBe(
            """
              // Hello,
              //
              // world!
            
            """);
    }

    [Fact]
    public void NormalText_MultilineLiteral_WithNonEmptyIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ")
            .WithIndentationOnEmptyLines(false);

        // Act
        writer.Append(
            """
            Hello,

            world!

            """);

        // Assert
        writer.ShouldBe(
            """
              // Hello,
            
              // world!
            
            """);
    }

    [Fact]
    public void NormalText_MultilineLiteral_AsGeneric_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ");

        // Act
        writer.Append<string>(
            """
            Hello,

            world!

            """);

        // Assert
        writer.ShouldBe(
            """
              // Hello,
              //
              // world!
            
            """);
    }

    [Fact]
    public void NormalText_MultilineLiteral_AsGenericMemory_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ");

        // Act
        writer.Append<ReadOnlyMemory<char>>(
            """
            Hello,

            world!

            """.AsMemory());

        // Assert
        writer.ShouldBe(
            """
              // Hello,
              //
              // world!
            
            """);
    }

    [Fact]
    public void NormalText_MultilineLiteral_AsSpan_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ");

        // Act
        writer.Append(
            """
            Hello,

            world!

            """.AsSpan());

        // Assert
        writer.ShouldBe(
            """
              // Hello,
              //
              // world!
            
            """);
    }

    [Fact]
    public void NormalText_MultilineLiteral_AsMemory_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ");

        // Act
        writer.Append(
            """
            Hello,

            world!

            """.AsMemory());

        // Assert
        writer.ShouldBe(
            """
              // Hello,
              //
              // world!
            
            """);
    }

    [Fact]
    public void NormalText_MultilineWrites_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer
            .AppendLine()
            .Append("Hello, ")
            .AppendLine()
            .Append("  world!")
            .AppendLine();

        // Assert
        writer.ShouldBe(
            """

            Hello,
              world!

            """,
            beforeClose:
            """

            Hello,
              world!
            
            """);
    }

    [Fact]
    public void NormalText_MultilineWrites_WithIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("// ");

        // Act
        writer
            .AppendLine()
            .Append("Hello, ")
            .AppendLine()
            .Append("  world!")
            .AppendLine();

        // Assert
        writer.ShouldBe(
            """
            //
            // Hello,
            //   world!
            
            """,
            beforeClose:
            """
            //
            // Hello,
            //   world!
            
            """);
    }

    [Fact]
    public void NormalText_MultilineWrites_WithNonEmptyIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("// ")
            .WithIndentationOnEmptyLines(false);

        // Act
        writer
            .AppendLine()
            .Append("Hello, ")
            .AppendLine()
            .Append("  world!")
            .AppendLine();

        // Assert
        writer.ShouldBe(
            """
            
            // Hello,
            //   world!
            
            """,
            beforeClose:
            """
            
            // Hello,
            //   world!
            
            """);
    }

    [Fact]
    public void NormalText_WhitespaceAndMultiline_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer.Append("Hello, \r\n world!");

        // Assert
        writer.ShouldBe("Hello,\n world!");
    }

    [Fact]
    public void Value_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer.Append(1.2);

        // Assert
        writer.ShouldBe("1.2", beforeClose: "");
    }

    [Fact]
    public void Value_WithIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("// ");

        // Act
        writer.Append(1.2);

        // Assert
        writer.ShouldBe("// 1.2");
    }

    [Fact]
    public void Value_CustomCulture_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithCulture("ru-RU");

        // Act
        writer.Append(1.2);

        // Assert
        writer.ShouldBe("1,2");
    }

    [Fact]
    public void Value_CustomFormat_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer.Append(1.2, 0, "F4");

        // Assert
        writer.ShouldBe("1.2000");
    }

    [Fact]
    public void Value_LeftPadding_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer
            .Append('\'')
            .Append(1.2, 4, null)
            .Append('\'');

        // Assert
        writer.ShouldBe("' 1.2'");
    }

    [Fact]
    public void Value_RightPadding_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer
            .Append('\'')
            .Append(1.2, -4, null)
            .Append('\'');

        // Assert
        writer.ShouldBe("'1.2 '");
    }

    [Fact]
    public void Whitespace_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer.Append("  \t \v ");

        // Assert
        writer.ShouldBe("");
    }

    [Fact]
    public void Whitespace_WithIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("// ");

        // Act
        writer.Append("  \t \v ");

        // Assert
        writer.ShouldBe("//");
    }

    [Fact]
    public void Whitespace_NoTrim_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithTrimLineEnd(false);

        // Act
        writer.Append("  \t \v ");

        // Assert
        writer.ShouldBe("  \t \v ");
    }

    [Fact]
    public void NewLine_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer.Append("\n\r\n\r\r");

        // Assert
        var expected = string.Join("", Enumerable.Repeat(Environment.NewLine, 4));
        var expectedBeforeClose = string.Join("", Enumerable.Repeat(Environment.NewLine, 3));
        writer.ShouldBe(expected, beforeClose: expectedBeforeClose);
    }

    [Fact]
    public void NewLine_WithIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("// ");

        // Act
        writer.Append("\n\r\n\r\r");

        // Assert
        writer.ShouldBe(
            """
            //
            //
            //
            //
            
            """);
    }

    [Fact]
    public void NewLine_WithNonEmptyIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("// ")
            .WithIndentationOnEmptyLines(false);

        // Act
        writer.Append("\n \r\n \r \r");

        // Assert
        writer.ShouldBe(
            """
            
            //
            //
            //
            
            """);
    }

    [Fact]
    public void NewLine_WithNonEmptyIndentation_Whitespace_NoTrimLine_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ")
            .WithIndentationOnEmptyLines(false)
            .WithTrimLineEnd(false)
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer.Append("\n \n \n \n");

        // Assert
        writer.ShouldBe("\n  //  \n  //  \n  //  \n");
    }

    [Fact]
    public void NewLine_Whitespace_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer.Append("\r \n ");

        // Assert
        writer.ShouldBe("\n\n", beforeClose: "\n\n");
    }

    [Fact]
    public void NewLine_WhitespaceNoTrim_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf)
            .WithTrimLineEnd(false);

        // Act
        writer.Append("\r \n ");

        // Assert
        writer.ShouldBe("\n \n ", beforeClose: "\n \n");
    }

    [Fact]
    public void NewLine_NoNormalization_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNormalizeNewLines(false);

        // Act
        writer.Append("\n\r\n\r\r");

        // Assert
        writer.ShouldBe("\n\r\n\r\r", beforeClose: "\n\r\n\r");
    }

    [Fact]
    public void NewLine_NoNormalization_WithIndentation_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithIndentation("  // ")
            .WithNormalizeNewLines(false);

        // Act
        writer.Append("\n\r\n\r\r");

        // Assert
        writer.ShouldBe("  //\n  //\r\n  //\r  //\r");
    }

    [Fact]
    public void NewLine_Custom_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer.Append("\n\r\n\r\r");

        // Assert
        writer.ShouldBe("\n\n\n\n");
    }

    [Fact]
    public void SetCurrentLineHandling_Ignore_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer
            .SetCurrentLineHandling(CurrentLineHandlingMode.Ignore)
            .Append("Hello");

        // Assert
        writer.ShouldBe("", beforeClose: "");
    }

    [Fact]
    public void SetCurrentLineHandling_Ignore_AfterAppend_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer
            .Append("Hello")
            .SetCurrentLineHandling(CurrentLineHandlingMode.Ignore);

        // Assert
        writer.ShouldBe("", beforeClose: "");
    }

    [Fact]
    public void SetCurrentLineHandling_Ignore_Multiline_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer
            .SetCurrentLineHandling(CurrentLineHandlingMode.Ignore)
            .Append("Hello\nworld!");

        // Assert
        writer.ShouldBe("world!", beforeClose: "");
    }

    [Fact]
    public void SetCurrentLineHandling_Ignore_AfterAppend_Multiline_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer
            .Append("Hello\nworld!")
            .SetCurrentLineHandling(CurrentLineHandlingMode.Ignore);

        // Assert
        writer.ShouldBe("Hello\n", beforeClose: "Hello\n");
    }

    [Fact]
    public void SetCurrentLineHandling_IgnoreIfEmpty_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer
            .Append("A\n")
            .SetCurrentLineHandling(CurrentLineHandlingMode.IgnoreIfEmpty)
            .Append(" \t \v \r\n")
            .SetCurrentLineHandling(CurrentLineHandlingMode.IgnoreIfEmpty)
            .Append("\n")
            .Append("B\n");

        // Assert
        writer.ShouldBe("A\nB\n", beforeClose: "A\nB\n");
    }

    [Fact]
    public void SetCurrentLineHandling_IgnoreIfEmpty_NonEmpty_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer
            .Append("A\n")
            .SetCurrentLineHandling(CurrentLineHandlingMode.IgnoreIfEmpty)
            .Append("B \n")
            .Append("C\n");

        // Assert
        writer.ShouldBe("A\nB\nC\n");
    }

    [Fact]
    public void SetCurrentLineHandling_IgnoreIfEmpty_NoTrim_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf)
            .WithTrimLineEnd(false);

        // Act
        writer
            .Append("A\n")
            .SetCurrentLineHandling(CurrentLineHandlingMode.IgnoreIfEmpty)
            .Append(" \t \v \r\n")
            .Append("B\n");

        // Assert
        writer.ShouldBe("A\n \t \v \nB\n");
    }

    [Fact]
    public void SetCurrentLineHandling_IgnoreIfWhitespace_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf)
            .WithTrimLineEnd(false);

        // Act
        writer
            .Append("A\n")
            .SetCurrentLineHandling(CurrentLineHandlingMode.IgnoreIfWhiteSpace)
            .Append(" \t \v \r\n")
            .SetCurrentLineHandling(CurrentLineHandlingMode.IgnoreIfWhiteSpace)
            .Append("\n")
            .Append("B\n");

        // Assert
        writer.ShouldBe("A\nB\n", beforeClose: "A\nB\n");
    }

    [Fact]
    public void SetCurrentLineHandling_IgnoreIfWhitespace_NonEmpty_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer
            .Append("A\n")
            .SetCurrentLineHandling(CurrentLineHandlingMode.IgnoreIfWhiteSpace)
            .Append("B \n")
            .Append("C\n");

        // Assert
        writer.ShouldBe("A\nB\nC\n");
    }

    [Fact]
    public void ClearLine_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer
            .Append("A\n")
            .Append("B")
            .ClearLine()
            .Append("\n")
            .Append("C\n");

        // Assert
        writer.ShouldBe("A\n\nC\n");
    }

    [Fact]
    public void ClearLine_AfterLineEnd_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter()
            .WithNewLineFormat(NewLineFormat.Lf);

        // Act
        writer
            .Append("A\n")
            .Append("B\n")
            .ClearLine()
            .Append("C\n");

        // Assert
        writer.ShouldBe("A\nB\nC\n");
    }

    [Fact]
    public void MultipleClose_ShouldBeExpected()
    {
        // Arrange
        var writer = CreateTextWriter();

        // Act
        writer.Append("Hello");
        writer.Close();
        writer.Close();

        // Assert
        writer.ShouldBe("Hello");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Append_AfterClose_ShouldThrow(bool leaveOpen)
    {
        // Arrange
        var writer = new TemplateTextWriter(new StringWriter(), leaveOpen: leaveOpen);

        // Act
        writer.Append("Hello");
        writer.Close();
        Assert.Throws<ObjectDisposedException>(() => writer.Append(", world!"));

        // Assert
        writer.ShouldBe("Hello");
    }

    private static TemplateTextWriter CreateTextWriter() => new(new StringWriter());
}