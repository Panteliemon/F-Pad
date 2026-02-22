using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

public struct Selection : IEquatable<Selection>
{
    public int Start { get; }
    public int Length { get; }
    public int End
    {
        get => Start + Length;
    }

    public Selection(int start, int length)
    {
        Start = start;
        Length = length;
    }

    public static Selection FromStartEnd(int start, int end)
    {
        return new Selection(start, end - start);
    }

    public bool Equals(Selection other)
    {
        return (Start == other.Start) && (Length == other.Length);
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is Selection sel)
            return Equals(sel);
        else
            return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, Length);
    }

    public static bool operator ==(Selection x, Selection y)
    {
        return x.Equals(y);
    }

    public static bool operator !=(Selection x, Selection y)
    {
        return !x.Equals(y);
    }
}

public static class SelectionExtensions
{
    public static string SubString(this string str, Selection selection)
    {
        return str.Substring(selection.Start, selection.Length);
    }
}
