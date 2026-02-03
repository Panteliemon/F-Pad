using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FPad;

public static class StringUtils
{
    private static SHA256 sha256;

    public static string GetPathHash(string fullPath)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(fullPath.ToUpperInvariant().Normalize());
        sha256 ??= SHA256.Create();
        byte[] hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
