using FPad.Edit;
using Tests.FPad;
using Xunit;

namespace Tests.FPad;

public class SingleSymbolEraseEditActionTests : IClassFixture<EncodingTestsFixture>
{
    private readonly EncodingTestsFixture fixture;

    public SingleSymbolEraseEditActionTests(EncodingTestsFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_Delete()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string prefix = "ab";
        string erased = "c";
        string suffix = "d";
        string initialText = prefix + erased + suffix;
        editor.SetTextNoUndo(initialText);
        int positionBeforeEdit = prefix.Length; // 2
        editor.Selection = new Selection(positionBeforeEdit, 0);

        IEditAction action = new SingleSymbolEraseEditAction(prefix.Length, suffix.Length,
            new ErasedSubString(erased, false, false), positionBeforeEdit);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + suffix, editor.Text);
        Assert.Equal(new Selection(prefix.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.Text);
        Assert.Equal(new Selection(positionBeforeEdit, 0), editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_Backspace()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string prefix = "a";
        string erased = "b";
        string suffix = "cd";
        string initialText = prefix + erased + suffix;
        editor.SetTextNoUndo(initialText);
        int positionBeforeEdit = prefix.Length + erased.Length; // 2
        editor.Selection = new Selection(positionBeforeEdit, 0);

        IEditAction action = new SingleSymbolEraseEditAction(prefix.Length, suffix.Length,
            new ErasedSubString(erased, false, false), positionBeforeEdit);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + suffix, editor.Text);
        Assert.Equal(new Selection(prefix.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.Text);
        Assert.Equal(new Selection(positionBeforeEdit, 0), editor.Selection);
    }

    [Fact]
    public void Absorb_SameType_SameCharsBeforeChange_Appends()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string initialText = "abc";
        editor.SetTextNoUndo(initialText);
        IEditAction action1 = new SingleSymbolEraseEditAction(1, 1, new ErasedSubString("b", false, false), 1); // erase 'b'
        IEditAction action2 = new SingleSymbolEraseEditAction(1, 0, new ErasedSubString("c", false, false), 1); // erase 'c' after 'b' erased

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);

        // Test the combined action
        IEditor testEditor = new MockEditor(fixture);
        testEditor.SetTextNoUndo(initialText);
        action1.Apply(testEditor);
        Assert.Equal("a", testEditor.Text);
        Assert.Equal(new Selection(1, 0), testEditor.Selection);

