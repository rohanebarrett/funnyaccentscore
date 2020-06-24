using Funny.Accents.Core.Reflection.Abstractions.Enumerations;
using Funny.Accents.Core.Reflection.Abstractions.Services;

namespace XUnitTestCoreSqlAbstractions.Model
{
    class SpReceiptDocumentNumberDetails : SqlReflector<SpReceiptDocumentNumberDetailsParameters>
    {
        public SpReceiptDocumentNumberDetails(string command,
            SpReceiptDocumentNumberDetailsParameters spDefinition, 
            ParameterOrientation orientation) : base(command, spDefinition, orientation)
        {
        }
    }
}
