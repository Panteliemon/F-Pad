using FPad.Edit;
using System;
using Xunit;

namespace Tests.FPad;

public class EditActionFactoryTests_DetectByTextChange : IClassFixture<EncodingTestsFixture>
{
    private readonly EncodingTestsFixture fixture;

    public EditActionFactoryTests_DetectByTextChange(EncodingTestsFixture fixture)
    {
        this.fixture = fixture;
    }

    private void AssertActionApplyAndRollback(IEditAction action, string textBefore, string textAfter, Selection selectionBefore, int positionAfter)
    {
        IEditor editor = new MockEditor(fixture);
        editor.SetTextNoUndo(textBefore);
        editor.Selection = selectionBefore;

        // Act: Apply
        action.Apply(editor);

        // Assert after Apply
        Assert.Equal(textAfter, editor.Text);
        Assert.Equal(new Selection(positionAfter, 0), editor.Selection);

        // Act: Rollback
        action.Rollback(editor);

        // Assert after Rollback
        Assert.Equal(textBefore, editor.Text);
        Assert.Equal(selectionBefore, editor.Selection);
    }

    #region No Change Detection

    [Fact]
    public void DetectByTextChange_NoChange_ReturnsNull()
    {
        // Arrange
        string text = "Hello World";
        Selection selection = new(5, 0);

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(text, selection, text, 5);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void DetectByTextChange_EmptyText_NoChange_ReturnsNull()
    {
        // Arrange
        string text = "";
        Selection selection = new(0, 0);

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(text, selection, text, 0);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region Typing Characters (SingleSymbolTypeEditAction)

    [Fact]
    public void DetectByTextChange_TypeSingleChar_ReturnsTypingAction()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello XWorld";
        Selection selectionBefore = new(6, 0); // cursor at position 6
        int positionAfter = 7; // cursor moved to position 7

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);
        Assert.Equal("Type", result.DisplayName);
    }

