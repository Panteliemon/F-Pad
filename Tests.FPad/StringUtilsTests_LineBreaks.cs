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
    public void DetectLineBreaks_OnlyCR_Returns()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("line1\rline2");
        Assert.Equal(LineBreaks.Macintosh, result);
    }

    [Fact]
    public void DetectLineBreaks_MultipleLF_ReturnsUnix()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("a\nb\nc\nd");
        Assert.Equal(LineBreaks.Unix, result);
    }

    [Fact]
    public void DetectLineBreaks_MultipleCR_Returns()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("a\rb\rc\rd");
        Assert.Equal(LineBreaks.Macintosh, result);
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
        Assert.Equal(LineBreaks.Windows | LineBreaks.Unix, result);
    }

    [Fact]
    public void DetectLineBreaks_MixedCRLFAndCR_ReturnsMixed()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("line1\r\nline2\rline3");
        Assert.Equal(LineBreaks.Windows | LineBreaks.Macintosh, result);
    }

    [Fact]
    public void DetectLineBreaks_MixedCRLF_ReturnsMixed()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("line1\n\rline2");
        Assert.Equal(LineBreaks.Unix | LineBreaks.Macintosh, result);
    }

    [Fact]
    public void DetectLineBreaks_MixedALL_ReturnsMixed()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("line1\nline2\rline3\r\n");
        Assert.Equal(LineBreaks.Windows | LineBreaks.Unix | LineBreaks.Macintosh, result);
    }

    [Fact]
    public void DetectLineBreaks_LFThenCRLF_ReturnsMixed()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("a\nb\r\nc");
        Assert.Equal(LineBreaks.Unix | LineBreaks.Windows, result);
    }

    [Fact]
    public void DetectLineBreaks_CRThenCRLF_ReturnsMixed()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("a\rb\r\nc");
        Assert.Equal(LineBreaks.Macintosh | LineBreaks.Windows, result);
    }

    [Fact]
    public void DetectLineBreaks_StandaloneCRAndLF_ReturnsMixed()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("a\rb\nc");
        Assert.Equal(LineBreaks.Unix | LineBreaks.Macintosh, result);
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
    public void DetectLineBreaks_SingleCR_ReturnsMacintosh()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("\r");
        Assert.Equal(LineBreaks.Macintosh, result);
    }

    [Fact]
    public void DetectLineBreaks_CRAtEndOfString_ReturnsMac()
    {
        LineBreaks result = StringUtils.DetectLineBreaks("hello\r");
        Assert.Equal(LineBreaks.Macintosh, result);
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
    public void DetectLineBreaks_CRCRLFSequence_ReturnsWindowsMac()
    {
        // \r\r\n: first \r is stray, second \r\n is CRLF
        LineBreaks result = StringUtils.DetectLineBreaks("a\r\r\nb");
        Assert.Equal(LineBreaks.Windows | LineBreaks.Macintosh, result);
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
    public void NormalizeLineBreaks_EmptyString_ToMac_ReturnsEmpty()
    {
        string result = StringUtils.NormalizeLineBreaks("", LineBreaks.Macintosh);
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
    public void NormalizeLineBreaks_NoLineBreaks_ToMac_ReturnsUnchanged()
    {
        string result = StringUtils.NormalizeLineBreaks("hello world", LineBreaks.Macintosh);
        Assert.Equal("hello world", result);
    }

    [Fact]
    public void NormalizeLineBreaks_UnixToШindows_ConvertsCRLF()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\nline2", LineBreaks.Windows);
        Assert.Equal("line1\r\nline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MacToШindows_ConvertsCRLF()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\rline2", LineBreaks.Windows);
        Assert.Equal("line1\r\nline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_ШindowsToШindows_ConvertsCRLF()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2", LineBreaks.Windows);
        Assert.Equal("line1\r\nline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_WindowsToUnix_ConvertsLF()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2", LineBreaks.Unix);
        Assert.Equal("line1\nline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MacToUnix_ConvertsLF()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\rline2", LineBreaks.Unix);
        Assert.Equal("line1\nline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_WindowsToMac_Converts()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2", LineBreaks.Macintosh);
        Assert.Equal("line1\rline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_UnixToMac_Converts()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\nline2", LineBreaks.Macintosh);
        Assert.Equal("line1\rline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MacToMac_Converts()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\rline2", LineBreaks.Macintosh);
        Assert.Equal("line1\rline2", result);
    }

    [Fact]
    public void NormalizeLineBreaks_AlreadyUnix_ToUnix_Unchanged()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\nline2\nline3", LineBreaks.Unix);
        Assert.Equal("line1\nline2\nline3", result);
    }

    [Fact]
    public void NormalizeLineBreaks_AlreadyMac_ToMac_Unchanged()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\rline2\rline3", LineBreaks.Macintosh);
        Assert.Equal("line1\rline2\rline3", result);
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
    public void NormalizeLineBreaks_MixedToUnix_AllBecomeLF2()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\rline2\r\nline3", LineBreaks.Unix);
        Assert.Equal("line1\nline2\nline3", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MixedToWindows_AllBecomeCRLF()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2\nline3", LineBreaks.Windows);
        Assert.Equal("line1\r\nline2\r\nline3", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MixedToWindows_AllBecomeCRLF2()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\rline2\r\nline3", LineBreaks.Windows);
        Assert.Equal("line1\r\nline2\r\nline3", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MixedToMac_AllBecomeCR()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2\nline3", LineBreaks.Macintosh);
        Assert.Equal("line1\rline2\rline3", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MixedToMac_AllBecomeCR2()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\rline2\r\nline3", LineBreaks.Macintosh);
        Assert.Equal("line1\rline2\rline3", result);
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
    public void NormalizeLineBreaks_MultipleUnixToMac_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("a\nb\nc\nd", LineBreaks.Macintosh);
        Assert.Equal("a\rb\rc\rd", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MultipleWindowsToMac_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("a\r\nb\r\nc\r\nd", LineBreaks.Macintosh);
        Assert.Equal("a\rb\rc\rd", result);
    }

    [Fact]
    public void NormalizeLineBreaks_MultipleMacToWindows_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("a\rb\rc\rd", LineBreaks.Windows);
        Assert.Equal("a\r\nb\r\nc\r\nd", result);
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
    public void NormalizeLineBreaks_MultipleMacToWindows2_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("a\r\r\rd", LineBreaks.Windows);
        Assert.Equal("a\r\n\r\n\r\nd", result);
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
    public void NormalizeLineBreaks_TrailingNewline_ToMac_PreservedAsTrailing()
    {
        string result = StringUtils.NormalizeLineBreaks("line1\r\nline2\r\n", LineBreaks.Macintosh);
        Assert.Equal("line1\rline2\r", result);
    }

    [Fact]
    public void NormalizeLineBreaks_SingleLF_ToWindows_ReturnsCRLF()
    {
        string result = StringUtils.NormalizeLineBreaks("\n", LineBreaks.Windows);
        Assert.Equal("\r\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_SingleCR_ToWindows_ReturnsCRLF()
    {
        string result = StringUtils.NormalizeLineBreaks("\r", LineBreaks.Windows);
        Assert.Equal("\r\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_SingleCRLF_ToUnix_ReturnsLF()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\n", LineBreaks.Unix);
        Assert.Equal("\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_SingleCR_ToUnix_ReturnsLF()
    {
        string result = StringUtils.NormalizeLineBreaks("\r", LineBreaks.Unix);
        Assert.Equal("\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_SingleCRLF_ToMac_ReturnsCR()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\n", LineBreaks.Macintosh);
        Assert.Equal("\r", result);
    }

    [Fact]
    public void NormalizeLineBreaks_SingleLF_ToNac_ReturnsCR()
    {
        string result = StringUtils.NormalizeLineBreaks("\n", LineBreaks.Macintosh);
        Assert.Equal("\r", result);
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
    public void NormalizeLineBreaks_OnlyNewlines_ToMac_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("\n\n\n", LineBreaks.Macintosh);
        Assert.Equal("\r\r\r", result);
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
    public void NormalizeLineBreaks_OnlyNewlinesCRLF_ToMac_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\n\r\n\r\n", LineBreaks.Macintosh);
        Assert.Equal("\r\r\r", result);
    }

    [Fact]
    public void NormalizeLineBreaks_OnlyNewlinesCR_ToWindows_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\r\r", LineBreaks.Windows);
        Assert.Equal("\r\n\r\n\r\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_OnlyNewlinesCR_ToUnix_AllConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\r\r", LineBreaks.Unix);
        Assert.Equal("\n\n\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_OnlyNewlinesCR_ToMac_Unchanged()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\r\r", LineBreaks.Macintosh);
        Assert.Equal("\r\r\r", result);
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
            StringUtils.NormalizeLineBreaks("hello\nworld", LineBreaks.Windows | LineBreaks.Unix));
    }

    [Fact]
    public void NormalizeLineBreaks_Mixed_ThrowsArgumentException2()
    {
        Assert.Throws<ArgumentException>(() =>
            StringUtils.NormalizeLineBreaks("hello\nworld", LineBreaks.Windows | LineBreaks.Macintosh));
    }

    [Fact]
    public void NormalizeLineBreaks_Mixed_ThrowsArgumentException3()
    {
        Assert.Throws<ArgumentException>(() =>
            StringUtils.NormalizeLineBreaks("hello\nworld", LineBreaks.Unix | LineBreaks.Macintosh));
    }

    [Fact]
    public void NormalizeLineBreaks_Mixed_ThrowsArgumentExceptionAll()
    {
        Assert.Throws<ArgumentException>(() =>
            StringUtils.NormalizeLineBreaks("hello\nworld", LineBreaks.Windows | LineBreaks.Unix | LineBreaks.Macintosh));
    }

    [Fact]
    public void NormalizeLineBreaks_DetectThenNormalize_RoundTripToUnix()
    {
        string input = "first\r\nsecond\nthird\rfourth";
        LineBreaks detected = StringUtils.DetectLineBreaks(input);
        Assert.Equal(LineBreaks.Windows | LineBreaks.Unix | LineBreaks.Macintosh, detected);

        string result = StringUtils.NormalizeLineBreaks(input, LineBreaks.Unix);
        Assert.Equal(LineBreaks.Unix, StringUtils.DetectLineBreaks(result));
        Assert.Equal("first\nsecond\nthird\nfourth", result);
    }

    [Fact]
    public void NormalizeLineBreaks_DetectThenNormalize_RoundTripToWindows()
    {
        string input = "first\r\nsecond\nthird\rfourth";
        string result = StringUtils.NormalizeLineBreaks(input, LineBreaks.Windows);
        Assert.Equal(LineBreaks.Windows, StringUtils.DetectLineBreaks(result));
        Assert.Equal("first\r\nsecond\r\nthird\r\nfourth", result);
    }

    [Fact]
    public void NormalizeLineBreaks_DetectThenNormalize_RoundTripToMac()
    {
        string input = "first\r\nsecond\nthird\rfourth";
        string result = StringUtils.NormalizeLineBreaks(input, LineBreaks.Macintosh);
        Assert.Equal(LineBreaks.Macintosh, StringUtils.DetectLineBreaks(result));
        Assert.Equal("first\rsecond\rthird\rfourth", result);
    }

    [Fact]
    public void NormalizeLineBreaks_EmptyLines_ToWindows_AllSeparatorsConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("\n\ntext\n\n", LineBreaks.Windows);
        Assert.Equal("\r\n\r\ntext\r\n\r\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_EmptyLinesM_ToWindows_AllSeparatorsConverted()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\rtext\r\r", LineBreaks.Windows);
        Assert.Equal("\r\n\r\ntext\r\n\r\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_EmptyLines_ToUnix_AllSeparatorsPreservedAsBareNewlines()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\n\r\ntext\r\n\r\n", LineBreaks.Unix);
        Assert.Equal("\n\ntext\n\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_EmptyLinesM_ToUnix_AllSeparatorsPreservedAsBareNewlines()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\rtext\r\r", LineBreaks.Unix);
        Assert.Equal("\n\ntext\n\n", result);
    }

    [Fact]
    public void NormalizeLineBreaks_EmptyLines_ToMac_AllSeparatorsPreservedAsBareNewlines()
    {
        string result = StringUtils.NormalizeLineBreaks("\r\n\r\ntext\r\n\r\n", LineBreaks.Macintosh);
        Assert.Equal("\r\rtext\r\r", result);
    }

    [Fact]
    public void NormalizeLineBreaks_EmptyLinesU_ToMac_AllSeparatorsPreservedAsBareNewlines()
    {
        string result = StringUtils.NormalizeLineBreaks("\n\ntext\n\n", LineBreaks.Macintosh);
        Assert.Equal("\r\rtext\r\r", result);
    }
}
