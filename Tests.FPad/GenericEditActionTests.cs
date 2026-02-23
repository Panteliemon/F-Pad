using FPad.Edit;
using Tests.FPad;
using Xunit;

namespace Tests.FPad;

public class GenericEditActionTests : IClassFixture<EncodingTestsFixture>
{
    private readonly EncodingTestsFixture fixture;

    public GenericEditActionTests(EncodingTestsFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_Insert()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string prefix = "Hello ";
        string erased = "";
        string inserted = "Beautiful ";
        string suffix = "World!";
        string initialText = prefix + erased + suffix;
        editor.TextNoUndo = initialText;
        Selection selectionBefore = new Selection(prefix.Length, erased.Length);
        Selection selectionAfter = new Selection(prefix.Length, inserted.Length);

        IEditAction action = new GenericEditAction(prefix.Length, suffix.Length, erased, inserted, selectionBefore, selectionAfter);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.TextNoUndo);
        Assert.Equal(selectionAfter, editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(selectionBefore, editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_Delete()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string prefix = "Hello ";
        string erased = "World ";
        string inserted = "";
        string suffix = "!";
        string initialText = prefix + erased + suffix;
        editor.TextNoUndo = initialText;
        Selection selectionBefore = new Selection(prefix.Length, erased.Length);
        Selection selectionAfter = new Selection(prefix.Length, inserted.Length);

        IEditAction action = new GenericEditAction(prefix.Length, suffix.Length, erased, inserted, selectionBefore, selectionAfter);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.TextNoUndo);
        Assert.Equal(selectionAfter, editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(selectionBefore, editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_Replace()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string prefix = "Hello ";
        string erased = "World";
        string inserted = "Universe";
        string suffix = "!";
        string initialText = prefix + erased + suffix;
        editor.TextNoUndo = initialText;
        Selection selectionBefore = new Selection(prefix.Length, erased.Length);
        Selection selectionAfter = new Selection(prefix.Length, inserted.Length);

        IEditAction action = new GenericEditAction(prefix.Length, suffix.Length, erased, inserted, selectionBefore, selectionAfter);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.TextNoUndo);
        Assert.Equal(selectionAfter, editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(selectionBefore, editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_AtBeginning()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string prefix = "";
        string erased = "Hello";
        string inserted = "Hi";
        string suffix = " World!";
        string initialText = prefix + erased + suffix;
        editor.TextNoUndo = initialText;
        Selection selectionBefore = new Selection(prefix.Length, erased.Length);
        Selection selectionAfter = new Selection(prefix.Length, inserted.Length);

        IEditAction action = new GenericEditAction(prefix.Length, suffix.Length, erased, inserted, selectionBefore, selectionAfter);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.TextNoUndo);
        Assert.Equal(selectionAfter, editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(selectionBefore, editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_AtEnd()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string prefix = "Hello World";
        string erased = "!";
        string inserted = "!!";
        string suffix = "";
        string initialText = prefix + erased + suffix;
        editor.TextNoUndo = initialText;
        Selection selectionBefore = new Selection(prefix.Length, erased.Length);
        Selection selectionAfter = new Selection(prefix.Length, inserted.Length);

        IEditAction action = new GenericEditAction(prefix.Length, suffix.Length, erased, inserted, selectionBefore, selectionAfter);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.TextNoUndo);
        Assert.Equal(selectionAfter, editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(selectionBefore, editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_EmptyText()
    {
        // Arrange
        IEditor editor = new MockEditor(fixture);
        string prefix = "";
        string erased = "";
        string inserted = "Hello";
        string suffix = "";
        string initialText = prefix + erased + suffix;
        editor.TextNoUndo = initialText;
        Selection selectionBefore = new Selection(prefix.Length, erased.Length);
        Selection selectionAfter = new Selection(prefix.Length, inserted.Length);

        IEditAction action = new GenericEditAction(prefix.Length, suffix.Length, erased, inserted, selectionBefore, selectionAfter);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + inserted + suffix, editor.TextNoUndo);
        Assert.Equal(selectionAfter, editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(selectionBefore, editor.Selection);
    }
}