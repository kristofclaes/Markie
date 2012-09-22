namespace System
{
    public static class StringExtensionMethods
    {
        public static bool IsNullOrEmpty(this string input)
        {
            return String.IsNullOrEmpty(input);
        }

        public static bool IsNullOrWhiteSpace(this string input)
        {
            return String.IsNullOrWhiteSpace(input);
        }

        public static bool IsValidEmailAddress(this string input)
        {
            const string pattern =
                @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" +
                @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var re = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return re.IsMatch(input);
        }

        public static bool IsInteger(this string input)
        {
            int temp;
            return Int32.TryParse(input, out temp);
        }

        public static int AsInteger(this string input)
        {
            return Int32.Parse(input);
        }

        public static int? AsNullableInteger(this string input)
        {
            int temp;
            if (Int32.TryParse(input, out temp))
            {
                return temp;
            }

            return null;
        }

        public static bool AsBoolean(this string input)
        {
            bool temp;
            return Boolean.TryParse(input, out temp) && temp;
        }
    }
}