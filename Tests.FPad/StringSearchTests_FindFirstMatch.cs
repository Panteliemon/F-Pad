using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPad;
using Xunit;

namespace Tests.FPad;

public class StringSearchTests_FindFirstMatch
{
    #region FindFirstMatch(ReadOnlySpan<char> str) - Case Sensitive

    [Fact]
    public void FindFirstMatch_CaseSensitive_ExactMatch_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatch_CaseSensitive_NoMatch_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("xyz", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatch_CaseSensitive_CaseMismatch_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("WORLD", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatch_CaseSensitive_MatchAtStart_ReturnsZero()
    {
        // Arrange
        var search = new StringSearch("Hello", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void FindFirstMatch_CaseSensitive_MatchAtEnd_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("world!", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatch_CaseSensitive_MultipleOccurrences_ReturnsFirstMatch()
    {
        // Arrange
        var search = new StringSearch("ab", caseSensitive: true);
        string text = "ab cd ab ef";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void FindFirstMatch_CaseSensitive_PartialOverlap_ReturnsFirstMatch()
    {
        // Arrange
        var search = new StringSearch("aba", caseSensitive: true);
        string text = "ababa";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void FindFirstMatch_CaseSensitive_SingleCharacter_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("w", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatch_CaseSensitive_EmptyText_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("test", caseSensitive: true);
        string text = "";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatch_CaseSensitive_SearchLongerThanText_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("Hello world!", caseSensitive: true);
        string text = "Hello";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(-1, result);
    }

    #endregion

    #region FindFirstMatch(ReadOnlySpan<char> str) - Case Insensitive

    [Fact]
    public void FindFirstMatch_CaseInsensitive_ExactMatch_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: false);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatch_CaseInsensitive_DifferentCase_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("WORLD", caseSensitive: false);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatch_CaseInsensitive_MixedCase_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("WoRlD", caseSensitive: false);
        string text = "Hello WORLD!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatch_CaseInsensitive_NoMatch_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("xyz", caseSensitive: false);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatch_CaseInsensitive_MultipleOccurrences_ReturnsFirstMatch()
    {
        // Arrange
        var search = new StringSearch("AB", caseSensitive: false);
        string text = "ab cd AB ef";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(0, result);
    }

    #endregion

    #region FindFirstMatch(ReadOnlySpan<char> str, int startPos, bool matchWholeWords) - No Whole Words

    [Fact]
    public void FindFirstMatchWithStartPos_NoWholeWords_MatchFromStart_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: false);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatchWithStartPos_NoWholeWords_StartPosAfterMatch_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("Hello", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 6, matchWholeWords: false);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatchWithStartPos_NoWholeWords_StartPosBeforeSecondOccurrence_ReturnsSecondMatch()
    {
        // Arrange
        var search = new StringSearch("test", caseSensitive: true);
        string text = "test abc test def";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 4, matchWholeWords: false);

        // Assert
        Assert.Equal(9, result);
    }

    [Fact]
    public void FindFirstMatchWithStartPos_NoWholeWords_StartPosAtMatchStart_ReturnsMatch()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 6, matchWholeWords: false);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatchWithStartPos_NoWholeWords_StartPosInMiddleOfMatch_SkipsMatch()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 7, matchWholeWords: false);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatchWithStartPos_CaseInsensitive_DifferentCase_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("WORLD", caseSensitive: false);
        string text = "Hello world test WORLD!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 10, matchWholeWords: false);

        // Assert
        Assert.Equal(17, result);
    }

    [Fact]
    public void FindFirstMatchWithStartPos_CaseInsensitive_DifferentCase_StillReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("WORLD", caseSensitive: false);
        string text = "Hello world test WORLD!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 3, matchWholeWords: false);

        // Assert
        Assert.Equal(6, result);
    }

    #endregion

    #region FindFirstMatch(ReadOnlySpan<char> str, int startPos, bool matchWholeWords) - Whole Words

