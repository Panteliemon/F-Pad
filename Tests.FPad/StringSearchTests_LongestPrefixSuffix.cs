using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPad;

namespace Tests.FPad;

public class StringSearchTests_LongestPrefixSuffix
{
    [Fact]
    public void CalculateLongestPrefixSuffixArray_SingleCharacter_ReturnsZero()
    {
        // Arrange
        string input = "a";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateLongestPrefixSuffixArray_NoRepeatingPattern_ReturnsAllZeros()
    {
        // Arrange
        string input = "abcdefg";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateLongestPrefixSuffixArray_SimpleRepeatingPattern_ReturnsCorrectArray()
    {
        // Arrange
        string input = "ababa";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateLongestPrefixSuffixArray_ComplexPattern_ReturnsCorrectArray()
    {
        // Arrange
        string input = "ababaca";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateLongestPrefixSuffixArray_AllSameCharacters_ReturnsIncreasingSequence()
    {
        // Arrange
        string input = "aaaaa";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateLongestPrefixSuffixArray_PartialMatchAtEnd_ReturnsCorrectArray()
    {
        // Arrange
        string input = "abcabc";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateLongestPrefixSuffixArray_2_ReturnsCorrectArray()
    {
        // Arrange
        string input = "abceeeabce";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateLongestPrefixSuffixArray_LongPattern_ReturnsCorrectArray()
    {
        // Arrange
        string input = "ababababab";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateLongestPrefixSuffixArray_MixedPattern_ReturnsCorrectArray()
    {
        // Arrange
        string input = "ababa111";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    

    [Fact]
    public void CalculateLongestPrefixSuffixArray_PartialOverlap_ReturnsCorrectArray()
    {
        // Arrange
        string input = "aabaaab";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateLongestPrefixSuffixArray_SpecialCharacters_ReturnsCorrectArray()
    {
        // Arrange
        string input = "!@#!@#";
        int[] expected = GetLongestPrefixSuffixArray(input);

        // Act
        int[] actual = StringSearch.CalculateLongestPrefixSuffixArray(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    private static int[] GetLongestPrefixSuffixArray(string str)
    {
        int[] result = new int[str.Length];
        for (int i = 0; i < str.Length; i++)
        {
            result[i] = GetLongestPrefixSuffix(str[..(i + 1)]);
        }

        return result;
    }

    private static int GetLongestPrefixSuffix(string str)
    {
        int longest = 0;
        for (int i = 1; i < str.Length; i++)
        {
            if (string.Equals(str[..i], str[^i..]))
                longest = i;
        }

        return longest;
    }
}
