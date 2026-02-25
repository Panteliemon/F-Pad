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
}