    [Fact]
    public void FindFirstMatchWithWholeWords_MatchIsWholeWord_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: true);
        string text = "Hello world test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_MatchIsPartOfWord_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: true);
        string text = "Hello worldly test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_MatchHasWordCharactersBefore_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: true);
        string text = "Hello myworld test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_MatchAtStartOfString_ReturnsZero()
    {
        // Arrange
        var search = new StringSearch("Hello", caseSensitive: true);
        string text = "Hello world!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_MatchAtEndOfString_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: true);
        string text = "Hello world";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_UnderscoreBoundary_TreatedAsWordCharacter()
    {
        // Arrange
        var search = new StringSearch("test", caseSensitive: true);
        string text = "my_test_var";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(-1, result); // Underscore is considered part of word
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_DigitBoundary_TreatedAsWordCharacter()
    {
        // Arrange
        var search = new StringSearch("test", caseSensitive: true);
        string text = "test123 test 456test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(8, result); // First whole word match is at index 8
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_SpaceBoundary_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: true);
        string text = "Hello world test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_PunctuationBoundary_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("world", caseSensitive: true);
        string text = "Hello,world.test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_MultipleOccurrences_FirstNotWholeWord_ReturnsSecondMatch()
    {
        // Arrange
        var search = new StringSearch("test", caseSensitive: true);
        string text = "testing test done";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(8, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_WithStartPos_SkipsEarlierMatches()
    {
        // Arrange
        var search = new StringSearch("test", caseSensitive: true);
        string text = "test abc test def test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 5, matchWholeWords: true);

        // Assert
        Assert.Equal(9, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_CaseInsensitive_MixedCase_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("WORLD", caseSensitive: false);
        string text = "Hello world test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_NoWholeWordMatch_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("wor", caseSensitive: true);
        string text = "Hello world test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_CaseInsensitive_NoWholeWordMatch_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("WOR", caseSensitive: false);
        string text = "Hello world test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_Unicode1()
    {
        // Arrange
        var search = new StringSearch("ķi", caseSensitive: true);
        string text = "āboli.gurķi.ķiploki";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_Unicode2()
    {
        // Arrange
        var search = new StringSearch("ķi", caseSensitive: true);
        string text = "āboli.gurķi.ķiploki";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: false);

        // Assert
        Assert.Equal(9, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_Unicode3()
    {
        // Arrange
        var search = new StringSearch("gur", caseSensitive: true);
        string text = "āboli.gurķi.ķiploki";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_Unicode4()
    {
        // Arrange
        var search = new StringSearch("GURĶI", caseSensitive: false);
        string text = "āboli.gurķi.ķiploki";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(6, result);
    }

    #endregion

    #region FindFirstMatch - Complex Patterns (KMP Algorithm)

    [Fact]
    public void FindFirstMatch_RepeatingPattern_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("ababaca", caseSensitive: true);
        string text = "ababababacaxyz";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void FindFirstMatch_LongRepeatingPattern_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("aaaa", caseSensitive: true);
        string text = "aaaaab";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void FindFirstMatch_PatternWithPartialMatches_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("abcabc", caseSensitive: true);
        string text = "abcabdabcabc";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(6, result);
    }

    #endregion

    #region Edge Cases and Special Scenarios

    [Fact]
    public void FindFirstMatch_EntireTextMatches_ReturnsZero()
    {
        // Arrange
        var search = new StringSearch("test", caseSensitive: true);
        string text = "test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void FindFirstMatch_SpecialCharacters_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("!@#", caseSensitive: true);
        string text = "abc!@#def";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public void FindFirstMatch_Unicode_ReturnsCorrectIndex()
    {
        // Arrange
        var search = new StringSearch("мир", caseSensitive: true);
        string text = "Привет мир!";

        // Act
        int result = search.FindFirstMatch(text.AsSpan());

        // Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_SingleWordText_ReturnsZero()
    {
        // Arrange
        var search = new StringSearch("test", caseSensitive: true);
        string text = "test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void FindFirstMatchWithStartPos_StartPosAtEnd_ReturnsMinusOne()
    {
        // Arrange
        var search = new StringSearch("test", caseSensitive: true);
        string text = "Hello test";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 10, matchWholeWords: false);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_NumbersInWord_TreatedAsPartOfWord()
    {
        // Arrange
        var search = new StringSearch("var", caseSensitive: true);
        string text = "var1 var var2";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(5, result); // "var" at position 5 is a whole word
    }

    [Fact]
    public void FindFirstMatchWithWholeWords_HyphenBoundary_TreatedAsNonWordCharacter()
    {
        // Arrange
        var search = new StringSearch("test", caseSensitive: true);
        string text = "pre-test-post";

        // Act
        int result = search.FindFirstMatch(text.AsSpan(), startPos: 0, matchWholeWords: true);

        // Assert
        Assert.Equal(4, result);
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_NullSubstring_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new StringSearch(null!, caseSensitive: true));
    }

    [Fact]
    public void Constructor_EmptySubstring_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new StringSearch("", caseSensitive: true));
    }

    [Fact]
    public void Constructor_ValidSubstring_SetsProperties()
    {
        // Arrange & Act
        var search = new StringSearch("test", caseSensitive: true);

        // Assert
        Assert.Equal("test", search.SubStringToSearch);
        Assert.True(search.IsCaseSensitive);
    }

    [Fact]
    public void Constructor_CaseInsensitive_SetsProperties()
    {
        // Arrange & Act
        var search = new StringSearch("TEST", caseSensitive: false);

        // Assert
        Assert.Equal("TEST", search.SubStringToSearch);
        Assert.False(search.IsCaseSensitive);
    }

    #endregion
}
