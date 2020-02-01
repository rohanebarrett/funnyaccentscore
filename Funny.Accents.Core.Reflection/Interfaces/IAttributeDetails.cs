namespace Funny.Accents.Core.Reflection.Interfaces
{
    public interface IAttributeDetails
    {
        string AttributePropertyName { get; set; }
        bool IsInitializedByConstructor { get; set; }
        int Position { get; set; }
        object Value { get; set; }
        string ValueType { get; set; }
    }/*End of IAttributeDetails interface*/
}/*End of Funny.Accents.Core.Reflection.Interfaces namespace*/
