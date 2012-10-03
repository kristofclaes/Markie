using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Markie.Infrastructure
{
    public interface ISlugifier
    {
        string Slugify(string input);
    }

    public class Slugifier : ISlugifier
    {
        public string Slugify(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return String.Empty;
            }

            var output = Transliterate(input);
            output = output.ToLower();
            output = ReplaceInvalidCharactersWithDashes(output);
            output = output.Trim('-');
            output = RemoveDoubleDashes(output);

            return output;
        }

        /// <summary>
        /// Transliterates the string. Replaces 'é' with 'e', 'à' with 'a', 'ø' with o, ...
        /// </summary>
        /// <param name="input">The string to transliterate</param>
        /// <returns>A transliterated string</returns>
        private string Transliterate(string input)
        {
            return Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(input));
        }

        /// <summary>
        /// Replaces everything that's not 'a-z', '0-9' or '-' with a '-'
        /// </summary>
        /// <param name="input">The string to clean</param>
        /// <returns>A cleaned string</returns>
        private string ReplaceInvalidCharactersWithDashes(string input)
        {
            return Regex.Replace(input, "[^a-z0-9-]+", "-", RegexOptions.Compiled);
        }

        /// <summary>
        /// Makes sure the string doesn't contain double, triple, ... dashes
        /// </summary>
        /// <param name="input">The string to clean</param>
        /// <returns>A cleaned string</returns>
        private string RemoveDoubleDashes(string input)
        {
            return Regex.Replace(input, "--+", "-", RegexOptions.Compiled);
        }
    }
}