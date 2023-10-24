using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Sisa.Extensions;

public static class StringExtensions
{
    public static string ToSnakeCase(this string input, bool useUpperCase = false)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var startUnderscores = Regex.Match(input, @"^_+");
        var slug = startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2");

        if (!useUpperCase)
            return slug.ToLower();

        return slug.ToUpper();
    }

    public static string ToCamelCase(this string input)
    {
        var text = input.Replace("_", "");

        if (text.Length == 0) return string.Empty;

        text = Regex.Replace(text, "([A-Z])([A-Z]+)($|[A-Z])",
            m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);

        text = char.ToLower(text[0]) + text[1..];

        return text;
    }

    public static string ToSlug(this string text)
    {
        string slug = RemoveAccent(RemoveDiacritics(text)); // Remove Accent & Diacritics chars

        slug = slug.ToLower();                              // Lowercase
        slug = Regex.Replace(slug, @"\s+", " ").Trim();     // convert multiple spaces into one space
        slug = Regex.Replace(slug, @"\s", "-");             // hyphens

        return slug;
    }

    public static string ToASCII(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        string slug = RemoveAccent(RemoveDiacritics(text)); // Remove Accent & Diacritics chars

        slug = Regex.Replace(slug, @"\s+", " ").Trim();     // convert multiple spaces into one space

        return slug;
    }

    private static string RemoveAccent(this string text)
    {
        byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(text);

        return Encoding.ASCII.GetString(bytes);
    }

    private static string RemoveDiacritics(this string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormKD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

            switch (unicodeCategory)
            {
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.OtherLetter:
                case UnicodeCategory.DecimalDigitNumber:
                    // Keep letters and digits
                    stringBuilder.Append(c);
                    break;
                case UnicodeCategory.NonSpacingMark:
                    // Remove diacritics
                    break;
                default:
                    // Replace all other chars with dash
                    stringBuilder.Append(' ');
                    break;
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static bool Like(this string value, string pattern)
    {
        int patternIndex = 0;
        int valueIndex = 0;
        int patternLength = pattern.Length;
        int valueLength = value.Length;

        while (valueIndex < valueLength && patternIndex < patternLength)
        {
            if (pattern[patternIndex] == '%')
            {
                // Handle '%' as a wildcard (matches any sequence of characters).
                patternIndex++;
                if (patternIndex == patternLength)
                {
                    // If '%' is at the end of the pattern, it matches anything remaining in the value.
                    return true;
                }

                char nextChar = pattern[patternIndex];

                while (valueIndex < valueLength && value[valueIndex] != nextChar)
                {
                    valueIndex++;
                }
            }
            else if (pattern[patternIndex] == '_')
            {
                // Handle '_' as a wildcard (matches any single character).
                patternIndex++;
                valueIndex++;
            }
            else if (pattern[patternIndex] == value[valueIndex])
            {
                patternIndex++;
                valueIndex++;
            }
            else
            {
                return false; // Characters don't match.
            }
        }

        // If we reached the end of both the value and the pattern, it's a match.
        if (valueIndex == valueLength && patternIndex == patternLength)
        {
            return true;
        }

        // If we didn't reach the end of the pattern, it's not a match.
        return false;
    }

    public static bool ILike(this string value, string pattern)
    {
        int patternIndex = 0;
        int valueIndex = 0;
        int patternLength = pattern.Length;
        int valueLength = value.Length;

        while (valueIndex < valueLength && patternIndex < patternLength)
        {
            if (pattern[patternIndex] == '%')
            {
                // Handle '%' as a wildcard (matches any sequence of characters).
                patternIndex++;
                if (patternIndex == patternLength)
                {
                    // If '%' is at the end of the pattern, it matches anything remaining in the value.
                    return true;
                }

                char nextChar = pattern[patternIndex];

                while (valueIndex < valueLength && char.ToLower(value[valueIndex]) != char.ToLower(nextChar))
                {
                    valueIndex++;
                }
            }
            else if (pattern[patternIndex] == '_')
            {
                // Handle '_' as a wildcard (matches any single character).
                patternIndex++;
                valueIndex++;
            }
            else if (char.ToLower(pattern[patternIndex]) == char.ToLower(value[valueIndex]))
            {
                patternIndex++;
                valueIndex++;
            }
            else
            {
                return false; // Characters don't match.
            }
        }

        // If we reached the end of both the value and the pattern, it's a match.
        if (valueIndex == valueLength && patternIndex == patternLength)
        {
            return true;
        }

        // If we didn't reach the end of the pattern, it's not a match.
        return false;
    }
}
