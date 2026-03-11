using FPad;

namespace Tests.FPad;

public class StringUtilsTests_LineAndCol
{
    // -------------------------------------------------------------------------
    // Simple overload: GetLineAndCol(string str, int position)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetLineAndCol_NullString_ReturnsZeroZero()
    {
        var result = StringUtils.GetLineAndCol(null, 0);
        Assert.Equal((0, 0), result);
    }

    [Fact]
    public void GetLineAndCol_EmptyString_PositionZero_ReturnsZeroZero()
    {
        var result = StringUtils.GetLineAndCol("", 0);
        Assert.Equal((0, 0), result);
    }

    [Fact]
    public void GetLineAndCol_SingleLine_PositionZero_ReturnsFirstCol()
    {
        var result = StringUtils.GetLineAndCol("hello", 0);
        Assert.Equal((0, 0), result);
    }

    [Fact]
    public void GetLineAndCol_SingleLine_PositionMiddle_ReturnsCorrectCol()
    {
        var result = StringUtils.GetLineAndCol("hello", 3);
        Assert.Equal((0, 3), result);
    }

    [Fact]
    public void GetLineAndCol_SingleLine_PositionAtEnd_ReturnsCorrectCol()
    {
        // position == str.Length is valid (past-the-end)
        var result = StringUtils.GetLineAndCol("hello", 5);
        Assert.Equal((0, 5), result);
    }

    [Fact]
    public void GetLineAndCol_SingleLine_PositionBeyondEnd_ReturnsStartValues()
    {
        // targetPosition > str.Length triggers guard -> returns (startLineIndex, startCharIndex) = (0, 0)
        var result = StringUtils.GetLineAndCol("hello", 6);
        Assert.Equal((0, 0), result);
    }

    [Fact]
    public void GetLineAndCol_TwoLines_PositionOnSecondLine_ReturnsLineOneColZero()
    {
        // "ab\ncd"  positions: a=0 b=1 \n=2 c=3 d=4
        // Position 3 is the first char of line 1
        var result = StringUtils.GetLineAndCol("ab\ncd", 3);
        Assert.Equal((1, 0), result);
    }

    [Fact]
    public void GetLineAndCol_TwoLines_PositionAfterNewline_ReturnsColZero()
    {
        // Position right after \n starts a new line at col 0
        var result = StringUtils.GetLineAndCol("abc\nxyz", 4);
        Assert.Equal((1, 0), result);
    }

    [Fact]
    public void GetLineAndCol_TwoLines_PositionMidSecondLine_ReturnsCorrectLineAndCol()
    {
        // "abc\nxyz"
        var result = StringUtils.GetLineAndCol("abc\nxyz", 6);
        Assert.Equal((1, 2), result);
    }

    [Fact]
    public void GetLineAndCol_ThreeLines_PositionOnThirdLine_ReturnsLineTwo()
    {
        // "a\nb\nc"  \n at 1 and 3
        var result = StringUtils.GetLineAndCol("a\nb\nc", 4);
        Assert.Equal((2, 0), result);
    }

    [Fact]
    public void GetLineAndCol_PositionAtNewlineChar_CountsNewlineAsCurrentLineChar()
    {
        // The loop runs up to (not including) targetPosition, so \n at index 3
        // is included in the iteration when targetPosition > 3.
        // When targetPosition == 3 (\n itself), the \n is NOT processed → col stays at 3
        var result = StringUtils.GetLineAndCol("abc\nxyz", 3);
        Assert.Equal((0, 3), result);
    }

    [Fact]
    public void GetLineAndCol_CarriageReturnNotTreatedAsLineBreak()
    {
        // \r (char 13) is just a regular char in GetLineAndCol
        var result = StringUtils.GetLineAndCol("ab\rcd", 4);
        Assert.Equal((0, 4), result);
    }

    [Fact]
    public void GetLineAndCol_CRLFSequence_OnlyLFBreaksLine()
    {
        // "ab\r\ncd"  \r at 2, \n at 3
        // After position 4 (= first char of second line): line 1, col 0
        var result = StringUtils.GetLineAndCol("ab\r\ncd", 4);
        Assert.Equal((1, 0), result);
    }

    [Fact]
    public void GetLineAndCol_MultipleNewlines_PositionAtEnd()
    {
        // "a\nb\nc\nd"
        var result = StringUtils.GetLineAndCol("a\nb\nc\nd", 7);
        Assert.Equal((3, 1), result);
    }

    [Fact]
    public void GetLineAndCol_ConsecutiveNewlines_CorrectLineCount()
    {
        // "\n\n\n"  — 4 lines, positions 0..3
        var result = StringUtils.GetLineAndCol("\n\n\n", 3);
        Assert.Equal((3, 0), result);
    }

    [Fact]
    public void GetLineAndCol_ConsecutiveNewlines_PositionOnFourthLine()
    {
        // "\r\n\r\n\r\nabc"  → position 8 is 'b' on line 3
        var result = StringUtils.GetLineAndCol("\r\n\r\n\r\nabc", 8);
        Assert.Equal((3, 2), result);
    }

