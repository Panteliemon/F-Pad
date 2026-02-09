using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad;

/// <summary>
/// Engine for finding given substring in text.
/// </summary>
public class StringSearch
{
    private string transformedSubStrToSearch;

    // Implements Knuth-Morris-Pratt algorithm

    /// <summary>
    /// For substring <see cref="transformedSubStrToSearch"/>[0..(i+1)]
    /// contains length of a longest prefix which is also a suffix.
    /// pattern == "ababa111" -> longestPrefixSuffix[4] == 3 (substring
    /// "ababa" starts with "aba" and ends with "aba").
    /// Also this suffix/prefix must be shorter than the entire substring.
    /// </summary>
    private int[] longestPrefixSuffix;

    public string SubStringToSearch { get; private set; }
    public bool IsCaseSensitive { get; private set; }

    public StringSearch(string subStringToSearch, bool caseSensitive)
    {
        ArgumentNullException.ThrowIfNull(subStringToSearch);
        if (subStringToSearch.Length == 0)
            throw new ArgumentException("Substring to search must be not empty");

        SubStringToSearch = subStringToSearch;
        IsCaseSensitive = caseSensitive;
        transformedSubStrToSearch = caseSensitive ? subStringToSearch : subStringToSearch.ToUpper(); // ToUpper with current culture

        longestPrefixSuffix = CalculateLongestPrefixSuffixArray(transformedSubStrToSearch);
    }

    /// <summary>
    /// Returns index starting from which the <see cref="SubStringToSearch"/> is found,
    /// or -1 if not found.
    /// This overload doesn't support "whole words only" search.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public int FindFirstMatch(ReadOnlySpan<char> str)
    {
        int posInSubStrToSearch = 0;
        if (IsCaseSensitive)
        {
            for (int i = 0; i < str.Length; i++)
            {
                while (true)
                {
                    if (str[i] == transformedSubStrToSearch[posInSubStrToSearch])
                    {
                        posInSubStrToSearch++;
                        if (posInSubStrToSearch == transformedSubStrToSearch.Length)
                            return i - transformedSubStrToSearch.Length + 1;

                        break;
                    }
                    else
                    {
                        if (posInSubStrToSearch == 0)
                        {
                            // Go next [i]
                            break;
                        }
                        else
                        {
                            // Prefix of SubStr now: "abcdE", str now: "...abcdF..."
                            // If say "ab" == "cd" then to avoid redundant comparisons
                            // we can set posInSubStrToSearch to 2 and retry (compare "abc" vs "cdF").
                            // But '2' would be in this case longestPrefixSuffix("abcd"),
                            // thus longestPrefixSuffix[3].
                            posInSubStrToSearch = longestPrefixSuffix[posInSubStrToSearch - 1];
                            // Pos goes down here bc longestPrefixSuffix(str) is by definition
                            // less than length of str (length and pos are both 4 in our example,
                            // and then pos changes to something which is less than 4).
                        }
                    }
                }
            }
        }
        else
        {
            // Copypaste
            for (int i = 0; i < str.Length; i++)
            {
                while (true)
                {
                    // Hope this doesn't use WinAPI which requires to allocate a whole string for that...
                    if (char.ToUpper(str[i]) == transformedSubStrToSearch[posInSubStrToSearch])
                    {
                        posInSubStrToSearch++;
                        if (posInSubStrToSearch == transformedSubStrToSearch.Length)
                            return i - transformedSubStrToSearch.Length + 1;

                        break;
                    }
                    else
                    {
                        if (posInSubStrToSearch == 0)
                        {
                            break;
                        }
                        else
                        {
                            posInSubStrToSearch = longestPrefixSuffix[posInSubStrToSearch - 1];
                        }
                    }
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns index starting from which the <see cref="SubStringToSearch"/> is found,
    /// or -1 if not found.
    /// </summary>
    /// <param name="str">If <paramref name="matchWholeWords"/> is used,
    /// need to pass whole uncut string, because in that case need to know
    /// where true start/end are, and have to analyze characters
    /// before <paramref name="startPos"/></param>
    /// <param name="startPos">Possible matches with lower starting index
    /// than this one are ignored.</param>
    /// <param name="matchWholeWords"></param>
    /// <returns>Index which is &gt;= <paramref name="startPos"/> and &lt; <paramref name="str"/>.Length if found;
    /// -1 if not found.</returns>
    public int FindFirstMatch(ReadOnlySpan<char> str, int startPos, bool matchWholeWords)
    {
        if (matchWholeWords)
        {
            int currentStartPos = startPos;
            while (true)
            {
                int matchPos = FindFirstMatch(str[currentStartPos..]);
                if (matchPos < 0)
                    return -1;
                matchPos += currentStartPos; // We've cut the string before sub-search, so here we go

                // Whole word?
                if ((matchPos == 0) || !StringUtils.IsPartOfWord(str[matchPos - 1]))
                {
                    int matchEnd = matchPos + transformedSubStrToSearch.Length;
                    if ((matchEnd == str.Length) || !StringUtils.IsPartOfWord(str[matchEnd]))
                    {
                        return matchPos;
                    }
                }

                // If not a whole word - try somewhere else
                currentStartPos = matchPos + 1;
            }
        }
        else
        {
            int matchPos = FindFirstMatch(str[startPos..]);
            if (matchPos < 0)
                return -1;
            return matchPos + startPos;
        }
    }

    #region Private

    internal static int[] CalculateLongestPrefixSuffixArray(string str)
    {
        int[] result = new int[str.Length];
        result[0] = 0;

        // Position moves along chars in the beginning
        // as long as they match current ending.
        int posInPrefix = 0;
        for (int i = 1; i < result.Length; i++)
        {
            while (true)
            {
                // If we meet new matching symbol -
                // increase the length of currently matching prefix by 1.
                // Comparison here works for both case sensitive and case insensitive version.
                if (str[posInPrefix] == str[i])
                {
                    result[i] = posInPrefix + 1;
                    posInPrefix++;
                    break;
                }
                else
                {
                    // Doesn't match and there is no chain of matching symbols
                    // before the current one - legitimate 0.
                    if (posInPrefix == 0)
                    {
                        result[i] = 0;
                        break;
                    }
                    else
                    {
                        // Prefix now: "abcdE", suffix now: "abcdF". "abcd" matches but E/F - doesn't.
                        // See how we can shorten "abcd" part:
                        // If "abc" == "bcd": but that would happen if longestPrefixSuffix("abcd") == 3
                        // If "ab" == "cd": but that would happen if longestPrefixSuffix("abcd") == 2
                        // If "a" == "d": but that would happen if longestPrefixSuffix("abcd") == 1
                        // longestPrefixSuffix("abcd") is longestPrefixSuffix[3].
                        // So each time already evaluated longestPrefixSuffix[posInPrefix - 1]
                        // tells us how many characters before [i] match with the beginning
                        // of the str.
                        posInPrefix = result[posInPrefix - 1];
                        // With that:
                        // 1. Inner cycle is guaranteed to exit because this line
                        // always makes posInPrefix smaller than before.
                        // 2. No need to check whether characters [0..posForPrefix]
                        // equal to [posForPrefix] characters before [i] - we know they are equal.
                        // Now need to repeat checks: whether it continues sequence of matching characters,
                        // or we need even shorter prefix, or no matches at all. Hence the inner cycle.
                    }
                }
            }
        }

        return result;
    }

    #endregion
}
