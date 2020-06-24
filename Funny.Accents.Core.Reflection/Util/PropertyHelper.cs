using System;
using System.Reflection;

namespace Funny.Accents.Core.Reflection.Util
{
    public class PropertyHelper
    {
        public static void SetValue(object inputObject, string propertyName, object propertyVal)
        {
            Type type = inputObject.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            Type propertyType = propertyInfo.PropertyType;
            var targetType = IsNullableType(propertyInfo.PropertyType)
                ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)
                : propertyInfo.PropertyType;
            propertyVal = Convert.ChangeType(propertyVal, targetType);
            propertyInfo.SetValue(inputObject, propertyVal, null);
        }/*End of SetValue method*/

        public static void SetObjectValue<SourceObject, DestinationObj>(
            SourceObject sourceObject, DestinationObj destinationObj,
            bool includeNullValues = false)
        {
            Type type = typeof(SourceObject);

            foreach (PropertyInfo pi in type.GetProperties())
            {
                object value = pi.GetValue(sourceObject, null);

                if (includeNullValues)
                {
                    SetValue(destinationObj, pi.Name, value);
                }
                else if (includeNullValues == false && value != null)
                {
                    SetValue(destinationObj, pi.Name, value);
                }
            }
        }/*End of SetObjectValue method*/

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType
                && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }/*End of IsNullableType method*/
    }/*End of PropertyHelper class*/
}/*End of Funny.Accents.Core.Reflection.Util namespace*/
