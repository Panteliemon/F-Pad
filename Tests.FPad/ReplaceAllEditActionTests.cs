using FPad.Edit;
using System.Collections.Generic;
using Tests.FPad;
using Xunit;
using static FPad.Edit.ReplaceAllEditAction;

namespace Tests.FPad;

public class ReplaceAllEditActionTests : IClassFixture<EncodingTestsFixture>
{
    private readonly EncodingTestsFixture fixture;

    public ReplaceAllEditActionTests(EncodingTestsFixture fixture)
    {
        this.fixture = fixture;
    }

    private void AssertApplyAndRollback(
        string initialText,
        IReadOnlyList<Match> matches,
        string replacedWith,
        Selection selectionBefore,
        Selection selectionAfter,
        string expectedTextAfterApply)
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        editor.TextNoUndo = initialText;
        editor.Selection = selectionBefore;

        IEditAction action = new ReplaceAllEditAction(matches, replacedWith, selectionBefore, selectionAfter);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(expectedTextAfterApply, editor.TextNoUndo);
        Assert.Equal(selectionAfter, editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(selectionBefore, editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_SingleMatch()
    {
        AssertApplyAndRollback(
            initialText: "Hello World",
            matches: new List<Match> { new Match(6, "World") },
            replacedWith: "Universe",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(11, 0),
            expectedTextAfterApply: "Hello Universe");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_MultipleMatches()
    {
        AssertApplyAndRollback(
            initialText: "foo bar foo baz foo",
            matches: new List<Match>
            {
                new Match(0, "foo"),
                new Match(8, "foo"),
                new Match(16, "foo")
            },
            replacedWith: "FOO",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(19, 0),
            expectedTextAfterApply: "FOO bar FOO baz FOO");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_ConsecutiveMatches()
    {
        AssertApplyAndRollback(
            initialText: "aaa",
            matches: new List<Match>
            {
                new Match(0, "a"),
                new Match(1, "a"),
                new Match(2, "a")
            },
            replacedWith: "b",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(3, 0),
            expectedTextAfterApply: "bbb");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_ReplaceWithLongerString()
    {
        AssertApplyAndRollback(
            initialText: "I like cats and cats",
            matches: new List<Match>
            {
                new Match(7, "cats"),
                new Match(16, "cats")
            },
            replacedWith: "kittens",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(10, 0),
            expectedTextAfterApply: "I like kittens and kittens");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_ReplaceWithShorterString()
    {
        AssertApplyAndRollback(
            initialText: "I like kittens and kittens",
            matches: new List<Match>
            {
                new Match(7, "kittens"),
                new Match(19, "kittens")
            },
            replacedWith: "cats",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(11, 0),
            expectedTextAfterApply: "I like cats and cats");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_ReplaceWithEmptyString()
    {
        AssertApplyAndRollback(
            initialText: "Hello World Hello",
            matches: new List<Match>
            {
                new Match(0, "Hello "),
                new Match(12, "Hello")
            },
            replacedWith: "",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(6, 0),
            expectedTextAfterApply: "World ");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_NoMatches()
    {
        AssertApplyAndRollback(
            initialText: "Hello World",
            matches: new List<Match>(),
            replacedWith: "Anything",
            selectionBefore: new Selection(5, 0),
            selectionAfter: new Selection(5, 0),
            expectedTextAfterApply: "Hello World");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_CaseInsensitiveMatches()
    {
        AssertApplyAndRollback(
            initialText: "Hello hello HELLO",
            matches: new List<Match>
            {
                new Match(0, "Hello"),
                new Match(6, "hello"),
                new Match(12, "HELLO")
            },
            replacedWith: "Hi",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(8, 0),
            expectedTextAfterApply: "Hi Hi Hi");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_ReplaceAtBeginning()
    {
        AssertApplyAndRollback(
            initialText: "foo bar baz",
            matches: new List<Match> { new Match(0, "foo") },
            replacedWith: "START",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(11, 0),
            expectedTextAfterApply: "START bar baz");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_ReplaceAtEnd()
    {
        AssertApplyAndRollback(
            initialText: "foo bar baz",
            matches: new List<Match> { new Match(8, "baz") },
            replacedWith: "END",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(11, 0),
            expectedTextAfterApply: "foo bar END");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_ReplaceEntireText()
    {
        AssertApplyAndRollback(
            initialText: "foo",
            matches: new List<Match> { new Match(0, "foo") },
            replacedWith: "bar",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(3, 0),
            expectedTextAfterApply: "bar");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_ComplexTextWithMultipleMatches()
    {
        AssertApplyAndRollback(
            initialText: "The quick brown fox jumps over the lazy dog. The fox is quick.",
            matches: new List<Match>
            {
                new Match(4, "quick"),
                new Match(16, "fox"),
                new Match(45, "The"),
                new Match(49, "fox"),
                new Match(56, "quick")
            },
            replacedWith: "X",
            selectionBefore: new Selection(10, 5),
            selectionAfter: new Selection(0, 0),
            expectedTextAfterApply: "The X brown X jumps over the lazy dog. X X is X.");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_MixedLengthReplacements()
    {
        AssertApplyAndRollback(
            initialText: "a bb ccc dddd",
            matches: new List<Match>
            {
                new Match(0, "a"),
                new Match(2, "bb"),
                new Match(5, "ccc"),
                new Match(9, "dddd")
            },
            replacedWith: "xyz",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(13, 0),
            expectedTextAfterApply: "xyz xyz xyz xyz");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_ReplaceWithSameString()
    {
        AssertApplyAndRollback(
            initialText: "foo bar foo",
            matches: new List<Match>
            {
                new Match(0, "foo"),
                new Match(8, "foo")
            },
            replacedWith: "foo",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(11, 0),
            expectedTextAfterApply: "foo bar foo");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_SpecialCharacters()
    {
        AssertApplyAndRollback(
            initialText: "Line1\nLine2\r\nLine3\tTab",
            matches: new List<Match>
            {
                new Match(0, "Line1"),
                new Match(6, "Line2"),
                new Match(13, "Line3")
            },
            replacedWith: "L",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(10, 0),
            expectedTextAfterApply: "L\nL\r\nL\tTab");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_UnicodeCharacters()
    {
        AssertApplyAndRollback(
            initialText: "Hello 世界 Hello",
            matches: new List<Match>
            {
                new Match(0, "Hello"),
                new Match(9, "Hello")
            },
            replacedWith: "你好",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(8, 0),
            expectedTextAfterApply: "你好 世界 你好");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_OverlappingPatternsInOriginal()
    {
        AssertApplyAndRollback(
            initialText: "aaabbbaaabbb",
            matches: new List<Match>
            {
                new Match(0, "aaa"),
                new Match(6, "aaa")
            },
            replacedWith: "X",
            selectionBefore: new Selection(0, 0),
            selectionAfter: new Selection(8, 0),
            expectedTextAfterApply: "XbbbXbbb");
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_WithSelectionChange()
    {
        AssertApplyAndRollback(
            initialText: "test test test",
            matches: new List<Match>
            {
                new Match(0, "test"),
                new Match(5, "test"),
                new Match(10, "test")
            },
            replacedWith: "TEST",
            selectionBefore: new Selection(5, 4),
            selectionAfter: new Selection(0, 6),
            expectedTextAfterApply: "TEST TEST TEST");
    }

    [Fact]
    public void Absorb_ReturnsFalse()
    {
        // Arrange
        var matches = new List<Match>
        {
            new Match(0, "test")
        };
        IEditAction action1 = new ReplaceAllEditAction(matches, "TEST", new Selection(0, 0), new Selection(0, 0));
        IEditAction action2 = new ReplaceAllEditAction(matches, "TEST", new Selection(0, 0), new Selection(0, 0));

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void DisplayName_ReturnsCorrectValue()
    {
        // Arrange
        var matches = new List<Match>();
        IEditAction action = new ReplaceAllEditAction(matches, "", new Selection(0, 0), new Selection(0, 0));

        // Act & Assert
        Assert.Equal("Replace All", action.DisplayName);
    }
}
