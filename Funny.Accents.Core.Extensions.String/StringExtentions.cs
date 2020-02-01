using System;

namespace Funny.Accents.Core.Extensions.String
{
    public static class StringExtentions
    {
        public enum Direction
        {
            Back = 0,
            Current = 1,
            Forward = 2
        };

        public static string SurroundWith(this string text, string ends)
        {
            return ends + text + ends;
        }/*End of SurroundWith method*/

        public static string ToProperCase(this string the_string, Direction direction)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null) return the_string;
            if (the_string.Length < 2) return the_string.ToUpper();

            // Start with the first character.
            string result = the_string.Substring(0, 1).ToUpper();

            // Add the remaining characters.
            for (int i = 1; i < the_string.Length; i++)
            {
                switch (direction)
                {
                    case Direction.Back:
                        if (char.IsUpper(the_string[i])
                            && char.IsLower(the_string[i - 1]))
                        {
                            result += " ";
                        }
                        break;
                    case Direction.Current:
                        if (char.IsUpper(the_string[i]))
                        {
                            result += " ";
                        }
                        break;
                    case Direction.Forward:
                        if (char.IsUpper(the_string[i])
                            && i + 1 < the_string.Length
                            && char.IsLower(the_string[i + 1]))
                        {
                            result += " ";
                        }
                        break;
                    default:
                        break;
                }

                result += the_string[i];
            }

            return result;
        }/*End of ToProperCase method*/

        public static string ToCamelCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null || the_string.Length < 2)
                return the_string;

            // Split the string into words.
            string[] words = the_string.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }/*End of ToCamelCase method*/

        public static string ToPascalCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null) return the_string;
            if (the_string.Length < 2) return the_string.ToUpper();

            // Split the string into words.
            string[] words = the_string.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = "";
            foreach (string word in words)
            {
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);
            }

            return result;
        }/*End of ToPascalCase method*/
    }/*End of  class*/
}/*End of Funny.Accents.Core.Extensions.String namespace*/
