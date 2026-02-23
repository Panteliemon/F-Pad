using FPad.Edit;
using Tests.FPad;
using Xunit;

namespace Tests.FPad;

public class SingleSymbolTypeEditActionTests : IClassFixture<EncodingTestsFixture>
{
    private readonly EncodingTestsFixture fixture;

    public SingleSymbolTypeEditActionTests(EncodingTestsFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_TypeLetter()
    {
        // Arrange
        var editor = new MockEditor(fixture);
        string prefix = "ab";
        string inserted = "c";
        string suffix = "d";
        string initialText = prefix + suffix;
        editor.TextNoUndo = initialText;
        int positionBeforeEdit = prefix.Length; // 2
        editor.Selection = new Selection(positionBeforeEdit, 0);

        var action = new SingleSymbolTypeEditAction(prefix.Length, suffix.Length, "", inserted, prefix.Length + inserted.Length);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.TextNoUndo);
        Assert.Equal(new Selection(prefix.Length + inserted.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(new Selection(positionBeforeEdit, 0), editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_TypeSpace()
    {
        // Arrange
        var editor = new MockEditor(fixture);
        string prefix = "ab";
        string inserted = " ";
        string suffix = "d";
        string initialText = prefix + suffix;
        editor.TextNoUndo = initialText;
        int positionBeforeEdit = prefix.Length; // 2
        editor.Selection = new Selection(positionBeforeEdit, 0);

        var action = new SingleSymbolTypeEditAction(prefix.Length, suffix.Length, "", inserted, prefix.Length + inserted.Length);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.TextNoUndo);
        Assert.Equal(new Selection(prefix.Length + inserted.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(new Selection(positionBeforeEdit, 0), editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_TypeNonWord()
    {
        // Arrange
        var editor = new MockEditor(fixture);
        string prefix = "ab";
        string inserted = "!";
        string suffix = "d";
        string initialText = prefix + suffix;
        editor.TextNoUndo = initialText;
        int positionBeforeEdit = prefix.Length; // 2
        editor.Selection = new Selection(positionBeforeEdit, 0);

        var action = new SingleSymbolTypeEditAction(prefix.Length, suffix.Length, "", inserted, prefix.Length + inserted.Length);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.TextNoUndo);
        Assert.Equal(new Selection(prefix.Length + inserted.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(new Selection(positionBeforeEdit, 0), editor.Selection);
    }

    [Fact]
    public void Absorb_SameType_Appends()
    {
        // Arrange
        var editor = new MockEditor(fixture);
        string initialText = "ab";
        editor.TextNoUndo = initialText;
        var action1 = new SingleSymbolTypeEditAction(1, 1, "", "c", 2); // insert 'c' at position 1
        var action2 = new SingleSymbolTypeEditAction(2, 1, "", "d", 3); // insert 'd' at position 2 (after 'c')

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);

        // Test the combined action
        var testEditor = new MockEditor(fixture);
        testEditor.TextNoUndo = initialText;
        action1.Apply(testEditor);
        Assert.Equal("acdb", testEditor.TextNoUndo);
        Assert.Equal(new Selection(3, 0), testEditor.Selection); // after "cd"

        action1.Rollback(testEditor);
        Assert.Equal(initialText, testEditor.TextNoUndo);
        Assert.Equal(new Selection(1, 0), testEditor.Selection); // position of action1
    }

    [Fact]
    public void Absorb_SameType_InsertsInMiddle()
    {
        // Arrange
        var editor = new MockEditor(fixture);
        string initialText = "ab";
        editor.TextNoUndo = initialText;
        var action1 = new SingleSymbolTypeEditAction(1, 1, "", "cd", 3); // insert "cd" at position 1
        var action2 = new SingleSymbolTypeEditAction(2, 2, "", "e", 3); // insert 'e' at position 2 (between 'c' and 'd')

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);

        // Test the combined action
        var testEditor = new MockEditor(fixture);
        testEditor.TextNoUndo = initialText;
        action1.Apply(testEditor);
        Assert.Equal("acedb", testEditor.TextNoUndo);
        Assert.Equal(new Selection(3, 0), testEditor.Selection); // after "e"

        action1.Rollback(testEditor);
        Assert.Equal(initialText, testEditor.TextNoUndo);
        Assert.Equal(new Selection(1, 0), testEditor.Selection);
    }

    [Fact]
    public void Absorb_DifferentType_DoesNotAbsorb()
    {
        // Arrange
        var action1 = new SingleSymbolTypeEditAction(1, 2, "", "a", 2); // 'a' is Word
        var action2 = new SingleSymbolTypeEditAction(1, 3, "", " ", 2); // ' ' is Space

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_SameType_ButErased_DoesNotAbsorb()
    {
        // Arrange
        var action1 = new SingleSymbolTypeEditAction(1, 2, "", "a", 2);
        var action2 = new SingleSymbolTypeEditAction(1, 1, "b", "a", 2); // has erased

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_Erased_Absorbs()
    {
        // Arrange
        var action1 = new SingleSymbolTypeEditAction(1, 2, "", "a", 2);
        var action2 = new SingleSymbolTypeEditAction(1, 1, "b", "a", 2); // has erased

        // Act
        bool absorbed = action2.Absorb(action1);

        // Assert
        Assert.True(absorbed);
    }

    [Fact]
    public void Absorb_SameType_OutOfRange_DoesNotAbsorb()
    {
        // Arrange
        var action1 = new SingleSymbolTypeEditAction(1, 2, "", "a", 2); // inserts at 1, inserted length 1, so range 1 to 2
        var action2 = new SingleSymbolTypeEditAction(3, 1, "", "a", 4); // position 3, out of range

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }
}