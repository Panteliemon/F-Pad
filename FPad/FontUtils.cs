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
    /// <summary>
    /// </summary>
    /// <param name="str"></param>
    /// <param name="families"></param>
    /// <param name="category">Fallback value if not found by name</param>
    /// <returns></returns>
    public static FontFamily GetFontFamilyByString(string str, FontFamily[] families, FontCategory category)
    {
        if (string.IsNullOrEmpty(str))
        {
            return GetGeneric(families, category);
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

        return GetGeneric(families, category);
    }

    private static FontFamily GetGeneric(FontFamily[] families, FontCategory category)
    {
        FontFamily genericFamily = category switch
        {
            FontCategory.Monospace => FontFamily.GenericMonospace,
            _ => FontFamily.GenericSerif
        };

        FontFamily result = families.FirstOrDefault(x => x.Name == genericFamily.Name);
        return result ?? genericFamily;
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

    /// <summary>
    /// </summary>
    /// <param name="fontSettings"></param>
    /// <param name="category">Fallback value if settings don't contain info about font.</param>
    /// <returns></returns>
    public static Font GetFontBySettings(FontSettings fontSettings, FontCategory category)
    {
        FontFamily fontFamily = GetFontFamilyByString(fontSettings.Family, FontFamily.Families, category);
        return GetFontByParameters(fontFamily, fontSettings.Size, fontSettings.IsBold, fontSettings.IsItalic);
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
