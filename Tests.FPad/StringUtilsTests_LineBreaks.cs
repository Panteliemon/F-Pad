using System;
using FPad;
using Xunit;

namespace Tests.FPad;

public class StringUtilsTests_LineBreaks
{
    // -------------------------------------------------------------------------
    // DetectLineBreaks
    // -------------------------------------------------------------------------

    [Fact]
    public void DetectLineBreaks_EmptyString_ReturnsNone()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("");
        Assert.Equal(LineBreaks.None, result);
    }

    [Fact]
    public void DetectLineBreaks_NoLineBreaks_ReturnsNone()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("hello world");
        Assert.Equal(LineBreaks.None, result);
    }

    [Fact]
    public void DetectLineBreaks_OnlyLF_ReturnsUnix()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("line1\nline2");
        Assert.Equal(LineBreaks.Unix, result);
    }

    [Fact]
    public void DetectLineBreaks_MultipleLF_ReturnsUnix()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("a\nb\nc\nd");
        Assert.Equal(LineBreaks.Unix, result);
    }

    [Fact]
    public void DetectLineBreaks_OnlyCRLF_ReturnsWindows()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("line1\r\nline2");
        Assert.Equal(LineBreaks.Windows, result);
    }

    [Fact]
    public void DetectLineBreaks_MultipleCRLF_ReturnsWindows()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("a\r\nb\r\nc\r\nd");
        Assert.Equal(LineBreaks.Windows, result);
    }

    [Fact]
    public void DetectLineBreaks_MixedCRLFAndLF_ReturnsMixed()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("line1\r\nline2\nline3");
        Assert.Equal(LineBreaks.Mixed, result);
    }

    [Fact]
    public void DetectLineBreaks_LFThenCRLF_ReturnsMixed()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("a\nb\r\nc");
        Assert.Equal(LineBreaks.Mixed, result);
    }

    [Fact]
    public void DetectLineBreaks_StandaloneCR_ReturnsNone()
    {
        // Standalone CR is not treated as a line break
        LineBreaks result = StringUtils.DetectLineBreaks("line1\rline2");
        Assert.Equal(LineBreaks.None, result);
    }

    [Fact]
    public void DetectLineBreaks_MultipleStaloneCR_ReturnsNone()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("a\rb\rc");
        Assert.Equal(LineBreaks.None, result);
    }

    [Fact]
    public void DetectLineBreaks_StandaloneCRAndLF_ReturnsUnix()
    {
        // The \r before \n forms CRLF; the solo \r should be ignored
        LineBreaks result = StringUtils.DetectLineBreaks("a\rb\nc");
        Assert.Equal(LineBreaks.Unix, result);
    }

    [Fact]
    public void DetectLineBreaks_SingleLF_ReturnsUnix()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("\n");
        Assert.Equal(LineBreaks.Unix, result);
    }

    [Fact]
    public void DetectLineBreaks_SingleCRLF_ReturnsWindows()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("\r\n");
        Assert.Equal(LineBreaks.Windows, result);
    }

    [Fact]
    public void DetectLineBreaks_SingleCR_ReturnsNone()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("\r");
        Assert.Equal(LineBreaks.None, result);
    }

    [Fact]
    public void DetectLineBreaks_MixedFlags_ContainsBothFlags()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("a\nb\r\nc");
        Assert.True(result.HasFlag(LineBreaks.Unix));
        Assert.True(result.HasFlag(LineBreaks.Windows));
    }

    [Fact]
    public void DetectLineBreaks_CRAtEndOfString_ReturnsNone()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("hello\r");
        Assert.Equal(LineBreaks.None, result);
    }

    [Fact]
    public void DetectLineBreaks_CRLFAtEnd_ReturnsWindows()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("hello\r\n");
        Assert.Equal(LineBreaks.Windows, result);
    }

    [Fact]
    public void DetectLineBreaks_LFAtEnd_ReturnsUnix()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("hello\n");
        Assert.Equal(LineBreaks.Unix, result);
    }

    [Fact]
    public void DetectLineBreaks_CRCRLFSequence_ReturnsWindows()
    {
        // \r\r\n: first \r is stray, second \r\n is CRLF
        LineBreaks result = StringUtils.DetectLineBreaks("a\r\r\nb");
        Assert.Equal(LineBreaks.Windows, result);
    }

    [Fact]
    public void DetectLineBreaks_TextWithNoSpecialChars_ReturnsNone()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("abcdefghijklmnopqrstuvwxyz0123456789");
        Assert.Equal(LineBreaks.None, result);
    }

    // -------------------------------------------------------------------------
    // NormalizeLineBreaks
    // -------------------------------------------------------------------------

    [Fact]
    public void NormalizeLineBreaks_EmptyString_ToUnix_ReturnsEmpty()
    {
        string result = StringUtils.NormalizeLineBreaks("", LineBreaks.Unix);
        Assert.Equal("", result);
    }

    [Fact]
    public void NormalizeLineBreaks_EmptyString_ToWindows_ReturnsEmpty()
    {
        string result = StringUtils.NormalizeLineBreaks("", LineBreaks.Windows);
        Assert.Equal("", result);
    }

    [Fact]
    public void NormalizeLineBreaks_NoLineBreaks_ToUnix_ReturnsUnchanged()
    {
        string result = StringUtils.NormalizeLineBreaks("hello world", LineBreaks.Unix);
        Assert.Equal("hello world", result);
    }

    [Fact]
    public void NormalizeLineBreaks_NoLineBreaks_ToWindows_ReturnsUnchanged()
    {
        string result = StringUtils.NormalizeLineBreaks("hello world", LineBreaks.Windows);
        Assert.Equal("hello world", result);
    }

    [Fact]
    public void NormalizeLineBreaks_UnixToWindows_ConvertsCRLF()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\nline2", LineBreaks.Windows);
        Assert.Equal("line1\r\nline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_WindowsToUnix_ConvertsLF()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2", LineBreaks.Unix);
        Assert.Equal("line1\nline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_AlreadyUnix_ToUnix_Unchanged()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\nline2\nline3", LineBreaks.Unix);
        Assert.Equal("line1\nline2\nline3", result);
    }

    [Fact]
    public void NormalizeLineBreaks_AlreadyWindows_ToWindows_Unchanged()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2\r\nline3", LineBreaks.Windows);
        Assert.Equal("line1\r\nline2\r\nline3", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MixedToUnix_AllBecomeLF()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2\nline3", LineBreaks.Unix);
        Assert.Equal("line1\nline2\nline3", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MixedToWindows_AllBecomeCRLF()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2\nline3", LineBreaks.Windows);
        Assert.Equal("line1\r\nline2\r\nline3", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MultipleUnixToWindows_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("a\nb\nc\nd", LineBreaks.Windows);
        Assert.Equal("a\r\nb\r\nc\r\nd", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MultipleWindowsToUnix_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("a\r\nb\r\nc\r\nd", LineBreaks.Unix);
        Assert.Equal("a\nb\nc\nd", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MultipleUnixToWindows2_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("a\n\n\nd", LineBreaks.Windows);
        Assert.Equal("a\r\n\r\n\r\nd", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MultipleWindowsToUnix2_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("a\r\n\r\n\r\nd", LineBreaks.Unix);
        Assert.Equal("a\n\n\nd", result);
    }

    [Fact]
    public void NormalizeLineBreaks_TrailingNewline_ToWindows_PreservedAsTrailing()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\nline2\n", LineBreaks.Windows);
        Assert.Equal("line1\r\nline2\r\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_TrailingNewline_ToUnix_PreservedAsTrailing()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2\r\n", LineBreaks.Unix);
        Assert.Equal("line1\nline2\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_SingleLF_ToWindows_ReturnsCRLF()
    {
        string result = StringUtils.NormalizeLineBreaks("\n", LineBreaks.Windows);
        Assert.Equal("\r\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_SingleCRLF_ToUnix_ReturnsLF()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\n", LineBreaks.Unix);
        Assert.Equal("\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_OnlyNewlines_ToWindows_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("\n\n\n", LineBreaks.Windows);
        Assert.Equal("\r\n\r\n\r\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_OnlyNewlines_ToUnix_Unchanged()
    {
        string result = StringUtils.NormalizeLineBreaks("\n\n\n", LineBreaks.Unix);
        Assert.Equal("\n\n\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_OnlyNewlines_ToWindows_Unchanged()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\n\r\n\r\n", LineBreaks.Windows);
        Assert.Equal("\r\n\r\n\r\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_OnlyNewlines_ToUnix_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\n\r\n\r\n", LineBreaks.Unix);
        Assert.Equal("\n\n\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_StandaloneCR_ToUnix_PreservedInLine()
    {
        // Standalone \r is not a line break, so it stays in the line content
        string result = StringUtils.NormalizeLineBreaks("line1\rline2", LineBreaks.Unix);
        Assert.Equal("line1\rline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_StandaloneCR_ToWindows_PreservedInLine()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\rline2", LineBreaks.Windows);
        Assert.Equal("line1\rline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_None_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            StringUtils.NormalizeLineBreaks("hello\nworld", LineBreaks.None));
    }

    [Fact]
    public void NormalizeLineBreaks_Mixed_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            StringUtils.NormalizeLineBreaks("hello\nworld", LineBreaks.Mixed));
    }

    [Fact]
    public void NormalizeLineBreaks_DetectThenNormalize_RoundTripToUnix()
    {
        string input = "first\r\nsecond\nthird\r\nfourth";
        LineBreaks detected = StringUtils.DetectLineBreaks(input);
        Assert.Equal(LineBreaks.Mixed, detected);

        string result = StringUtils.NormalizeLineBreaks(input, LineBreaks.Unix);
        Assert.Equal(LineBreaks.Unix, StringUtils.DetectLineBreaks(result));
        Assert.Equal("first\nsecond\nthird\nfourth", result);
    }

    [Fact]
    public void NormalizeLineBreaks_DetectThenNormalize_RoundTripToWindows()
    {
        string input = "first\r\nsecond\nthird\r\nfourth";
        string result = StringUtils.NormalizeLineBreaks(input, LineBreaks.Windows);
        Assert.Equal(LineBreaks.Windows, StringUtils.DetectLineBreaks(result));
        Assert.Equal("first\r\nsecond\r\nthird\r\nfourth", result);
    }

    [Fact]
    public void NormalizeLineBreaks_EmptyLines_ToWindows_AllSeparatorsConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("\n\ntext\n\n", LineBreaks.Windows);
        Assert.Equal("\r\n\r\ntext\r\n\r\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_EmptyLines_ToUnix_AllSeparatorsPreservedAsBareNewlines()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\n\r\ntext\r\n\r\n", LineBreaks.Unix);
        Assert.Equal("\n\ntext\n\n", result);
    }
}
