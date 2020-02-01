using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Primitives;

namespace Funny.Accents.Core.Core.Parsing.ParsingUtilities
{
    public static class ParsingUtil
    {
        private const string StringBlank = "";

        public static List<string> SplitByDelimiter(List<string> stringValueParam, string delimiterParam)
        {
            string[] delimiterArray = { delimiterParam };

            var splitItemList = new List<string>();
            stringValueParam.ForEach(p =>
            {
                p.Split(delimiterArray, StringSplitOptions.None).ToList().ForEach(px =>
                {
                    splitItemList.Add(px);
                });
            });

            return splitItemList;
        }/*End of splitByDelimiter method*/

        public static List<string> RemovePattern(List<string> stringValueParam, params string[] stringToReplaceParam)
        {
            var replacedValuesList = new List<string>();
            stringValueParam.ForEach(p =>
            {
                var replacedString = p;
                stringToReplaceParam.ToList().ForEach(px =>
                {
                    replacedString = replacedString.Replace(px, StringBlank);
                });

                replacedValuesList.Add(replacedString);
            });

            return replacedValuesList;
        }

        public static Dictionary<string, List<string>> GetQueryStringValues(
            IEnumerable<KeyValuePair<string, StringValues>> queryStringCollectionParam, bool removeDuplicatesParam)
        {
            var queryDictionary = new Dictionary<string, List<string>>();

            foreach (var keyValue in queryStringCollectionParam)
            {
                if (!queryDictionary.ContainsKey(keyValue.Key))
                {
                    queryDictionary.Add(keyValue.Key,
                        removeDuplicatesParam ? keyValue.Value.Distinct().ToList() : keyValue.Value.ToList());
                }
            }

            return queryDictionary;
        }/*End of getQueryStringValues method*/

        public static Dictionary<string, List<string>> GetQueryStringValues(List<string> queryKeysParam
            , Dictionary<string, StringValues> queryStringCollectionParam, bool removeDuplicatesParam = true)
        {
            var queryDictionary = new Dictionary<string, List<string>>();

            queryKeysParam.ForEach(p =>
            {
                try
                {
                    queryStringCollectionParam.TryGetValue(p, out var values);

                    if (!queryDictionary.ContainsKey(p) && values.Count > 0)
                    {
                        queryDictionary.Add(p,
                            removeDuplicatesParam ? values.Distinct().ToList() : values.ToList());
                    }
                }
                catch (ArgumentNullException)
                {
                    queryDictionary.Add(p, null);
                }
            });

            return queryDictionary;
        }/*End of getQueryStringValues method*/

        public static Dictionary<string, string> ParseQueryString(string requestQueryString)
        {
            Dictionary<string, string> rc = new Dictionary<string, string>();
            string[] ar1 = requestQueryString.Split(new char[] { '&', '?' });
            foreach (string row in ar1)
            {
                if (string.IsNullOrEmpty(row)) continue;
                int index = row.IndexOf('=');
                if (index < 0) continue;
                rc[Uri.UnescapeDataString(row.Substring(0, index))] = Uri.UnescapeDataString(row.Substring(index + 1)); // use Unescape only parts          
            }
            return rc;
        }/*End of ParseQueryString method*/

        public static string GetHeaderValue(string headerKeyParam, IDictionary<string, StringValues> headerDictionaryParam)
        {
            foreach (var pair in headerDictionaryParam)
            {
                if (pair.Key.Equals(headerKeyParam))
                {
                    return pair.Value;
                }
            }
            return "";
        }/*End of getHeaderValue method*/

        public static IEnumerable<string> DelimitClassData<T>(string separator, IEnumerable<T> objectlist
            , List<string> ignoreList = null)
        {
            var separatorVarified = int.TryParse(separator, out var fileDelimiterChar)
                ? new string(new char[] { (char)fileDelimiterChar })
                : separator;

            FieldInfo[] fields = ignoreList == null || ignoreList.Count <= 0
                ? typeof(T).GetFields()
                : typeof(T).GetFields().Where(f => ignoreList.Contains(f.Name)).ToArray();
            PropertyInfo[] properties = ignoreList == null || ignoreList.Count <= 0
                ? typeof(T).GetProperties()
                : typeof(T).GetProperties().Where(f => ignoreList.Contains(f.Name)).ToArray();

            yield return string.Join(separatorVarified, fields.Select(f => f.Name).Concat(properties.Select(p => p.Name)).ToArray());
            foreach (var o in objectlist)
            {
                yield return string.Join(separatorVarified, fields.Select(f => (f.GetValue(o) ?? "").ToString())
                    .Concat(properties.Select(p => (p.GetValue(o, null) ?? "").ToString())).ToArray());
            }
        }/*End of DelimitClassData class*/
    }/*End of ParsingUtil Class*/
}/*End of CmkParsingUtilities.ParsingUtilities*/