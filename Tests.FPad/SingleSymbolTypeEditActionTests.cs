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
        IEditor editor = new MockEditor(fixture);
        string prefix = "ab";
        string inserted = "c";
        string suffix = "d";
        string initialText = prefix + suffix;
        editor.SetTextNoUndo(initialText);
        int positionBeforeEdit = prefix.Length; // 2
        editor.Selection = new Selection(positionBeforeEdit, 0);

        IEditAction action = new SingleSymbolTypeEditAction(prefix.Length, suffix.Length, "", inserted, prefix.Length + inserted.Length);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.Text);
        Assert.Equal(new Selection(prefix.Length + inserted.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.Text);
        Assert.Equal(new Selection(positionBeforeEdit, 0), editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_TypeSpace()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string prefix = "ab";
        string inserted = " ";
        string suffix = "d";
        string initialText = prefix + suffix;
        editor.SetTextNoUndo(initialText);
        int positionBeforeEdit = prefix.Length; // 2
        editor.Selection = new Selection(positionBeforeEdit, 0);

        IEditAction action = new SingleSymbolTypeEditAction(prefix.Length, suffix.Length, "", inserted, prefix.Length + inserted.Length);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.Text);
        Assert.Equal(new Selection(prefix.Length + inserted.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.Text);
        Assert.Equal(new Selection(positionBeforeEdit, 0), editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_TypeNonWord()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string prefix = "ab";
        string inserted = "!";
        string suffix = "d";
        string initialText = prefix + suffix;
        editor.SetTextNoUndo(initialText);
        int positionBeforeEdit = prefix.Length; // 2
        editor.Selection = new Selection(positionBeforeEdit, 0);

        IEditAction action = new SingleSymbolTypeEditAction(prefix.Length, suffix.Length, "", inserted, prefix.Length + inserted.Length);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.Text);
        Assert.Equal(new Selection(prefix.Length + inserted.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.Text);
        Assert.Equal(new Selection(positionBeforeEdit, 0), editor.Selection);
    }

    [Fact]
    public void Absorb_SameType_Appends()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string initialText = "ab";
        editor.SetTextNoUndo(initialText);
        IEditAction action1 = new SingleSymbolTypeEditAction(1, 1, "", "c", 2); // insert 'c' at position 1
        IEditAction action2 = new SingleSymbolTypeEditAction(2, 1, "", "d", 3); // insert 'd' at position 2 (after 'c')

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);

        // Test the combined action
        IEditor testEditor = new MockEditor(fixture);
        testEditor.SetTextNoUndo(initialText);
        action1.Apply(testEditor);
        Assert.Equal("acdb", testEditor.Text);
        Assert.Equal(new Selection(3, 0), testEditor.Selection); // after "cd"

        action1.Rollback(testEditor);
        Assert.Equal(initialText, testEditor.Text);
        Assert.Equal(new Selection(1, 0), testEditor.Selection); // position of action1
    }

    [Fact]
    public void Absorb_SameType_InsertsInMiddle()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string initialText = "ab";
        editor.SetTextNoUndo(initialText);
        IEditAction action1 = new SingleSymbolTypeEditAction(1, 1, "", "cd", 3); // insert "cd" at position 1
        IEditAction action2 = new SingleSymbolTypeEditAction(2, 2, "", "e", 3); // insert 'e' at position 2 (between 'c' and 'd')

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);

        // Test the combined action
        IEditor testEditor = new MockEditor(fixture);
        testEditor.SetTextNoUndo(initialText);
        action1.Apply(testEditor);
        Assert.Equal("acedb", testEditor.Text);
        Assert.Equal(new Selection(3, 0), testEditor.Selection); // after "e"

        action1.Rollback(testEditor);
        Assert.Equal(initialText, testEditor.Text);
        Assert.Equal(new Selection(1, 0), testEditor.Selection);
    }

    [Fact]
    public void Absorb_DifferentType_DoesNotAbsorb()
    {
        // Arrange
        IEditAction action1 = new SingleSymbolTypeEditAction(1, 2, "", "a", 2); // 'a' is Word
        IEditAction action2 = new SingleSymbolTypeEditAction(1, 3, "", " ", 2); // ' ' is Space

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_SameType_ButErased_DoesNotAbsorb()
    {
        // Arrange
        IEditAction action1 = new SingleSymbolTypeEditAction(1, 2, "", "a", 2);
        IEditAction action2 = new SingleSymbolTypeEditAction(1, 1, "b", "a", 2); // has erased

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_Erased_Absorbs()
    {
        // Arrange
        IEditAction action1 = new SingleSymbolTypeEditAction(1, 2, "", "a", 2);
        IEditAction action2 = new SingleSymbolTypeEditAction(1, 1, "b", "a", 2); // has erased

        // Act
        bool absorbed = action2.Absorb(action1);

        // Assert
        Assert.True(absorbed);
    }

    [Fact]
    public void Absorb_SameType_OutOfRange_DoesNotAbsorb()
    {
        // Arrange
        IEditAction action1 = new SingleSymbolTypeEditAction(1, 2, "", "a", 2); // inserts at 1, inserted length 1, so range 1 to 2
        IEditAction action2 = new SingleSymbolTypeEditAction(3, 1, "", "a", 4); // position 3, out of range

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_GlueSpace_AtBegin()
    {
        IEditAction action1 = new SingleSymbolTypeEditAction(10, 20, "", " ", 11);
        IEditAction action2 = new SingleSymbolTypeEditAction(11, 20, "", "A", 12);

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);

        // Check how combined works
        IEditor editor = new MockEditor(fixture);
        editor.SetTextNoUndo("012345678901234567890123456789");
        editor.Selection = new Selection(10, 0);
        action1.Apply(editor);

        Assert.Equal("0123456789 A01234567890123456789", editor.Text);

        // Should not allow glue anything at begin
        IEditAction action3 = new SingleSymbolTypeEditAction(10, 22, "", "B", 11);
        bool absorbed3 = action1.Absorb(action3);
        Assert.False(absorbed3);

        IEditAction action4 = new SingleSymbolTypeEditAction(10, 22, "", " ", 11);
        bool absorbed4 = action1.Absorb(action4);
        Assert.False(absorbed4);

        // Should allow absorb right after space
        IEditAction action5 = new SingleSymbolTypeEditAction(11, 21, "", "x", 12);
        bool absorbed5 = action1.Absorb(action5);
        Assert.True(absorbed5);
        // Check how it works
        editor.SetTextNoUndo("012345678901234567890123456789");
        editor.Selection = new Selection(10, 0);
        action1.Apply(editor);
        Assert.Equal("0123456789 xA01234567890123456789", editor.Text);

        // Should not allow absorb spaces, because the type has switched to letters
        IEditAction action6 = new SingleSymbolTypeEditAction(11, 22, "", " ", 12);
        bool absorbed6 = action1.Absorb(action6);
        Assert.False(absorbed6);

        // Should allow absorb letter at end
        IEditAction action7 = new SingleSymbolTypeEditAction(13, 20, "", "Q", 14);
        bool absorbed7 = action1.Absorb(action7);
        Assert.True(absorbed7);
    }

    [Fact]
    public void Absorb_GlueSpace_AtEnd_Doesnt()
    {
        IEditAction action1 = new SingleSymbolTypeEditAction(10, 20, "", "A", 11);
        IEditAction action2 = new SingleSymbolTypeEditAction(11, 20, "", " ", 12);

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_GlueSpace_ManySpaces_Doesnt()
    {
        IEditAction action1 = new SingleSymbolTypeEditAction(10, 20, "", "  ", 12);
        IEditAction action2 = new SingleSymbolTypeEditAction(12, 20, "", "A", 13);

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_GlueSpace_ManyLetters_Doesnt()
    {
        IEditAction action1 = new SingleSymbolTypeEditAction(10, 20, "", " ", 11);
        IEditAction action2 = new SingleSymbolTypeEditAction(11, 20, "", "AA", 13);

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_GlueSpace_SpaceErasedSomething_Doesnt()
    {
        IEditAction action1 = new SingleSymbolTypeEditAction(10, 20, "Oops", " ", 11);
        IEditAction action2 = new SingleSymbolTypeEditAction(11, 20, "", "A", 12);

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_GlueSpace_LettersErasedSomething_Doesnt()
    {
        IEditAction action1 = new SingleSymbolTypeEditAction(10, 20, "", " ", 11);
        IEditAction action2 = new SingleSymbolTypeEditAction(11, 17, "Oops", "A", 12);

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }
}