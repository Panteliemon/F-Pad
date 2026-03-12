using System;
using System.Collections.Generic;
using FPad;
using Xunit;

namespace Tests.FPad;

public class StringUtilsTests_IterateOverSplitByLines
{
    [Fact]
    public void IterateOverSplitByLines_EmptyString_ReturnsOneLine()
    {
        // Arrange
        string text = "";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Single(lines);
        Assert.Equal("", lines[0].line);
        Assert.Equal(0, lines[0].lineIndex);
        Assert.Equal(0, lines[0].lineStartPosition);
        Assert.Equal(0, lines[0].nextLineStartPosition);
        Assert.True(lines[0].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_SingleLineNoNewline_ReturnsOneLine()
    {
        // Arrange
        string text = "hello world";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Single(lines);
        Assert.Equal("hello world", lines[0].line);
        Assert.Equal(0, lines[0].lineIndex);
        Assert.Equal(0, lines[0].lineStartPosition);
        Assert.Equal(11, lines[0].nextLineStartPosition);
        Assert.True(lines[0].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_TwoLinesWithLF_ReturnsTwoLines()
    {
        // Arrange
        string text = "line1\nline2";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(2, lines.Count);
        Assert.Equal("line1", lines[0].line);
        Assert.Equal(0, lines[0].lineIndex);
        Assert.Equal(0, lines[0].lineStartPosition);
        Assert.Equal(6, lines[0].nextLineStartPosition);
        Assert.False(lines[0].isLastLine);

        Assert.Equal("line2", lines[1].line);
        Assert.Equal(1, lines[1].lineIndex);
        Assert.Equal(6, lines[1].lineStartPosition);
        Assert.Equal(11, lines[1].nextLineStartPosition);
        Assert.True(lines[1].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_TwoLinesWithCR_ReturnsTwoLines()
    {
        // Arrange
        string text = "line1\rline2";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(2, lines.Count);
        Assert.Equal("line1", lines[0].line);
        Assert.Equal(0, lines[0].lineIndex);
        Assert.Equal(0, lines[0].lineStartPosition);
        Assert.Equal(6, lines[0].nextLineStartPosition);
        Assert.False(lines[0].isLastLine);

        Assert.Equal("line2", lines[1].line);
        Assert.Equal(1, lines[1].lineIndex);
        Assert.Equal(6, lines[1].lineStartPosition);
        Assert.Equal(11, lines[1].nextLineStartPosition);
        Assert.True(lines[1].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_TwoLinesWithCRLF_ReturnsTwoLines()
    {
        // Arrange
        string text = "line1\r\nline2";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(2, lines.Count);
        Assert.Equal("line1", lines[0].line);
        Assert.Equal(0, lines[0].lineIndex);
        Assert.Equal(0, lines[0].lineStartPosition);
        Assert.Equal(7, lines[0].nextLineStartPosition);
        Assert.False(lines[0].isLastLine);

        Assert.Equal("line2", lines[1].line);
        Assert.Equal(1, lines[1].lineIndex);
        Assert.Equal(7, lines[1].lineStartPosition);
        Assert.Equal(12, lines[1].nextLineStartPosition);
        Assert.True(lines[1].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_ThreeLinesWithMixedLineEndings_ReturnsThreeLines()
    {
        // Arrange
        string text = "line1\nline2\r\nline3";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(3, lines.Count);
        Assert.Equal("line1", lines[0].line);
        Assert.Equal(0, lines[0].lineIndex);
        Assert.Equal(0, lines[0].lineStartPosition);
        Assert.Equal(6, lines[0].nextLineStartPosition);
        Assert.False(lines[0].isLastLine);

        Assert.Equal("line2", lines[1].line);
        Assert.Equal(1, lines[1].lineIndex);
        Assert.Equal(6, lines[1].lineStartPosition);
        Assert.Equal(13, lines[1].nextLineStartPosition);
        Assert.False(lines[1].isLastLine);

        Assert.Equal("line3", lines[2].line);
        Assert.Equal(2, lines[2].lineIndex);
        Assert.Equal(13, lines[2].lineStartPosition);
        Assert.Equal(18, lines[2].nextLineStartPosition);
        Assert.True(lines[2].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_ThreeLinesWithMixedLineEndings_ReturnsThreeLines_R()
    {
        // Arrange
        string text = "line1\rline2\r\nline3";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(3, lines.Count);
        Assert.Equal("line1", lines[0].line);
        Assert.Equal(0, lines[0].lineIndex);
        Assert.Equal(0, lines[0].lineStartPosition);
        Assert.Equal(6, lines[0].nextLineStartPosition);
        Assert.False(lines[0].isLastLine);

        Assert.Equal("line2", lines[1].line);
        Assert.Equal(1, lines[1].lineIndex);
        Assert.Equal(6, lines[1].lineStartPosition);
        Assert.Equal(13, lines[1].nextLineStartPosition);
        Assert.False(lines[1].isLastLine);

        Assert.Equal("line3", lines[2].line);
        Assert.Equal(2, lines[2].lineIndex);
        Assert.Equal(13, lines[2].lineStartPosition);
        Assert.Equal(18, lines[2].nextLineStartPosition);
        Assert.True(lines[2].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_EmptyLines_ReturnsAllLines()
    {
        // Arrange
        string text = "\n\n";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(3, lines.Count);
        Assert.Equal("", lines[0].line);
        Assert.Equal(0, lines[0].lineIndex);
        Assert.False(lines[0].isLastLine);

        Assert.Equal("", lines[1].line);
        Assert.Equal(1, lines[1].lineIndex);
        Assert.False(lines[1].isLastLine);

        Assert.Equal("", lines[2].line);
        Assert.Equal(2, lines[2].lineIndex);
        Assert.True(lines[2].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_EmptyLines_ReturnsAllLines_R()
    {
        // Arrange
        string text = "\r\r";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(3, lines.Count);
        Assert.Equal("", lines[0].line);
        Assert.Equal(0, lines[0].lineIndex);
        Assert.False(lines[0].isLastLine);

        Assert.Equal("", lines[1].line);
        Assert.Equal(1, lines[1].lineIndex);
        Assert.False(lines[1].isLastLine);

        Assert.Equal("", lines[2].line);
        Assert.Equal(2, lines[2].lineIndex);
        Assert.True(lines[2].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_EmptyLines_ReturnsAllLines_RN()
    {
        // Arrange
        string text = "\r\n\r\n";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(3, lines.Count);
        Assert.Equal("", lines[0].line);
        Assert.Equal(0, lines[0].lineIndex);
        Assert.False(lines[0].isLastLine);

        Assert.Equal("", lines[1].line);
        Assert.Equal(1, lines[1].lineIndex);
        Assert.False(lines[1].isLastLine);

        Assert.Equal("", lines[2].line);
        Assert.Equal(2, lines[2].lineIndex);
        Assert.True(lines[2].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_EndsWithNewline_ReturnsEmptyLastLine()
    {
        // Arrange
        string text = "line1\nline2\n";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(3, lines.Count);
        Assert.Equal("line1", lines[0].line);
        Assert.Equal("line2", lines[1].line);
        Assert.Equal("", lines[2].line);
        Assert.True(lines[2].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_EndsWithCR_ReturnsEmptyLastLine()
    {
        // Arrange
        string text = "line1\rline2\r";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(3, lines.Count);
        Assert.Equal("line1", lines[0].line);
        Assert.Equal("line2", lines[1].line);
        Assert.Equal("", lines[2].line);
        Assert.True(lines[2].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_EndsWithCRLF_ReturnsEmptyLastLine()
    {
        // Arrange
        string text = "line1\r\nline2\r\n";
        List<(string line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine)> lines = new();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, lineStartPosition, nextLineStartPosition, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(3, lines.Count);
        Assert.Equal("line1", lines[0].line);
        Assert.Equal("line2", lines[1].line);
        Assert.Equal("", lines[2].line);
        Assert.True(lines[2].isLastLine);
    }

    [Fact]
    public void IterateOverSplitByLines_CallbackReturnsFalse_StopsIteration()
    {
        // Arrange
        string text = "line1\nline2\nline3";
        var lines = new List<string>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add(line.ToString());
            return lineIndex < 1; // Stop after second line
        });

        // Assert
        Assert.Equal(2, lines.Count);
        Assert.Equal("line1", lines[0]);
        Assert.Equal("line2", lines[1]);
    }

    [Fact]
    public void IterateOverSplitByLines_OnlyNewlines_ReturnsEmptyLines()
    {
        // Arrange
        string text = "\n\n\n";
        var lines = new List<(string line, int lineIndex)>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex));
            return true;
        });

        // Assert
        Assert.Equal(4, lines.Count);
        for (int i = 0; i < 4; i++)
        {
            Assert.Equal("", lines[i].line);
            Assert.Equal(i, lines[i].lineIndex);
        }
    }

    [Fact]
    public void IterateOverSplitByLines_OnlyCRLF_ReturnsEmptyLines()
    {
        // Arrange
        string text = "\r\n\r\n\r\n";
        var lines = new List<(string line, int lineIndex)>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex));
            return true;
        });

        // Assert
        Assert.Equal(4, lines.Count);
        for (int i = 0; i < 4; i++)
        {
            Assert.Equal("", lines[i].line);
            Assert.Equal(i, lines[i].lineIndex);
        }
    }

    [Fact]
    public void IterateOverSplitByLines_OnlyCR_ReturnsEmptyLines()
    {
        // Arrange
        string text = "\r\r\r";
        var lines = new List<(string line, int lineIndex)>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex));
            return true;
        });

        // Assert
        Assert.Equal(4, lines.Count);
        for (int i = 0; i < 4; i++)
        {
            Assert.Equal("", lines[i].line);
            Assert.Equal(i, lines[i].lineIndex);
        }
    }

    [Fact]
    public void IterateOverSplitByLines_LineStartPositions_AreCorrect()
    {
        // Arrange
        string text = "abc\nde\nf";
        var positions = new List<(int lineStartPosition, int nextLineStartPosition)>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            positions.Add((lineStartPosition, nextLineStartPosition));
            return true;
        });

        // Assert
        Assert.Equal(3, positions.Count);
        Assert.Equal((0, 4), positions[0]); // "abc\n" -> next starts at 4
        Assert.Equal((4, 7), positions[1]); // "de\n" -> next starts at 7
        Assert.Equal((7, 8), positions[2]); // "f" -> next starts at 8 (text.Length)
    }

    [Fact]
    public void IterateOverSplitByLines_LineStartPositions_AreCorrect_R()
    {
        // Arrange
        string text = "abc\rde\rf";
        var positions = new List<(int lineStartPosition, int nextLineStartPosition)>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            positions.Add((lineStartPosition, nextLineStartPosition));
            return true;
        });

        // Assert
        Assert.Equal(3, positions.Count);
        Assert.Equal((0, 4), positions[0]); // "abc\n" -> next starts at 4
        Assert.Equal((4, 7), positions[1]); // "de\n" -> next starts at 7
        Assert.Equal((7, 8), positions[2]); // "f" -> next starts at 8 (text.Length)
    }

    [Fact]
    public void IterateOverSplitByLines_WithCRLF_LineStartPositions_AreCorrect()
    {
        // Arrange
        string text = "abc\r\nde\r\nf";
        var positions = new List<(int lineStartPosition, int nextLineStartPosition)>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            positions.Add((lineStartPosition, nextLineStartPosition));
            return true;
        });

        // Assert
        Assert.Equal(3, positions.Count);
        Assert.Equal((0, 5), positions[0]); // "abc\r\n" -> next starts at 5
        Assert.Equal((5, 9), positions[1]); // "de\r\n" -> next starts at 9
        Assert.Equal((9, 10), positions[2]); // "f" -> next starts at 10 (text.Length)
    }

    [Fact]
    public void IterateOverSplitByLines_ComplexMixedLineEndings_HandlesCorrectly()
    {
        // Arrange
        string text = "a\rb\r\ncRdRReR\nf";
        var lines = new List<(string line, int lineIndex, bool isLastLine)>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex, isLastLine));
            return true;
        });

        // Assert
        Assert.Equal(4, lines.Count);
        Assert.Equal(("a", 0, false), lines[0]);
        Assert.Equal(("b", 1, false), lines[1]);
        Assert.Equal(("cRdRReR", 2, false), lines[2]);
        Assert.Equal(("f", 3, true), lines[3]);
    }

