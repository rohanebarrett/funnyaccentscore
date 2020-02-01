using System;
using System.Collections.Generic;
using Funny.Accents.Core.Reflection.Interfaces;

namespace Funny.Accents.Core.Reflection.Services
{
    public static class AttributeReflector
    {
        /*testing generic form to construct a type from attributes*/
        public static TR ConstructTypeByMemberDetails<T, TR>(T data,
            Func<IEnumerable<IMemberDetails>, TR> construct)
            where T : class
        {
            var attItems = ReflectionUtil.GetMemberDetails(data);
            var constructedType = construct(attItems);
            return constructedType;
        }

        /*testing generic form to construct a type from attributes*/
        public static TR ConstructTypeByMemberDetails<T, TA, TR>(T data,
            Func<IEnumerable<IMemberDetails<TA>>, TR> construct)
            where T : class
            where TA : Attribute
        {
            var attItems = ReflectionUtil.GetMemberDetailsByType<T, TA>(data);
            var constructedType = construct(attItems);
            return constructedType;
        }

        public static IEnumerable<IMemberDetails<TA>> GetMemberDetails<T, TA>(T data)
            where T : class
            where TA : Attribute
        {
            var attItems = ReflectionUtil.GetMemberDetailsByType<T, TA>(data);
            return attItems;
        }

        public static IEnumerable<IMemberDetails> GetMemberDetails<T>(T data)
            where T : class
        {
            var attItems = ReflectionUtil.GetMemberDetails(data);
            return attItems;
        }
    }/*End of AttributeReflector*/
}/*End of Funny.Accents.Core.Reflection.Services namespace*/
