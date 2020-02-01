using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Funny.Accents.Core.Serialization.Utilities
{
    public static class CsvHelper
    {
        public static string TypeToCsv<T>(IEnumerable<T> items, char delimiter,
            bool isHeaderIncluded = false, string[] replaceHeaders = null, Func<object, string> modifyData = null)
            where T : class
        {
            if (items == null) { return ""; }

            string output;
            var properties = GetProperties<T>();

            /*Check if the calling application wants to modify the data*/
            modifyData = modifyData ?? (n => n == null ? "" : n.ToString());

            using (var sw = new StringWriter())
            {
                /*Add headers*/
                var propertyInfos = properties as PropertyInfo[] ?? properties.ToArray();
                if (isHeaderIncluded)
                {
                    if (replaceHeaders != null)
                    {
                        var header = ConcatenateByDelimiter(replaceHeaders, delimiter, n => n);
                        sw.WriteLine(header);
                    }
                    else
                    {
                        var header = ConcatenateByDelimiter(propertyInfos, delimiter, n => n.Name);
                        sw.WriteLine(header);
                    }
                }

                /*Add the main content body*/
                foreach (var item in items)
                {
                    var row = ConcatenateByDelimiter(propertyInfos, values =>
                    {
                        return values
                            .Select(n => n.GetValue(item, null))
                            .Select(modifyData)
                            .Select(n =>
                            {
                                /*If the string values contains a comma(,) surround it with quotes*/
                                var itemValue = n ?? "";
                                return itemValue.Contains(@",")
                                    ? $"\"{itemValue}\""
                                    : itemValue;
                            });
                    }, delimiter);
                    sw.WriteLine(row);
                }
                output = sw.ToString();
            }
            return output;
        }/*End of TypeToCsv method*/

        private static string ConcatenateByDelimiter<T>(IEnumerable<T> items,
            char delimiter, Func<T, string> predicate)
        {
            var delimitedResults = items
                .Select(predicate)
                .Aggregate((a, b) => a + delimiter + b);
            return delimitedResults;
        }/*End of ConcatenateByDelimiter*/

        private static string ConcatenateByDelimiter<T>(IEnumerable<T> items
            , Func<IEnumerable<T>, IEnumerable<string>> predicate, char delimiter)
        {
            var delimitedResults = predicate.Invoke(items)
                .Aggregate((a, b) => a + delimiter + b);
            return delimitedResults;
        }/*End of ConcatenateByDelimiter*/

        private static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            var properties = typeof(T).GetProperties()
                .Where(n =>
                    n.PropertyType == typeof(string)
                    || n.PropertyType == typeof(bool)
                    || n.PropertyType == typeof(bool?)
                    || n.PropertyType == typeof(char)
                    || n.PropertyType == typeof(char?)
                    || n.PropertyType == typeof(byte)
                    || n.PropertyType == typeof(byte?)
                    || n.PropertyType == typeof(decimal)
                    || n.PropertyType == typeof(decimal?)
                    || n.PropertyType == typeof(short)
                    || n.PropertyType == typeof(short?)
                    || n.PropertyType == typeof(int)
                    || n.PropertyType == typeof(int?)
                    || n.PropertyType == typeof(long)
                    || n.PropertyType == typeof(long?)
                    || n.PropertyType == typeof(DateTime)
                    || n.PropertyType == typeof(DateTime?));
            return properties;
        }/*End of GetProperties*/
    }/*End of CsvHelper class*/
}/*End of Funny.Accents.Core.Serialization.Utilities namespace*/