    [Fact]
    public void DetectByTextChange_TypeMultipleChars_ReturnsTypingAction()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello ABCWorld";
        Selection selectionBefore = new(6, 0);
        int positionAfter = 9;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_TypeMultipleChars_ApplyRollback()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello ABCWorld";
        Selection selectionBefore = new(6, 0);
        int positionAfter = 9;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);

        AssertActionApplyAndRollback(result, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_TypeAtBeginning_ReturnsTypingAction()
    {
        // Arrange
        string textBefore = "Hello";
        string textAfter = "XHello";
        Selection selectionBefore = new(0, 0);
        int positionAfter = 1;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_TypeAtEnd_ReturnsTypingAction()
    {
        // Arrange
        string textBefore = "Hello";
        string textAfter = "HelloX";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 6;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_TypeIntoEmptyText_ReturnsTypingAction()
    {
        // Arrange
        string textBefore = "";
        string textAfter = "Hello";
        Selection selectionBefore = new(0, 0);
        int positionAfter = 5;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_TypeOverSelection_ReturnsTypingAction()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello X";
        Selection selectionBefore = new(6, 5); // "World" selected
        int positionAfter = 7;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_TypeOverSelection_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello Universe";
        Selection selectionBefore = new(6, 5); // "World" selected
        int positionAfter = 14;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    #endregion

    #region Backspace (SingleSymbolEraseEditAction)

    [Fact]
    public void DetectByTextChange_BackspaceSingleChar_ReturnsEraseAction()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello orld";
        Selection selectionBefore = new(7, 0); // cursor after 'W'
        int positionAfter = 6;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolEraseEditAction>(result);
        Assert.Equal("Erase Text", result.DisplayName);
    }

    [Fact]
    public void DetectByTextChange_BackspaceMultipleChars_ReturnsEraseAction()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello rld";
        Selection selectionBefore = new(8, 0);
        int positionAfter = 6;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolEraseEditAction>(result);

        AssertActionApplyAndRollback(result, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_BackspaceAtBeginning_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "Hello";
        string textAfter = "ello";
        Selection selectionBefore = new(1, 0);
        int positionAfter = 0;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    #endregion

    #region Delete Key (SingleSymbolEraseEditAction)

    [Fact]
    public void DetectByTextChange_DeleteSingleChar_ReturnsEraseAction()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello orld";
        Selection selectionBefore = new(6, 0); // cursor before 'W'
        int positionAfter = 6;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolEraseEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_DeleteMultipleChars_ReturnsEraseAction()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello rld";
        Selection selectionBefore = new(6, 0);
        int positionAfter = 6;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolEraseEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_DeleteAtEnd_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "Hello";
        string textAfter = "Hell";
        Selection selectionBefore = new(4, 0);
        int positionAfter = 4;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    #endregion

    #region Clear Selection (SelectionEraseEditAction)

    [Fact]
    public void DetectByTextChange_ClearSelection_ReturnsSelectionEraseAction()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello ";
        Selection selectionBefore = new(6, 5); // "World" selected
        int positionAfter = 6;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SelectionEraseEditAction>(result);
        Assert.Equal("Erase Selection", result.DisplayName);
    }

    [Fact]
    public void DetectByTextChange_ClearSelectionAtBeginning_ReturnsSelectionEraseAction()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = " World";
        Selection selectionBefore = new(0, 5); // "Hello" selected
        int positionAfter = 0;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SelectionEraseEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_ClearEntireText_ReturnsSelectionEraseAction()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "";
        Selection selectionBefore = new(0, 11); // entire text selected
        int positionAfter = 0;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SelectionEraseEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_ClearSelection_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hello ";
        Selection selectionBefore = new(6, 5);
        int positionAfter = 6;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    #endregion

    #region Unrecognized Pattern (GenericEditAction)

    [Fact]
    public void DetectByTextChange_UnrecognizedPattern_ReturnsGenericAction()
    {
        // Arrange - This simulates an unusual edit that doesn't match known patterns
        // e.g., cursor moved significantly and text changed in unexpected ways
        string textBefore = "ABCDEF";
        string textAfter = "AXYZEF";
        Selection selectionBefore = new(0, 0); // cursor at beginning
        int positionAfter = 4; // cursor at position 4

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<GenericEditAction>(result);
        Assert.Equal("Edit", result.DisplayName);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "ABCDEF";
        string textAfter = "AXYZEF";
        Selection selectionBefore = new(3, 2);
        int positionAfter = 2;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_AtBegin()
    {
        // Arrange
        string textBefore = "ABCDEF";
        string textAfter = "XYZABCDEF";
        Selection selectionBefore = new(3, 2);
        int positionAfter = 9;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_AtEnd()
    {
        // Arrange
        string textBefore = "ABCDEF";
        string textAfter = "ABCDEFXYZ";
        Selection selectionBefore = new(3, 2);
        int positionAfter = 3;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeType_PrefixFail()
    {
        // Arrange
        string textBefore = "AbCDEF";
        string textAfter = "ABCxDEF";
        Selection selectionBefore = new(3, 0);
        int positionAfter = 4;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeType_SuffixFail()
    {
        // Arrange
        string textBefore = "ABCdEF";
        string textAfter = "ABCxDEF";
        Selection selectionBefore = new(3, 0);
        int positionAfter = 4;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeType2_PrefixFail()
    {
        // Arrange
        string textBefore = "AbCDEF";
        string textAfter = "ABCxyzDEF";
        Selection selectionBefore = new(3, 0);
        int positionAfter = 6;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeType2_SuffixFail()
    {
        // Arrange
        string textBefore = "ABCdEF";
        string textAfter = "ABCxyzDEF";
        Selection selectionBefore = new(3, 0);
        int positionAfter = 6;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeBackspace_PrefixFail()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "hell World";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 4;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeBackspace_SuffixFail()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hell world";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 4;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeBackspace2_PrefixFail()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "he World";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 2;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeBackspace2_SuffixFail()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "He world";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 2;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeDelete_PrefixFail()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "helloWorld";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 5;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeDelete_SuffixFail()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Helloworld";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 5;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeDelete2_PrefixFail()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "hellorld";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 5;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeDelete2_SuffixFail()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "HellOrld";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 5;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeSelectionErase_PrefixFail()
    {
        // Arrange
        string textBefore = "Hi there, hello!";
        string textAfter = "5i thhello!";
        Selection selectionBefore = new(5, 5);
        int positionAfter = 5;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_SuffixGoesIntoSelection()
    {
        // Arrange
        string textBefore = "abcXabcabc"; // Must correctly determine that only "X" has changed,
        string textAfter = "abcabcabc";   // and not "Xabc" has changed
        Selection selectionBefore = new(3, 4); // leaves 3 "chars to end", but suffix length for generic must be 6
        int positionAfter = 5;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        IEditor editor = new MockEditor(fixture);
        editor.SetTextNoUndo("1234567890");
        editor.Selection = selectionBefore;

        action.Apply(editor);

        Assert.Equal(new Selection(positionAfter, 0), editor.Selection);
        Assert.Equal("123567890", editor.Text); // must erase 1 symbol and not replace 4 symbols or something
    }

    [Fact]
    public void DetectByTextChange_GenericEdit_PretendsToBeSelectionErase_SuffixFail()
    {
        // Arrange
        string textBefore = "Hi there, hello!";
        string textAfter = "Hi thHello!";
        Selection selectionBefore = new(5, 5);
        int positionAfter = 5;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);
        Assert.IsType<GenericEditAction>(action);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void DetectByTextChange_TypeSpace_ReturnsTypingAction()
    {
        // Arrange
        string textBefore = "HelloWorld";
        string textAfter = "Hello World";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 6;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_TypeNewline_ReturnsTypingAction()
    {
        // Arrange
        string textBefore = "Hello";
        string textAfter = "Hello\n";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 6;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_TypeSpecialCharacters_ReturnsTypingAction()
    {
        // Arrange
        string textBefore = "Hello";
        string textAfter = "Hello!@#";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 8;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_BackspaceRemovesNewline_ReturnsEraseAction()
    {
        // Arrange
        string textBefore = "Hello\nWorld";
        string textAfter = "HelloWorld";
        Selection selectionBefore = new(6, 0);
        int positionAfter = 5;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolEraseEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_TypeUnicodeCharacters_ReturnsTypingAction()
    {
        // Arrange
        string textBefore = "Hello";
        string textAfter = "HelloЖОПА";
        var selectionBefore = new Selection(5, 0);
        int positionAfter = 9;

        // Act
        IEditAction result = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SingleSymbolTypeEditAction>(result);
    }

    [Fact]
    public void DetectByTextChange_ReplaceSelectionInMiddle_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "Hello Beautiful World";
        string textAfter = "Hello Amazing World";
        Selection selectionBefore = new(6, 9); // "Beautiful" selected
        int positionAfter = 13;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    #endregion

    #region Complex Scenarios

    [Fact]
    public void DetectByTextChange_TypeAtBeginning_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "World";
        string textAfter = "Hello World";
        Selection selectionBefore = new(0, 0);
        int positionAfter = 6;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_TypeAtEnd_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "Hello";
        string textAfter = "Hello World";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 11;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_DeleteEntireText_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "Hello";
        string textAfter = "";
        Selection selectionBefore = new(0, 5);
        int positionAfter = 0;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_DeleteInMiddle_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Helloorld";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 5;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    [Fact]
    public void DetectByTextChange_BackspaceInMiddle_ApplyRollbackRoundTrip()
    {
        // Arrange
        string textBefore = "Hello World";
        string textAfter = "Hell World";
        Selection selectionBefore = new(5, 0);
        int positionAfter = 4;

        IEditAction action = EditActionFactory.DetectByTextChange(textBefore, selectionBefore, textAfter, positionAfter);

        AssertActionApplyAndRollback(action, textBefore, textAfter, selectionBefore, positionAfter);
    }

    #endregion
}
