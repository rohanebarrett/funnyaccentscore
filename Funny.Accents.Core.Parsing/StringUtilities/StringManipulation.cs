using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Funny.Accents.Core.Core.Parsing.StringUtilities
{
    public static class StringManipulation
    {
        private const string LongStringBreakSplitPoint = @"@_splitLocation";

        public static List<string> LongStringBreak(string stringValue, int stringMaxLength
            , string splitDelimiter = null)
        {
            var closest = 0;

            /* Check to see if:
             * 1) the string value is less than the max length
             * 2) a delimiter was supplied; if none then use the max string lenght to split the string
             */
            if (stringValue.Length < stringMaxLength) { return new List<string>() { stringValue }; }
            if (splitDelimiter == null)
            {
                return stringValue.Insert(stringMaxLength, Environment.NewLine)
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            }

            /* Retrieve the position of the delimiter in the string that is:
             * 1) closest to the max string length
             * 2) less than the max string length
             */
            var expr = new Regex(splitDelimiter);
            MatchCollection matches = expr.Matches(stringValue);

            var matchList = matches.Cast<Match>().Select(match => match.Index).ToList();
            if (matchList.Count > 0)
            {
                closest = matchList.Aggregate((x, y) => Math.Abs(x - stringMaxLength)
                                                        < Math.Abs(y - stringMaxLength)
                    ? x < stringMaxLength ? x : y
                    : y < stringMaxLength ? y : x);
            }
            else
            {
                closest = stringMaxLength;
            }

            /* 1) insert a delimiter (@_splitLocation) at the position where you want to split the string
             * 2) split the string on the delimiter
             * 3) trim the leading and trailing spaces for the newly split string values
             * 4) Call the method again to check if any of the split strings were still greater than the
             *    specified max string limit
             */
            var newStringList = new List<string>();
            stringValue.Insert(closest, LongStringBreakSplitPoint)
                .Split(new[] { LongStringBreakSplitPoint }, StringSplitOptions.None)
                .ToList().ForEach(p =>
                {
                    newStringList.AddRange(LongStringBreak(p.Trim(), stringMaxLength, splitDelimiter));
                });

            return newStringList; ;
        }

        public static string SafeSubstring(string originalString, int startPosition, int endPosition)
        {
            if (string.IsNullOrWhiteSpace(originalString)) { return null; }

            try
            {
                return originalString.Length > endPosition - startPosition
                    ? originalString.Substring(startPosition, endPosition)
                    : originalString;
            }
            catch (Exception)
            {
                return null;
            }
        }/*End of SafeSubstring method*/

        public static string ReplaceUriValues(string stringValue,
            Dictionary<string, string> stringReplacement)
        {
            if (stringReplacement == null) { return string.Empty; }

            var updatedString = stringValue;

            foreach (var keyValuePair in stringReplacement)
            {
                updatedString = updatedString.Replace(keyValuePair.Key, keyValuePair.Value);
            }

            return updatedString;
        }

        public static string PathCombine(string pathBase,
            char separator = '/', params string[] paths)
        {
            if (paths == null
                || paths.All(string.IsNullOrWhiteSpace)) { return pathBase; }

            var slash = new[] { '/', '\\' };

            void RemoveLastSlash(StringBuilder sb)
            {
                while (true)
                {
                    if (sb.Length == 0) { return; }
                    if (!slash.Contains(sb[sb.Length - 1])) { return; }
                    sb.Remove(sb.Length - 1, 1);
                }
            }

            var pathSb = new StringBuilder();
            pathSb.Append(pathBase);
            RemoveLastSlash(pathSb);
            foreach (var path in paths)
            {
                if (string.IsNullOrWhiteSpace(path)) { continue; }

                /*Avoid separator acting as an escape character*/
                if (!slash.Contains(path.First()))
                {
                    pathSb.Append(separator);
                }

                pathSb.Append(path);
                RemoveLastSlash(pathSb);
            }

            if (slash.Contains(paths.Last().Last()))
            {
                pathSb.Append(separator);
            }

            return pathSb.ToString();
        }

        public static string ReplacePlaceholder(string stringValue, string startDelimiter,
            string endDelimiter, string valueDelimiter,
            Func<string, string> replacementValue)
        {
            var endSplit = stringValue.Split(new[] { endDelimiter, startDelimiter }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(p => p.Contains(valueDelimiter))
                    .ToList();

            var newStringVal = stringValue;
            if (endSplit.Count == 1 && endSplit[0].Equals(stringValue)) { return newStringVal; }

            foreach (var item in endSplit)
            {
                var dateFormat = item.Split(new[] { valueDelimiter }, StringSplitOptions.RemoveEmptyEntries)[0];
                newStringVal = newStringVal.Replace(startDelimiter + item + endDelimiter, replacementValue.Invoke(DateTime.Now.ToString(dateFormat)));
            }

            return newStringVal;
        }
    }/*End of StringManipulation class*/
}/*End of CmkParsingUtilities.StringUtilities*/