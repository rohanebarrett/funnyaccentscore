using System;
using System.Collections.Generic;
using Funny.Accents.Core.Reflection.Interfaces;

namespace Funny.Accents.Core.Reflection.Attribues
{
    public class MemberDetails : IMemberDetails
    {
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
        public List<IAttributeDetails> AttributeDetails { get; set; }
    } /*End of MemberDetails class*/

    public class MemberDetails<T> : IMemberDetails<T>
        where T : Attribute
    {
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
        public T AttributeDetails { get; set; }
    } /*End of MemberDetails class*/
}/*end of Funny.Accents.Core.Reflection.Attribues namespace*/
