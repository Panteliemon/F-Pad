using FPad;

namespace Tests.FPad;

public class StringUtilsTests_GetPositionAdaptive
{
    // -------------------------------------------------------------------------
    // Guard conditions
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_NullString_ReturnsZeroZeroZero()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive(null, 0, 0);
        Assert.Equal((0, 0, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_NegativeLineIndex_ReturnsZeroZeroZero()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive("hello", -1, 0);
        Assert.Equal((0, 0, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_NegativeCharIndex_ReturnsZeroZeroZero()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive("hello", 0, -1);
        Assert.Equal((0, 0, 0), result);
    }

    // -------------------------------------------------------------------------
    // Empty string
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_EmptyString_LineZeroColZero_ReturnsEndOfText()
    {
        // Loop never executes; falls through to end-of-text return
        (int, int, int) result = StringUtils.GetPositionAdaptive("", 0, 0);
        Assert.Equal((0, 0, 0), result);
    }

    // -------------------------------------------------------------------------
    // Single line, no line breaks
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_SingleLine_ColZero_ReturnsPositionZero()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive("hello", 0, 0);
        Assert.Equal((0, 0, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_SingleLine_ColInMiddle_ReturnsCorrectPosition()
    {
        // "hello": col 3 -> 'l' at index 3
        (int, int, int) result = StringUtils.GetPositionAdaptive("hello", 0, 3);
        Assert.Equal((3, 0, 3), result);
    }

    [Fact]
    public void GetPositionAdaptive_SingleLine_ColAtEnd_ReturnsEnd()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive("hello", 0, 5);
        Assert.Equal((5, 0, 5), result);
    }

    [Fact]
    public void GetPositionAdaptive_SingleLine_ColBeyondEnd_ReturnsEndOfLine()
    {
        // col 99 exceeds line length; line ends at newline or text end -> returns end of text
        (int, int, int) result = StringUtils.GetPositionAdaptive("hello", 0, 99);
        Assert.Equal((5, 0, 5), result);
    }

    // -------------------------------------------------------------------------
    // Single line ending with LF (\n)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_SingleLineLF_ColAtNewline_ReturnsNewlinePosition()
    {
        // "abc\n": col 3 hits the newline char -> line ends there
        (int, int, int) result = StringUtils.GetPositionAdaptive("abc\n", 0, 3);
        Assert.Equal((3, 0, 3), result);
    }

    [Fact]
    public void GetPositionAdaptive_SingleLineLF_ColBeyondEnd_ReturnsAtNewline()
    {
        // "abc\n": col 99 -> clamped at the '\n'
        (int, int, int) result = StringUtils.GetPositionAdaptive("abc\n", 0, 99);
        Assert.Equal((3, 0, 3), result);
    }

    // -------------------------------------------------------------------------
    // Single line ending with CR (\r) - CR is stray (not CR LF)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_SingleLineStandaloneCR_ColAtCR_ReturnsCRPosition()
    {
        // "abc\r": stray CR -> line ends at CR
        (int, int, int) result = StringUtils.GetPositionAdaptive("abc\r", 0, 3);
        Assert.Equal((3, 0, 3), result);
    }

    [Fact]
    public void GetPositionAdaptive_SingleLineStandaloneCR_ColBeyondEnd_ReturnsCRPosition()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive("abc\r", 0, 99);
        Assert.Equal((3, 0, 3), result);
    }

    // -------------------------------------------------------------------------
    // Two lines separated by LF (\n)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_TwoLines_LF_Line1ColZero_ReturnsFirstCharOfLine1()
    {
        // "ab\ncd": line 1, col 0 -> 'c' at index 3
        (int, int, int) result = StringUtils.GetPositionAdaptive("ab\ncd", 1, 0);
        Assert.Equal((3, 1, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_TwoLines_LF_Line1Col1_ReturnsSecondCharOfLine1()
    {
        // "ab\ncd": line 1, col 1 -> 'd' at index 4
        (int, int, int) result = StringUtils.GetPositionAdaptive("ab\ncd", 1, 1);
        Assert.Equal((4, 1, 1), result);
    }

    [Fact]
    public void GetPositionAdaptive_TwoLines_LF_Line1ColBeyondEnd_ReturnEndOfText()
    {
        // "ab\ncd": line 1 has 2 chars; col 99 -> end of text
        (int, int, int) result = StringUtils.GetPositionAdaptive("ab\ncd", 1, 99);
        Assert.Equal((5, 1, 2), result);
    }

    // -------------------------------------------------------------------------
    // Two lines separated by CR LF (\r\n)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_TwoLines_CRLF_Line1ColZero_ReturnsFirstCharOfLine1()
    {
        // "ab\r\ncd": line 1, col 0 -> 'c' at index 4
        (int, int, int) result = StringUtils.GetPositionAdaptive("ab\r\ncd", 1, 0);
        Assert.Equal((4, 1, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_TwoLines_CRLF_Line1Col1_ReturnsSecondCharOfLine1()
    {
        // "ab\r\ncd": line 1, col 1 -> 'd' at index 5
        (int, int, int) result = StringUtils.GetPositionAdaptive("ab\r\ncd", 1, 1);
        Assert.Equal((5, 1, 1), result);
    }

    [Fact]
    public void GetPositionAdaptive_TwoLines_CRLF_Line1ColBeyondEnd_ReturnsEndOfText()
    {
        // "ab\r\ncd": line 1 has 2 chars; col 99 -> end of text
        (int, int, int) result = StringUtils.GetPositionAdaptive("ab\r\ncd", 1, 99);
        Assert.Equal((6, 1, 2), result);
    }

    // -------------------------------------------------------------------------
    // Three lines
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_ThreeLines_LF_Line2ColZero_ReturnsFirstCharOfLine2()
    {
        // "a\nb\nc": line 2, col 0 -> 'c' at index 4
        (int, int, int) result = StringUtils.GetPositionAdaptive("a\nb\nc", 2, 0);
        Assert.Equal((4, 2, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_ThreeLines_LF_Line2Col1_ReturnsSecondCharOfLine2()
    {
        // "abc\nxyz\n123": line 2, col 1 -> '2' at index 9
        (int, int, int) result = StringUtils.GetPositionAdaptive("abc\nxyz\n123", 2, 1);
        Assert.Equal((9, 2, 1), result);
    }

    [Fact]
    public void GetPositionAdaptive_ThreeLines_CRLF_Line2ColZero_ReturnsFirstCharOfLine2()
    {
        // "a\r\nb\r\nc": line 2, col 0 -> 'c' at index 6
        (int, int, int) result = StringUtils.GetPositionAdaptive("a\r\nb\r\nc", 2, 0);
        Assert.Equal((6, 2, 0), result);
    }

    // -------------------------------------------------------------------------
    // Line index beyond text (line does not exist)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_TwoLines_LineIndexBeyondText_ReturnsEndOfText()
    {
        // "a\nb" has lines 0 and 1; requesting line 9
        (int, int, int) result = StringUtils.GetPositionAdaptive("a\nb", 9, 0);
        Assert.Equal((3, 1, 1), result);
    }

    // -------------------------------------------------------------------------
    // Line index 0, various col values
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_Line0Col0_MultiLineText_ReturnsPositionZero()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive("abc\ndef\nghi", 0, 0);
        Assert.Equal((0, 0, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_Line0ColBeyondNewline_ReturnsAtNewline()
    {
        // "abc\ndef": line 0 ends at '\n' (index 3); col 99 returns position 3
        (int, int, int) result = StringUtils.GetPositionAdaptive("abc\ndef", 0, 99);
        Assert.Equal((3, 0, 3), result);
    }

    // -------------------------------------------------------------------------
    // Empty lines
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_EmptyFirstLine_LF_Line0Col0_ReturnsNewlinePosition()
    {
        // "\nabc": line 0 is empty, col 0 -> '\n' at index 0
        (int, int, int) result = StringUtils.GetPositionAdaptive("\nabc", 0, 0);
        Assert.Equal((0, 0, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_EmptyMiddleLine_Line1Col0_ReturnsNewlinePosition()
    {
        // "a\n\nb": line 1 is empty, col 0 -> '\n' at index 2
        (int, int, int) result = StringUtils.GetPositionAdaptive("a\n\nb", 1, 0);
        Assert.Equal((2, 1, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_EmptyMiddleLineCRLF_Line1Col0_ReturnsCarriageReturnPosition()
    {
        // "a\r\n\r\nb": line 1 is empty; col 0 -> '\r' at index 3 (isAfter13 path: c==13 -> zero-length line)
        (int, int, int) result = StringUtils.GetPositionAdaptive("a\r\n\r\nb", 1, 0);
        Assert.Equal((3, 1, 0), result);
    }

    // -------------------------------------------------------------------------
    // CRLF: isAfter13 branch — first char of target line after \r\n
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_CRLF_TargetLineFirstChar_IsAfter13Path_ReturnsCorrectPosition()
    {
        // "x\r\nabc": after '\r\n', on line 1: a=4, b=5, c=6
        // When we arrive at index 4 (the 'a'), isAfter13=true because we just processed '\r'
        // and the '\n' triggers the isAfter13 branch check for the target line entry
        (int, int, int) result = StringUtils.GetPositionAdaptive("x\r\nabc", 1, 0);
        Assert.Equal((3, 1, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_CRLF_TargetLineMiddleChar_IsAfter13Path()
    {
        // "x\r\nabc": line 1, col 2 -> 'c' at index 5
        (int, int, int) result = StringUtils.GetPositionAdaptive("x\r\nabc", 1, 2);
        Assert.Equal((5, 1, 2), result);
    }

    // -------------------------------------------------------------------------
    // Mixed line breaks
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_MixedLFandCRLF_Line2Col0_ReturnsCorrectPosition()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive("a\nb\r\nc", 2, 0);
        Assert.Equal((5, 2, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_MixedCRLFandLF_Line2Col1_ReturnsCorrectPosition()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive("aa\r\nbb\ncc", 2, 1);
        Assert.Equal((8, 2, 1), result);
    }

    // -------------------------------------------------------------------------
    // Target col = 0 on every line (boundary sweep across all lines)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_AllLines_ColZero_ReturnsLineStartPositions_LF()
    {
        // "abc\nde\nf"  -> line 0 starts at 0, line 1 at 4, line 2 at 7
        (int, int, int) r0 = StringUtils.GetPositionAdaptive("abc\nde\nf", 0, 0);
        (int, int, int) r1 = StringUtils.GetPositionAdaptive("abc\nde\nf", 1, 0);
        (int, int, int) r2 = StringUtils.GetPositionAdaptive("abc\nde\nf", 2, 0);

        Assert.Equal((0, 0, 0), r0);
        Assert.Equal((4, 1, 0), r1);
        Assert.Equal((7, 2, 0), r2);
    }

    [Fact]
    public void GetPositionAdaptive_AllLines_ColZero_ReturnsLineStartPositions_CR()
    {
        (int, int, int) r0 = StringUtils.GetPositionAdaptive("abc\rde\rf", 0, 0);
        (int, int, int) r1 = StringUtils.GetPositionAdaptive("abc\rde\rf", 1, 0);
        (int, int, int) r2 = StringUtils.GetPositionAdaptive("abc\rde\rf", 2, 0);

        Assert.Equal((0, 0, 0), r0);
        Assert.Equal((4, 1, 0), r1);
        Assert.Equal((7, 2, 0), r2);
    }

    [Fact]
    public void GetPositionAdaptive_AllLines_ColZero_ReturnsLineStartPositions_CRLF()
    {
        // "abc\r\nde\r\nf" -> line 0 starts at 0, line 1 at 5, line 2 at 9
        (int, int, int) r0 = StringUtils.GetPositionAdaptive("abc\r\nde\r\nf", 0, 0);
        (int, int, int) r1 = StringUtils.GetPositionAdaptive("abc\r\nde\r\nf", 1, 0);
        (int, int, int) r2 = StringUtils.GetPositionAdaptive("abc\r\nde\r\nf", 2, 0);

        Assert.Equal((0, 0, 0), r0);
        Assert.Equal((5, 1, 0), r1);
        Assert.Equal((9, 2, 0), r2);
    }

    // -------------------------------------------------------------------------
    // Consistency with GetLineAndCol (round-trip)
    // -------------------------------------------------------------------------

    // No they are NOT reverse of each other.
    // Round-trip idea is a slop.

    [Fact]
    public void GetPositionAdaptive_RoundTrip_LF_PositionMatchesGetLineAndCol()
    {
        string text = "Hello\nWorld\nFoo";
        // For every position in the text, GetLineAndCol gives (line, col),
        // and GetPositionAdaptive should give back the same position.
        for (int pos = 0; pos <= text.Length; pos++)
        {
            (int line, int col) = StringUtils.GetLineAndCol(text, pos);
            (int resultPos, int resultLine, int resultCol) = StringUtils.GetPositionAdaptive(text, line, col);
            Assert.Equal(pos, resultPos);
            Assert.Equal(line, resultLine);
            Assert.Equal(col, resultCol);
        }
    }

    
    [Fact]
    public void GetPositionAdaptive_RoundTrip_CRLF_PositionMatchesGetLineAndCol()
    {
        string text = "Hello\r\nWorld\r\nFoo";
        for (int pos = 0; pos <= text.Length; pos++)
        {
            (int line, int col) = StringUtils.GetLineAndCol(text, pos);
            (int resultPos, int resultLine, int resultCol) = StringUtils.GetPositionAdaptive(text, line, col);
            if ((pos != 6) && (pos != 13))
            {
                Assert.Equal(pos, resultPos);
                Assert.Equal(line, resultLine);
                Assert.Equal(col, resultCol);
            }
        }
    }

    [Fact]
    public void GetPositionAdaptive_RoundTrip_Mixed_PositionMatchesGetLineAndCol()
    {
        string text = "ab\ncd\r\nef\ngh";
        for (int pos = 0; pos <= text.Length; pos++)
        {
            (int line, int col) = StringUtils.GetLineAndCol(text, pos);
            (int resultPos, int resultLine, int resultCol) = StringUtils.GetPositionAdaptive(text, line, col);
            if (pos != 6)
            {
                Assert.Equal(pos, resultPos);
                Assert.Equal(line, resultLine);
                Assert.Equal(col, resultCol);
            }
        }
    }

    // -------------------------------------------------------------------------
    // Col clamping: col exceeds line length, returns end of that line
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_ColExceedsLineLengthOnFirstLine_ClampsAtLineEnd_LF()
    {
        // "ab\ncd": line 0 has 2 chars; col 99 should clamp at '\n' (index 2)
        (int, int, int) result = StringUtils.GetPositionAdaptive("ab\ncd", 0, 99);
        Assert.Equal((2, 0, 2), result);
    }

    [Fact]
    public void GetPositionAdaptive_ColExceedsLineLengthOnSecondLine_ClampsAtLineEnd_LF()
    {
        // "ab\ncd\nef": line 1 has 2 chars; col 99 clamps at '\n' (index 5)
        (int, int, int) result = StringUtils.GetPositionAdaptive("ab\ncd\nef", 1, 99);
        Assert.Equal((5, 1, 2), result);
    }

    [Fact]
    public void GetPositionAdaptive_ColExceedsLineLengthOnFirstLine_ClampsAtLineEnd_CRLF()
    {
        // "ab\r\ncd": line 0 has 2 chars; col 99 clamps at '\r' (index 2)
        (int, int, int) result = StringUtils.GetPositionAdaptive("ab\r\ncd", 0, 99);
        Assert.Equal((2, 0, 2), result);
    }

    // -------------------------------------------------------------------------
    // Last line (no trailing newline)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_LastLine_ColBeyondEnd_ReturnsEndOfText()
    {
        // "a\nb": line 1 has 1 char; col 99 -> end of text at index 3
        (int, int, int) result = StringUtils.GetPositionAdaptive("a\nb", 1, 99);
        Assert.Equal((3, 1, 1), result);
    }

    [Fact]
    public void GetPositionAdaptive_LastLine_CRLF_ColBeyondEnd_ReturnsEndOfText()
    {
        // "a\r\nb": line 1 has 1 char; col 99 -> end of text at index 4
        (int, int, int) result = StringUtils.GetPositionAdaptive("a\r\nb", 1, 99);
        Assert.Equal((4, 1, 1), result);
    }

    // -------------------------------------------------------------------------
    // Text ends with newline: empty last line
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_TextEndsWithLF_EmptyLastLine_Col0_ReturnsEndOfText()
    {
        // "abc\n": line 1 is empty; col 0 -> end of text at index 4
        (int, int, int) result = StringUtils.GetPositionAdaptive("abc\n", 1, 0);
        Assert.Equal((4, 1, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_TextEndsWithCR_EmptyLastLine_Col0_ReturnsEndOfText()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive("abc\r", 1, 0);
        Assert.Equal((4, 1, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_TextEndsWithCRLF_EmptyLastLine_Col0_ReturnsEndOfText()
    {
        // "abc\r\n": line 1 is empty; col 0 -> end of text at index 5
        (int, int, int) result = StringUtils.GetPositionAdaptive("abc\r\n", 1, 0);
        Assert.Equal((5, 1, 0), result);
    }

    // -------------------------------------------------------------------------
    // Consecutive newlines
    // -------------------------------------------------------------------------

    [Fact]
    public void GetPositionAdaptive_ConsecutiveLFs_Line3Col0_ReturnsCorrectPosition()
    {
        // "\n\n\n": lines 0-3; line 3 starts at index 3 (end of text)
        (int, int, int) result = StringUtils.GetPositionAdaptive("\n\n\n", 3, 0);
        Assert.Equal((3, 3, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_ConsecutiveCRs_Line3Col0_ReturnsCorrectPosition()
    {
        (int, int, int) result = StringUtils.GetPositionAdaptive("\r\r\r", 3, 0);
        Assert.Equal((3, 3, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_ConsecutiveCRLFs_Line3Col0_ReturnsCorrectPosition()
    {
        // "\r\n\r\n\r\n": lines 0-3; line 3 starts at index 6 (end of text)
        (int, int, int) result = StringUtils.GetPositionAdaptive("\r\n\r\n\r\n", 3, 0);
        Assert.Equal((6, 3, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_ConsecutiveLFs_Line1Col0_ReturnsSecondNewline()
    {
        // "\n\n\n": line 1 col 0 -> '\n' at index 1
        (int, int, int) result = StringUtils.GetPositionAdaptive("\n\n\n", 1, 0);
        Assert.Equal((1, 1, 0), result);
    }

    [Fact]
    public void GetPositionAdaptive_ConsecutiveCRs_Line1Col0_ReturnsSecondNewline()
    {
        // "\n\n\n": line 1 col 0 -> '\n' at index 1
        (int, int, int) result = StringUtils.GetPositionAdaptive("\r\r\r", 1, 0);
        Assert.Equal((1, 1, 0), result);
    }
}
