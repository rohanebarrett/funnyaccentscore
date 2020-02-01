using System;
using System.Collections.Generic;

namespace Funny.Accents.Core.Reflection.Interfaces
{
    public interface IMemberDetails
    {
        string PropertyName { get; set; }
        object PropertyValue { get; set; }
        List<IAttributeDetails> AttributeDetails { get; set; }
    }/*End of IMemberDetails interface*/

    public interface IMemberDetails<T>
        where T : Attribute
    {
        string PropertyName { get; set; }
        object PropertyValue { get; set; }
        T AttributeDetails { get; set; }
    }/*End of IMemberDetails interface*/
}/*End of Funny.Accents.Core.Reflection.Interfaces namespace*/
