using FPad;

namespace Tests.FPad;

public class StringUtilsTests_GetLinesCount
{
    // -------------------------------------------------------------------------
    // Null / empty
    // -------------------------------------------------------------------------

    [Fact]
    public void GetLinesCount_NullString_ReturnsZero()
    {
        int result = StringUtils.GetLinesCount(null);
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetLinesCount_EmptyString_ReturnsOne()
    {
        // Documented: "For empty strings returns 1"
        int result = StringUtils.GetLinesCount("");
        Assert.Equal(1, result);
    }

    // -------------------------------------------------------------------------
    // Single line (no newlines)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetLinesCount_SingleLine_NoNewlines_ReturnsOne()
    {
        int result = StringUtils.GetLinesCount("hello");
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetLinesCount_SingleChar_ReturnsOne()
    {
        int result = StringUtils.GetLinesCount("x");
        Assert.Equal(1, result);
    }

    // -------------------------------------------------------------------------
    // LF (\n) line breaks
    // -------------------------------------------------------------------------

    [Fact]
    public void GetLinesCount_OneLF_ReturnsTwoLines()
    {
        int result = StringUtils.GetLinesCount("ab\ncd");
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetLinesCount_TwoLF_ReturnsThreeLines()
    {
        int result = StringUtils.GetLinesCount("a\nb\nc");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetLinesCount_MultipleLF_ReturnsCorrectCount()
    {
        int result = StringUtils.GetLinesCount("a\nb\nc\nd");
        Assert.Equal(4, result);
    }

    [Fact]
    public void GetLinesCount_TrailingLF_CountsExtraLine()
    {
        // "abc\n" has two lines: "abc" and ""
        int result = StringUtils.GetLinesCount("abc\n");
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetLinesCount_LeadingLF_CountsExtraLine()
    {
        int result = StringUtils.GetLinesCount("\nabc");
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetLinesCount_OnlyLF_ReturnsLFCountPlusOne()
    {
        // "\n\n\n" -> 4 lines
        int result = StringUtils.GetLinesCount("\n\n\n");
        Assert.Equal(4, result);
    }

    [Fact]
    public void GetLinesCount_SingleLF_ReturnsTwoLines()
    {
        int result = StringUtils.GetLinesCount("\n");
        Assert.Equal(2, result);
    }

    // -------------------------------------------------------------------------
    // CR (\r) line breaks
    // -------------------------------------------------------------------------

    [Fact]
    public void GetLinesCount_OneCR_ReturnsTwoLines()
    {
        int result = StringUtils.GetLinesCount("ab\rcd");
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetLinesCount_TwoCR_ReturnsThreeLines()
    {
        int result = StringUtils.GetLinesCount("a\rb\rc");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetLinesCount_TrailingCR_CountsExtraLine()
    {
        int result = StringUtils.GetLinesCount("abc\r");
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetLinesCount_LeadingCR_CountsExtraLine()
    {
        int result = StringUtils.GetLinesCount("\rabc");
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetLinesCount_OnlyCR_ReturnsCountPlusOne()
    {
        // "\r\r\r" -> 4 lines
        int result = StringUtils.GetLinesCount("\r\r\r");
        Assert.Equal(4, result);
    }

    [Fact]
    public void GetLinesCount_SingleCR_ReturnsTwoLines()
    {
        int result = StringUtils.GetLinesCount("\r");
        Assert.Equal(2, result);
    }

    // -------------------------------------------------------------------------
    // CRLF (\r\n) line breaks — counts as ONE line break
    // -------------------------------------------------------------------------

    [Fact]
    public void GetLinesCount_OneCRLF_ReturnsTwoLines()
    {
        // \r increments, \n after \r does NOT increment (isAfter13 branch)
        int result = StringUtils.GetLinesCount("ab\r\ncd");
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetLinesCount_TwoCRLF_ReturnsThreeLines()
    {
        int result = StringUtils.GetLinesCount("a\r\nb\r\nc");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetLinesCount_MultipleCRLF_ReturnsCorrectCount()
    {
        int result = StringUtils.GetLinesCount("a\r\nb\r\nc\r\nd");
        Assert.Equal(4, result);
    }

    [Fact]
    public void GetLinesCount_TrailingCRLF_CountsExtraLine()
    {
        int result = StringUtils.GetLinesCount("abc\r\n");
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetLinesCount_LeadingCRLF_CountsExtraLine()
    {
        int result = StringUtils.GetLinesCount("\r\nabc");
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetLinesCount_OnlyCRLF_ReturnsSequenceCountPlusOne()
    {
        // "\r\n\r\n\r\n" -> 4 lines
        int result = StringUtils.GetLinesCount("\r\n\r\n\r\n");
        Assert.Equal(4, result);
    }

    [Fact]
    public void GetLinesCount_SingleCRLF_ReturnsTwoLines()
    {
        int result = StringUtils.GetLinesCount("\r\n");
        Assert.Equal(2, result);
    }

    // -------------------------------------------------------------------------
    // CRLF specifics: \r\r\n counts as TWO line breaks (two CRs before LF)
    // -------------------------------------------------------------------------

    [Fact]
    public void GetLinesCount_CRCRLFSequence_ReturnsThreeLines()
    {
        // \r -> lineIndex=1, isAfter13=true
        // \r -> lineIndex=2, isAfter13 stays true (second \r in isAfter13 branch)
        // \n -> isAfter13=false, no increment
        // Result: lineIndex=2 -> return 3
        int result = StringUtils.GetLinesCount("a\r\r\nb");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetLinesCount_CRFollowedByCR_EachCRCountsAsSeparateLine()
    {
        // \r\r -> two line increments
        int result = StringUtils.GetLinesCount("\r\r");
        Assert.Equal(3, result);
    }

    // -------------------------------------------------------------------------
    // Mixed line breaks
    // -------------------------------------------------------------------------

    [Fact]
    public void GetLinesCount_MixedCRLFAndLF_CountsCorrectly()
    {
        // "a\r\nb\nc" -> CRLF (1 break) + LF (1 break) = 3 lines
        int result = StringUtils.GetLinesCount("a\r\nb\nc");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetLinesCount_MixedLFAndCR_CountsCorrectly()
    {
        // "a\nb\rc" -> LF (1 break) + CR (1 break) = 3 lines
        int result = StringUtils.GetLinesCount("a\nb\rc");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetLinesCount_MixedAllThreeStyles_CountsCorrectly()
    {
        // "a\nb\r\nc\rd" -> LF + CRLF + CR = 4 lines
        int result = StringUtils.GetLinesCount("a\nb\r\nc\rd");
        Assert.Equal(4, result);
    }

    // -------------------------------------------------------------------------
    // Longer / realistic strings
    // -------------------------------------------------------------------------

    [Fact]
    public void GetLinesCount_LongTextWithUnixLineBreaks_ReturnsCorrectCount()
    {
        string text = string.Join("\n", new string[10]);
        // 10 empty strings joined by 9 newlines -> 10 lines
        int result = StringUtils.GetLinesCount(text);
        Assert.Equal(10, result);
    }

    [Fact]
    public void GetLinesCount_LongTextWithWindowsLineBreaks_ReturnsCorrectCount()
    {
        string text = string.Join("\r\n", new string[10]);
        int result = StringUtils.GetLinesCount(text);
        Assert.Equal(10, result);
    }

    [Fact]
    public void GetLinesCount_ConsecutiveNewlines_AllCounted()
    {
        // 5 LFs in a row -> 6 lines
        int result = StringUtils.GetLinesCount("\n\n\n\n\n");
        Assert.Equal(6, result);
    }

    [Fact]
    public void GetLinesCount_TextWithNoSpecialChars_ReturnsOne()
    {
        int result = StringUtils.GetLinesCount("abcdefghijklmnopqrstuvwxyz0123456789");
        Assert.Equal(1, result);
    }
}
