using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Funny.Accents.Core.Reflection.Attribues;
using Funny.Accents.Core.Reflection.Interfaces;

namespace Funny.Accents.Core.Reflection.Services
{
    internal static class ReflectionUtil
    {
        private static List<IAttributeDetails> ExtractConstructorArgumentsAttr(
            IEnumerable<CustomAttributeTypedArgument> consArguments)
        {
            var position = 1;
            var attributes =
                consArguments
                    .Select(attr => new AttributeDetails
                    {
                        AttributePropertyName = string.Empty,
                        IsInitializedByConstructor = true,
                        Position = position++,
                        Value = attr.Value,
                        ValueType = attr.ArgumentType.FullName
                    }).ToList();

            var convertedAttributes = new List<IAttributeDetails>();
            convertedAttributes.AddRange(attributes);
            return convertedAttributes;
        }

        private static List<IAttributeDetails> ExtractNamedArgumentsAttr(
            IEnumerable<CustomAttributeNamedArgument> namedArguments)
        {
            var position = 1;
            var attributes =
                namedArguments
                    .Where(p => !string.IsNullOrWhiteSpace(p.MemberName))
                    .Select(attr => new AttributeDetails
                    {
                        AttributePropertyName = attr.MemberName,
                        IsInitializedByConstructor = false,
                        Position = position++,
                        Value = attr.TypedValue.Value,
                        ValueType = attr.TypedValue.ArgumentType.FullName
                    }).ToList();

            var convertedAttributes = new List<IAttributeDetails>();
            convertedAttributes.AddRange(attributes);
            return convertedAttributes;
        }

        internal static IEnumerable<IMemberDetails> GetMemberDetails<T>(T data)
            where T : class
        {
            var memberDetails = new List<IMemberDetails>();
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                if (!prop.GetCustomAttributes().Any()) { continue; }

                var conAttributes = ExtractConstructorArgumentsAttr(prop.CustomAttributes
                    .SelectMany(p => p.ConstructorArguments));

                var namedAttributes = ExtractNamedArgumentsAttr(prop.CustomAttributes
                    .SelectMany(p => p.NamedArguments));

                if (conAttributes == null && namedAttributes == null) { continue; }

                memberDetails.Add(new MemberDetails
                {
                    PropertyName = prop.Name,
                    PropertyValue = prop.GetValue(data, null),
                    AttributeDetails
                        = conAttributes?.Concat(namedAttributes)
                            .ToList()
                });
            }

            return memberDetails;
        }

        internal static IEnumerable<MemberDetails<TA>> GetMemberDetailsByType<T, TA>(T data)
            where T : class
            where TA : Attribute
        {
            var memberDetails = new List<MemberDetails<TA>>();
            var props = typeof(T).GetProperties();
            var attributeType = typeof(TA);

            foreach (var prop in props)
            {
                if (!prop.GetCustomAttributes(attributeType).Any()) { continue; }

                memberDetails.AddRange(prop.GetCustomAttributes(attributeType)
                    .Select(attr => new MemberDetails<TA>
                    {
                        PropertyName = prop.Name,
                        PropertyValue = prop.GetValue(data, null),
                        AttributeDetails = (TA)attr
                    }));
            }

            return memberDetails;
        }
    }/*End of ReflectionUtil class*/
}/*End of Funny.Accents.Core.Reflection.Services namespace*/