    // -------------------------------------------------------------------------
    // Full overload: GetLineAndCol(str, startFromIndex, startLineIndex,
    //                              startCharIndex, targetPosition)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetLineAndCol_Full_NullString_ReturnsStartValues()
    {
        var result = StringUtils.GetLineAndCol(null, 0, 2, 5, 3);
        Assert.Equal((2, 5), result);
    }

    [Fact]
    public void GetLineAndCol_Full_TargetBeyondStringLength_ReturnsStartValues()
    {
        var result = StringUtils.GetLineAndCol("hello", 0, 0, 0, 10);
        Assert.Equal((0, 0), result);
    }

    [Fact]
    public void GetLineAndCol_Full_NegativeStartFromIndex_ReturnsStartValues()
    {
        var result = StringUtils.GetLineAndCol("hello", -1, 0, 0, 3);
        Assert.Equal((0, 0), result);
    }

    [Fact]
    public void GetLineAndCol_Full_StartFromIndexGreaterThanTarget_ReturnsStartValues()
    {
        // startFromIndex > targetPosition violates precondition
        var result = StringUtils.GetLineAndCol("hello world", 5, 0, 0, 3);
        Assert.Equal((0, 0), result);
    }

    [Fact]
    public void GetLineAndCol_Full_StartFromIndexEqualsTarget_ReturnsStartValues()
    {
        // Loop does not execute; returns start values unchanged
        var result = StringUtils.GetLineAndCol("hello", 3, 0, 0, 3);
        Assert.Equal((0, 0), result);
    }

    [Fact]
    public void GetLineAndCol_Full_ResumeFromMidString_ReturnsCorrectPosition()
    {
        // "abc\ndefg\nhi"
        // Suppose we already know position 4 is line 1, col 0.
        // Ask for position 8 starting from that cached state.
        // From index 4 to 8: d=4(col0) e=5(col1) f=6(col2) g=7(col3) → position 8 = line 1, col 4
        var result = StringUtils.GetLineAndCol("abc\ndefg\nhi", 4, 1, 0, 8);
        Assert.Equal((1, 4), result);
    }

    [Fact]
    public void GetLineAndCol_Full_ResumeAcrossLineBreak_CorrectlyIncrementsLine()
    {
        // "abc\ndefg\nhi"
        // Start from index 4 knowing we are at line 1, col 0; target position 10
        // d=4(1,0) e=5(1,1) f=6(1,2) g=7(1,3) \n=8 → line becomes 2, col 0; h=9(2,0)
        // position 10 = line 2, col 1
        var result = StringUtils.GetLineAndCol("abc\ndefg\nhi", 4, 1, 0, 10);
        Assert.Equal((2, 1), result);
    }

    [Fact]
    public void GetLineAndCol_Full_NonZeroStartLineAndChar_AddedToResult()
    {
        // Start with pretend offset: line 5, col 10; no newlines in range
        // "hello"  from 0 to 3 → adds 3 cols → (5, 13)
        var result = StringUtils.GetLineAndCol("hello", 0, 5, 10, 3);
        Assert.Equal((5, 13), result);
    }

    [Fact]
    public void GetLineAndCol_Full_StartLineOffsetResetOnNewLine()
    {
        // Start at (line=2, col=3); scan "a\nb" from 0 to 3
        // a → col 4; \n → line 3, col 0; b → col 1 → (3, 1)
        var result = StringUtils.GetLineAndCol("a\nb", 0, 2, 3, 3);
        Assert.Equal((3, 1), result);
    }

    [Fact]
    public void GetLineAndCol_Full_StartFromMiddle_SkipsEarlierContent()
    {
        // "line0\nline1\nline2"
        // If we start scanning from index 12 (start of "line2") with known state line=2,col=0,
        // and target position 17 (end), we should get (2, 5)
        var result = StringUtils.GetLineAndCol("line0\nline1\nline2", 12, 2, 0, 17);
        Assert.Equal((2, 5), result);
    }

    [Fact]
    public void GetLineAndCol_Full_ZeroTargetPosition_WithZeroStart_ReturnsZeroZero()
    {
        var result = StringUtils.GetLineAndCol("hello", 0, 0, 0, 0);
        Assert.Equal((0, 0), result);
    }

    [Fact]
    public void GetLineAndCol_Full_TargetEqualsStringLength_IsValid()
    {
        // "abc"  length 3, target 3 — past-the-end is allowed (guard: targetPosition > str.Length is false)
        var result = StringUtils.GetLineAndCol("abc", 0, 0, 0, 3);
        Assert.Equal((0, 3), result);
    }

    [Fact]
    public void GetLineAndCol_Full_StartFromIndexEqualsZero_SameAsSimpleOverload()
    {
        string text = "foo\nbar\nbaz";
        var simple = StringUtils.GetLineAndCol(text, 9);
        var full = StringUtils.GetLineAndCol(text, 0, 0, 0, 9);
        Assert.Equal(simple, full);
    }
}
