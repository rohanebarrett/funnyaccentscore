using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace Funny.Accents.Core.Conversion.Helpers
{
    public static class ConversionUtil
    {
        public static TK SafeConversion<T, TK>(T value, TK defaultValue = default)
        {
            try
            {
                return (TK)Convert.ChangeType(value, typeof(TK));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }/*End of SafeConversion method*/

        public static (bool status, TK result) SafeConversionWithStatus<T, TK>(T value, TK defaultValue = default)
        {
            try
            {
                TK returnValue = (TK)Convert.ChangeType(value, typeof(TK));
                return (true, returnValue);
            }
            catch (Exception)
            {
                return (true, defaultValue);
            }
        }/*End of SafeConversion method*/

        public static T GetEnumFromString<T>(string value, T defaultValue = default)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) { return defaultValue; }

            try
            {
                foreach (T item in Enum.GetValues(typeof(T)))
                {
                    if (item.ToString(CultureInfo.InvariantCulture).ToLower().Equals(value.Trim().ToLower()))
                    {
                        return item;
                    }
                }
                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }/*End of GetEnumFromString method*/

        public static (bool status, TK result) GetDictionaryVlaue<T, TK>(Dictionary<T, TK> valueColleciton,
            T searchValue, TK defaultValue = default)
        {
            try
            {
                var result = valueColleciton.TryGetValue(searchValue, out TK returnValue);
                return (result, result ? returnValue : defaultValue);
            }
            catch
            {
                return (false, defaultValue);
            }
        }/*End of GetDictionaryVlaue method*/

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }/*End of CreateDataTable method*/
    }/*End of ConversionUtil class*/
}/*End of ConversionUtil namespace*/
