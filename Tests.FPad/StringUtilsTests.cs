using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPad;

namespace Tests.FPad;

public class StringUtilsTests
{
    [Fact]
    public void GetCommonPrefixLength_BothEmpty_ReturnsZero()
    {
        int result = StringUtils.GetCommonPrefixLength("", "");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonPrefixLength_FirstEmpty_ReturnsZero()
    {
        int result = StringUtils.GetCommonPrefixLength("", "hello");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonPrefixLength_SecondEmpty_ReturnsZero()
    {
        int result = StringUtils.GetCommonPrefixLength("hello", "");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonPrefixLength_IdenticalStrings_ReturnsFullLength()
    {
        int result = StringUtils.GetCommonPrefixLength("hello", "hello");
        Assert.Equal(5, result);
    }

    [Fact]
    public void GetCommonPrefixLength_NoCommonPrefix_ReturnsZero()
    {
        int result = StringUtils.GetCommonPrefixLength("hello", "world");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonPrefixLength_PartialPrefix_ReturnsCorrectLength()
    {
        int result = StringUtils.GetCommonPrefixLength("hello", "help");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetCommonPrefixLength_FirstShorterAndFullPrefix_ReturnsFirstLength()
    {
        int result = StringUtils.GetCommonPrefixLength("hel", "hello");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetCommonPrefixLength_SecondShorterAndFullPrefix_ReturnsSecondLength()
    {
        int result = StringUtils.GetCommonPrefixLength("hello", "hel");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetCommonPrefixLength_SingleCharMatch_ReturnsOne()
    {
        int result = StringUtils.GetCommonPrefixLength("a", "a");
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetCommonPrefixLength_SingleCharNoMatch_ReturnsZero()
    {
        int result = StringUtils.GetCommonPrefixLength("a", "b");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonSuffixLength_BothEmpty_ReturnsZero()
    {
        int result = StringUtils.GetCommonSuffixLength("", "");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonSuffixLength_FirstEmpty_ReturnsZero()
    {
        int result = StringUtils.GetCommonSuffixLength("", "hello");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonSuffixLength_SecondEmpty_ReturnsZero()
    {
        int result = StringUtils.GetCommonSuffixLength("hello", "");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonSuffixLength_IdenticalStrings_ReturnsFullLength()
    {
        int result = StringUtils.GetCommonSuffixLength("hello", "hello");
        Assert.Equal(5, result);
    }

    [Fact]
    public void GetCommonSuffixLength_NoCommonSuffix_ReturnsZero()
    {
        int result = StringUtils.GetCommonSuffixLength("hello", "world");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonSuffixLength_PartialSuffix_ReturnsCorrectLength()
    {
        int result = StringUtils.GetCommonSuffixLength("hello", "jello");
        Assert.Equal(4, result);
    }

    [Fact]
    public void GetCommonSuffixLength_FirstShorterAndFullSuffix_ReturnsFirstLength()
    {
        int result = StringUtils.GetCommonSuffixLength("llo", "hello");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetCommonSuffixLength_SecondShorterAndFullSuffix_ReturnsSecondLength()
    {
        int result = StringUtils.GetCommonSuffixLength("hello", "llo");
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetCommonSuffixLength_SingleCharMatch_ReturnsOne()
    {
        int result = StringUtils.GetCommonSuffixLength("a", "a");
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetCommonSuffixLength_SingleCharNoMatch_ReturnsZero()
    {
        int result = StringUtils.GetCommonSuffixLength("a", "b");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonPrefixLength_CaseSensitive_ReturnsZero()
    {
        int result = StringUtils.GetCommonPrefixLength("Hello", "hello");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCommonSuffixLength_CaseSensitive_ReturnsZero()
    {
        int result = StringUtils.GetCommonSuffixLength("hellO", "hello");
        Assert.Equal(0, result);
    }
}
