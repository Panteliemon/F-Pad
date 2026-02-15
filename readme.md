# F-Pad

A minimalist text editor for Windows inspired by the classic Notepad.

## Features
- **Unformatted Text Editing**: Every one of us needs pureness of unformatted *.txt files once in a while, F-Pad makes sure the text you put in stays unformatted.
- **Single Document Interface**: We ain't need no tabs in our app, we wanna scatter individual windows all over the place, like back in ye olde days.
- **Encoding Detection**: Classic Notepad used to annoy you with no way of using different ANSI pages for different files on the same machine - we fixed that.
- **Change Encoding**: You know this, you create empty text file in Explorer, you open it in Notepad and wanna switch encoding to Unicode - you have to overwrite the file you've just created, you have to answer stupid questions - we fixed that as well. Btw Unicode is default, sue me. (Don't sue me)
- **Double-Click Whole Word Selection**: Made it less annoying than in the original Notepad.

## Technical Details

### Technology Stack
- **C# and .NET 8.0**
- **Windows Forms 🙈**: if 10 years ago someone would say that I'll be of my own free will using WinForms in 2026 - I wouldn't believe them, but here we are
- [**Ude.NET**](https://www.nuget.org/packages/Ude.NET): detect encoding
- [**Panlingo.LanguageIdentification.Whatlang**](https://www.nuget.org/packages/Panlingo.LanguageIdentification.Whatlang): detect encoding if Ude.NET fails 😝

### Architecture
- **Monolith**: We are talking tiny desktop app with handful of features, come on
- **Code in Event Handlers**: After 15 years of following sacred practices, sticking to MVVM and never sniffing WinForms code, it turns out that if you bring these best practices back into WinForms - it ain't that bad as you remember (i.e. still supportable and extendable). These code behind files which WinForms creates for you - basically act as VM, there is veeeery little overhead in comparison to using binding.
- **Synchronous Code**: I/O is synchronous, so it will freeze. But have you ever tried to load 1 MB txt file into Notepad on Win XP? It froze shamelessly, because it wasn't designed for that. F-Pad isn't designed for that either, although on modern hardware it's more like 100 MB is what would cause such problems, not 1 MB.

## Project Structure

```
FPad/                  # The app
├── MainWindow.cs      # Primary UI and application logic
├── Controls/          # Custom UI controls, if any
├── Encodings/         # Stuff related to encoding support
├── ExternalEditors/   # Integration with popular text editors
├── Interaction/       # Inter-process communication between running instances of F-Pad within one PC (did someone say it was supposed to be minimalistic and dead simple text editor?)
├── Resources/         # Media files which are packed into exe
└── Settings/          # Remembering state of the app

Helper/                # Serves for experiments with code and for one-time actions such as making icons out of png, etc.
Tests.FPad/            # Unit tests
```

## Philosophy

My thoughts on possible features and change requests:
- **Syntax highlighting, tabs/spaces, indentation**: No. F-Pad is not for coding. Please use Notepad++.
- **Async code and loading indication**: Realistically on modern hardware the app won't ever freeze, so I don't see a point in implementing something that will never be used. For large files most probably TextBox (UI control) would be the weakest point, and you cannot put loading on that. But I would anyway use something more serious than Notepad for large files. If some other long action will appear - I would migrate to async code, but no need for now.
- **Another UI framework?** I gave up all possible styling and customization, but when video driver on my laptop crashes - and drags to grave all running WPF apps along with it - WinForms apps don't crash! If seriously, I wanted this project finished FAST, and while WinForms provides you with some standardized look which is somewhat aligned with your current Windows theme - WPF forces you to customize your app from ground up or it will be ugly as hell.
- **Display special characters correctly?** It would be nice. But certainly not using plain TextBox :) So, maybe.
- **More encodings?** Yes. If someone really uses it - I would add. By now it has already more than I've ever needed.
- **Right-to-Left writing systems?** No. I lack competence and have no experience with it. I come from country which uses cyrillic alphabet, so I have a lot of experience how text files saved on one PC are open as gibberish on another PC, and therefore I know how to do it right. With R-t-L writing I don't know how to do it right.
- **Different languages for UI**: I would love to do it, just let me know.
- **Publish as EXE?** I have to sign it for that, otherwise Win11 will prevent the app from starting. Signing is expensive. If only I will end up using the app - I won't do it.


## Acknowledgments

- [**Whatlang**](https://www.nuget.org/packages/Panlingo.LanguageIdentification.Whatlang) for language detection
- [**Ude.NET**](https://www.nuget.org/packages/Ude.NET) for encoding detection
- [**Icons8**](https://icons8.com) for amazing icons which bring awesomeness to every app I make
- **Win 11 and its new Notepad** without which this project wouldn't have existed 

---

*F-Pad: Light-weight and stupid most basic text editor imaginable for Windows.*