        action1.Rollback(testEditor);
        Assert.Equal(initialText, testEditor.Text);
        Assert.Equal(new Selection(1, 0), testEditor.Selection);
    }

    [Fact]
    public void Absorb_SameType_SameCharsAfterChange_Prepends()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string initialText = "abc";
        editor.SetTextNoUndo(initialText);
        IEditAction action1 = new SingleSymbolEraseEditAction(1, 1, new ErasedSubString("b", false, false), 2); // erase 'b'
        IEditAction action2 = new SingleSymbolEraseEditAction(0, 1, new ErasedSubString("a", false, false), 1); // erase 'a' before 'b'

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);

        // Test the combined action
        IEditor testEditor = new MockEditor(fixture);
        testEditor.SetTextNoUndo(initialText);
        action1.Apply(testEditor);
        Assert.Equal("c", testEditor.Text);
        Assert.Equal(new Selection(0, 0), testEditor.Selection); // charsBeforeChange of combined is 0

        action1.Rollback(testEditor);
        Assert.Equal(initialText, testEditor.Text);
        Assert.Equal(new Selection(2, 0), testEditor.Selection); // positionBeforeEdit of action1
    }

    [Fact]
    public void Absorb_DifferentType_DoesNotAbsorb()
    {
        // Arrange
        IEditAction action1 = new SingleSymbolEraseEditAction(1, 2, new ErasedSubString("a", false, false), 1); // 'a' is Word
        IEditAction action2 = new SingleSymbolEraseEditAction(1, 1, new ErasedSubString("=", false, false), 1); // '=' is NonWord

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_SameType_DifferentChars_DoesNotAbsorb()
    {
        // Arrange
        IEditAction action1 = new SingleSymbolEraseEditAction(1, 2, new ErasedSubString("a", false, false), 1);
        IEditAction action2 = new SingleSymbolEraseEditAction(2, 3, new ErasedSubString("a", false, false), 2); // different charsBefore and charsAfter

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);
    }

    [Fact]
    public void Absorb_GlueSpace_Backspace()
    {
        IEditAction action1 = new SingleSymbolEraseEditAction(10, 10,
            new ErasedSubString("erased", true, false), 13);
        IEditAction action2 = new SingleSymbolEraseEditAction(9, 10,
            new ErasedSubString(" ", false, false), 10);

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);

        // Check how it works
        IEditor editor = new MockEditor(fixture);
        editor.SetTextNoUndo("012345678 erased0123456789");
        editor.Selection = new Selection(13, 0);
        action1.Apply(editor);
        Assert.Equal("0123456780123456789", editor.Text);

        // Accepts more erased chars at end (so it didn't change its type to "space")
        IEditAction action3 = new SingleSymbolEraseEditAction(9, 9,
            new ErasedSubString("0", false, false), 9);
        bool absorbed3 = action1.Absorb(action3);
        Assert.True(absorbed3);

        // Doesnt absorb at beginning, where space was
        IEditAction action4 = new SingleSymbolEraseEditAction(8, 9,
            new ErasedSubString("8", false, false), 9);
        bool absorbed4 = action1.Absorb(action4);
        Assert.False(absorbed4);
    }

    [Fact]
    public void Absorb_GlueSpace_Delete()
    {
        IEditAction action1 = new SingleSymbolEraseEditAction(10, 10,
            new ErasedSubString("erased", false, true), 13);
        IEditAction action2 = new SingleSymbolEraseEditAction(10, 9,
            new ErasedSubString(" ", false, false), 10);

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);

        // Check how it works
        IEditor editor = new MockEditor(fixture);
        editor.SetTextNoUndo("0123456789erased 123456789");
        editor.Selection = new Selection(13, 0);
        action1.Apply(editor);
        Assert.Equal("0123456789123456789", editor.Text);

        // Accepts more erased chars at beginning (so it didn't change its type to "space")
        IEditAction action3 = new SingleSymbolEraseEditAction(9, 9,
            new ErasedSubString("9", false, false), 10);
        bool absorbed3 = action1.Absorb(action3);
        Assert.True(absorbed3);

        // Doesnt absorb at end, where space was // "012345678123456789"
        IEditAction action4 = new SingleSymbolEraseEditAction(9, 8,
            new ErasedSubString("1", false, false), 9);
        bool absorbed4 = action1.Absorb(action4);
        Assert.False(absorbed4);
    }

    [Fact]
    public void Absorb_GlueSpace_Both()
    {
        IEditAction action1 = new SingleSymbolEraseEditAction(11, 11,
            new ErasedSubString("erased", true, true), 13);
        IEditAction action2 = new SingleSymbolEraseEditAction(10, 11,
            new ErasedSubString(" ", false, false), 11); // backsp

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);

        IEditAction action3 = new SingleSymbolEraseEditAction(10, 10,
            new ErasedSubString(" ", false, false), 10); // del
        bool absorbed3 = action1.Absorb(action3);
        Assert.True(absorbed3);

        // Check how it works
        IEditor editor = new MockEditor(fixture);
        editor.SetTextNoUndo("0123456789 erased 0123456789");
        editor.Selection = new Selection(13, 0);

        action1.Apply(editor);
        Assert.Equal("01234567890123456789", editor.Text);

        // Cannot be glued to anything else
        IEditAction action4 = new SingleSymbolEraseEditAction(10, 9,
            new ErasedSubString("0", false, false), 10);
        bool absorbed4 = action1.Absorb(action4);
        Assert.False(absorbed4);

        IEditAction action5 = new SingleSymbolEraseEditAction(9, 10,
            new ErasedSubString("9", false, false), 10);
        bool absorbed5 = action1.Absorb(action5);
        Assert.False(absorbed5);
    }

    [Fact]
    public void Absorb_GlueSpace_DelAtBeginning()
    {
        IEditAction action1 = new SingleSymbolEraseEditAction(10, 10,
            new ErasedSubString("erased", true, false), 13);
        IEditAction action2 = new SingleSymbolEraseEditAction(9, 10,
            new ErasedSubString(" ", false, false), 9); // 9 instead of 10 means we moved left and pressed del instead of using backspace

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);
    }

    [Fact]
    public void Absorb_GlueSpace_BackspaceAtEnd()
    {
        IEditAction action1 = new SingleSymbolEraseEditAction(10, 10,
            new ErasedSubString("erased", false, true), 13);
        IEditAction action2 = new SingleSymbolEraseEditAction(10, 9,
            new ErasedSubString(" ", false, false), 11); // 11 instead of 10 means we moved right and pressed backspace instead of using delete

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.True(absorbed);
    }

    [Fact]
    public void Absorb_GlueSpace_Backspace_MoreThan1Space_Doesnt()
    {
        IEditAction action1 = new SingleSymbolEraseEditAction(10, 10,
            new ErasedSubString("erased", true, false), 13);
        IEditAction action2 = new SingleSymbolEraseEditAction(9, 10,
            new ErasedSubString(" ", true, false), 10);

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);

        IEditAction action3 = new SingleSymbolEraseEditAction(8, 10,
            new ErasedSubString("  ", false, false), 10);
        bool absorbed3 = action1.Absorb(action3);
        Assert.False(absorbed3);
    }

    [Fact]
    public void Absorb_GlueSpace_Delete_ManySpaces_Doesnt()
    {
        IEditAction action1 = new SingleSymbolEraseEditAction(10, 10,
            new ErasedSubString("erased", false, true), 13);
        IEditAction action2 = new SingleSymbolEraseEditAction(10, 9,
            new ErasedSubString(" ", false, true), 10);

        // Act
        bool absorbed = action1.Absorb(action2);

        // Assert
        Assert.False(absorbed);

        IEditAction action3 = new SingleSymbolEraseEditAction(10, 8,
            new ErasedSubString("  ", false, false), 10);
        bool absorbed3 = action1.Absorb(action3);
        Assert.False(absorbed3);
    }
}