using FPad.Edit;
using Tests.FPad;
using Xunit;

namespace Tests.FPad;

public class SelectionEraseEditActionTests : IClassFixture<EncodingTestsFixture>
{
    private readonly EncodingTestsFixture fixture;

    public SelectionEraseEditActionTests(EncodingTestsFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Apply_Rollback_RoundTrip()
    {
        // Arrange
        var editor = new MockEditor(fixture);
        string prefix = "Hello ";
        string erased = "World";
        string suffix = "!";
        string initialText = prefix + erased + suffix;
        editor.TextNoUndo = initialText;
        editor.Selection = new Selection(prefix.Length, erased.Length);

        var action = new SelectionEraseEditAction(prefix.Length, suffix.Length, erased);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + suffix, editor.TextNoUndo);
        Assert.Equal(new Selection(prefix.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(new Selection(prefix.Length, erased.Length), editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_EmptySuffix()
    {
        // Arrange
        var editor = new MockEditor(fixture);
        string prefix = "Hello ";
        string erased = "World";
        string suffix = "";
        string initialText = prefix + erased + suffix;
        editor.TextNoUndo = initialText;
        editor.Selection = new Selection(prefix.Length, erased.Length);

        var action = new SelectionEraseEditAction(prefix.Length, suffix.Length, erased);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + suffix, editor.TextNoUndo);
        Assert.Equal(new Selection(prefix.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(new Selection(prefix.Length, erased.Length), editor.Selection);
    }

    [Fact]
    public void Apply_Rollback_RoundTrip_EmptyPrefix()
    {
        // Arrange
        var editor = new MockEditor(fixture);
        string prefix = "";
        string erased = "Hello";
        string suffix = " World!";
        string initialText = prefix + erased + suffix;
        editor.TextNoUndo = initialText;
        editor.Selection = new Selection(prefix.Length, erased.Length);

        var action = new SelectionEraseEditAction(prefix.Length, suffix.Length, erased);

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(prefix + suffix, editor.TextNoUndo);
        Assert.Equal(new Selection(prefix.Length, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(initialText, editor.TextNoUndo);
        Assert.Equal(new Selection(prefix.Length, erased.Length), editor.Selection);
    }
}