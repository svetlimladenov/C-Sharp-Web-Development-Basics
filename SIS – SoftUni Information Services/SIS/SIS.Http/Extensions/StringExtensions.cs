using System;

namespace SIS.Http.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string toBeCapitalized)
        {
            if (string.IsNullOrEmpty(toBeCapitalized))
            {
                throw new ArgumentException("Empty string cannot be capitalized");
            }
            var loweredString = toBeCapitalized.ToLower();
            var firstLetterCapitalized = Char.ToUpper(loweredString[0]);

            return firstLetterCapitalized + loweredString.Substring(1);

        }
    }
}
