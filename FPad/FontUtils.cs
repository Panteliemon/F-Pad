using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPad.Settings;

namespace FPad;

public static class FontUtils
{
    public static FontFamily GetFontFamilyByString(string str, FontFamily[] families)
    {
        if (string.IsNullOrEmpty(str))
        {
            return GetGenericMonospace(families);
        }

        FontFamily result = families.FirstOrDefault(x => x.Name == str);
        if (result != null)
            return result;

        result = families.FirstOrDefault(x => x.Name.Equals(str, StringComparison.InvariantCultureIgnoreCase));
        if (result != null)
            return result;

        result = families.FirstOrDefault(x => x.Name.StartsWith(str));
        if (result != null)
            return result;

        result = families.FirstOrDefault(x => x.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase));
        if (result != null)
            return result;

        result = families.FirstOrDefault(x => x.Name.Contains(str));
        if (result != null)
            return result;

        result = families.FirstOrDefault(x => x.Name.Contains(str, StringComparison.InvariantCultureIgnoreCase));
        if (result != null)
            return result;

        return GetGenericMonospace(families);
    }

    private static FontFamily GetGenericMonospace(FontFamily[] families)
    {
        FontFamily genericMonospace = FontFamily.GenericMonospace;
        FontFamily result = families.FirstOrDefault(x => x.Name == genericMonospace.Name);
        return result ?? genericMonospace;
    }

    public static Font GetFontByParameters(FontFamily fontFamily, int fontSize, bool isBold, bool isItalic)
    {
        FontStyle fontStyle = FontStyle.Regular;
        if (isBold)
            fontStyle |= FontStyle.Bold;
        if (isItalic)
            fontStyle |= FontStyle.Italic;

        Font result = new Font(fontFamily, fontSize, fontStyle);
        return result;
    }

    public static Font GetFontBySettings(AppSettings settings)
    {
        FontFamily fontFamily = GetFontFamilyByString(App.Settings.FontFamily, FontFamily.Families);
        return GetFontByParameters(fontFamily, settings.FontSize, settings.IsBold, settings.IsItalic);
    }

    public static Font ToBold(this Font font)
    {
        if (font.Bold)
            return font;
        else
            return new Font(font.FontFamily, font.Size, font.Style | FontStyle.Bold, font.Unit);
    }

    public static Font Unbold(this Font font)
    {
        if (font.Bold)
            return new Font(font.FontFamily, font.Size, font.Style & ~FontStyle.Bold, font.Unit);
        else
            return font;
    }
}