    [Fact]
    public void IterateOverSplitByLines_SingleLF_CreatesTwoLines()
    {
        // Arrange
        string text = "\n";
        var lines = new List<string>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add(line.ToString());
            return true;
        });

        // Assert
        Assert.Equal(2, lines.Count);
        Assert.Equal("", lines[0]);
        Assert.Equal("", lines[1]);
    }

    [Fact]
    public void IterateOverSplitByLines_SingleCR_CreatesTwoLines()
    {
        // Arrange
        string text = "\r";
        var lines = new List<string>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add(line.ToString());
            return true;
        });

        // Assert
        Assert.Equal(2, lines.Count);
        Assert.Equal("", lines[0]);
        Assert.Equal("", lines[1]);
    }

    [Fact]
    public void IterateOverSplitByLines_SingleCRLF_CreatesTwoLines()
    {
        // Arrange
        string text = "\r\n";
        var lines = new List<string>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add(line.ToString());
            return true;
        });

        // Assert
        Assert.Equal(2, lines.Count);
        Assert.Equal("", lines[0]);
        Assert.Equal("", lines[1]);
    }

    [Fact]
    public void IterateOverSplitByLines_LongText_IteratesAllLines()
    {
        // Arrange
        var textBuilder = new System.Text.StringBuilder();
        for (int i = 0; i < 100; i++)
        {
            textBuilder.AppendLine($"Line {i}");
        }
        string text = textBuilder.ToString();
        int lineCount = 0;

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lineCount++;
            Assert.Equal(lineIndex, lineCount - 1);
            return true;
        });

        // Assert
        Assert.Equal(101, lineCount); // 100 lines + 1 empty line after last newline
    }

    [Fact]
    public void IterateOverSplitByLines_EarlyTermination_StopsImmediately()
    {
        // Arrange
        string text = "line1\nline2\nline3\nline4\nline5";
        int callbackInvocations = 0;

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            callbackInvocations++;
            return callbackInvocations < 3; // Stop after 3 invocations
        });

        // Assert
        Assert.Equal(3, callbackInvocations);
    }

    [Fact]
    public void IterateOverSplitByLines_LineIndices_AreSequential()
    {
        // Arrange
        string text = "a\rb\nc\rd\ne";
        var indices = new List<int>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            indices.Add(lineIndex);
            return true;
        });

        // Assert
        Assert.Equal(5, indices.Count);
        for (int i = 0; i < indices.Count; i++)
        {
            Assert.Equal(i, indices[i]);
        }
    }

    [Fact]
    public void IterateOverSplitByLines_OnlyLastLineMarkedAsLast()
    {
        // Arrange
        string text = "line1\nline2\nline3";
        var lastLineFlags = new List<bool>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lastLineFlags.Add(isLastLine);
            return true;
        });

        // Assert
        Assert.Equal(3, lastLineFlags.Count);
        Assert.False(lastLineFlags[0]);
        Assert.False(lastLineFlags[1]);
        Assert.True(lastLineFlags[2]);
    }

    [Fact]
    public void IterateOverSplitByLines_NextLineStartPosition_MatchesExpected()
    {
        // Arrange
        string text = "abc\rdefgh\ri";
        var actualPositions = new List<int>();
        var expectedPositions = new List<int> { 4, 10, 11 }; // After each line break or at end

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            actualPositions.Add(nextLineStartPosition);
            return true;
        });

        // Assert
        Assert.Equal(expectedPositions, actualPositions);
    }

    [Fact]
    public void IterateOverSplitByLines_CRCRLF_HandlesTwoCRsBeforeLF()
    {
        // Arrange
        string text = "line1\r\r\nline2";
        var lines = new List<string>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add(line.ToString());
            return true;
        });

        // Assert
        Assert.Equal(3, lines.Count);
        Assert.Equal("line1", lines[0]);
        Assert.Equal("", lines[1]);
        Assert.Equal("line2", lines[2]);
    }

    [Fact]
    public void IterateOverSplitByLines_CRCRLFCR_ComplexCRSequence()
    {
        // Arrange
        string text = "a\r\r\n\rb";
        var lines = new List<string>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add(line.ToString());
            return true;
        });

        // Assert
        Assert.Equal(4, lines.Count);
        Assert.Equal("a", lines[0]);
        Assert.Equal("", lines[1]);
        Assert.Equal("", lines[2]);
        Assert.Equal("b", lines[3]);
    }

    [Fact]
    public void IterateOverSplitByLines_SpecialCharacters_PreservesContent()
    {
        // Arrange
        string text = "hello\tworld\nline2 🎉\nline3";
        var lines = new List<string>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add(line.ToString());
            return true;
        });

        // Assert
        Assert.Equal(3, lines.Count);
        Assert.Equal("hello\tworld", lines[0]);
        Assert.Equal("line2 🎉", lines[1]);
        Assert.Equal("line3", lines[2]);
    }

    [Fact]
    public void IterateOverSplitByLines_ValidateSlicing_LineContentMatchesOriginal()
    {
        // Arrange
        string text = "first\nsecond\nthird";

        // Act & Assert
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            // Verify that the line content matches the slice of the original text
            string expectedLine = lineIndex switch
            {
                0 => "first",
                1 => "second",
                2 => "third",
                _ => throw new Exception("Unexpected line index")
            };
            Assert.Equal(expectedLine, line.ToString());
            return true;
        });
    }

    [Fact]
    public void IterateOverSplitByLines_ValidateSlicing_LineContentMatchesOriginal_R()
    {
        // Arrange
        string text = "first\rsecond\rthird";

        // Act & Assert
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            // Verify that the line content matches the slice of the original text
            string expectedLine = lineIndex switch
            {
                0 => "first",
                1 => "second",
                2 => "third",
                _ => throw new Exception("Unexpected line index")
            };
            Assert.Equal(expectedLine, line.ToString());
            return true;
        });
    }

    [Fact]
    public void IterateOverSplitByLines_ValidateSlicing_LineContentMatchesOriginal_RN()
    {
        // Arrange
        string text = "first\r\nsecond\r\nthird";

        // Act & Assert
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            // Verify that the line content matches the slice of the original text
            string expectedLine = lineIndex switch
            {
                0 => "first",
                1 => "second",
                2 => "third",
                _ => throw new Exception("Unexpected line index")
            };
            Assert.Equal(expectedLine, line.ToString());
            return true;
        });
    }

    [Fact]
    public void IterateOverSplitByLines_EmptyLinesBetween_AllLinesReturned()
    {
        // Arrange
        string text = "line1\n\n\nline2";
        var lines = new List<(string line, int lineIndex)>();

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            lines.Add((line.ToString(), lineIndex));
            return true;
        });

        // Assert
        Assert.Equal(4, lines.Count);
        Assert.Equal(("line1", 0), lines[0]);
        Assert.Equal(("", 1), lines[1]);
        Assert.Equal(("", 2), lines[2]);
        Assert.Equal(("line2", 3), lines[3]);
    }

    [Fact]
    public void IterateOverSplitByLines_CallbackCanAccessLineMultipleTimes()
    {
        // Arrange
        string text = "test\nline";
        int accessCount = 0;

        // Act
        StringUtils.IterateOverSplitByLines(text, (line, lineIndex, lineStartPosition, nextLineStartPosition, isLastLine) =>
        {
            // Access line multiple times to ensure the span is stable during callback
            _ = line.Length;
            _ = line.ToString();
            _ = line[0];
            accessCount++;
            return true;
        });

        // Assert
        Assert.Equal(2, accessCount);
    }
}
