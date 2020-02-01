using Funny.Accents.Core.Reflection.Interfaces;

namespace Funny.Accents.Core.Reflection.Attribues
{
    public class AttributeDetails : IAttributeDetails
    {
        public string AttributePropertyName { get; set; }
        public bool IsInitializedByConstructor { get; set; }
        public int Position { get; set; }
        public object Value { get; set; }
        public string ValueType { get; set; }
    }/*End of AttributeDetails class*/
}/*End of Funny.Accents.Core.Reflection.Attribues namespace*/
