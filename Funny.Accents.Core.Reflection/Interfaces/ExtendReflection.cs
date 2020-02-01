using System;
using System.ComponentModel;

namespace Funny.Accents.Core.Reflection.Interfaces
{
    public static class ExtendReflection
    {
        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumeration Value");
            }

            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length <= 0)
            {
                return enumerationValue.ToString();
            }

            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attrs.Length > 0
                ? ((DescriptionAttribute)attrs[0]).Description
                : enumerationValue.ToString();
        }
    }/*End of ExtendReflection class*/
}/*End of Funny.Accents.Core.Reflection.Interfaces namespace*/
