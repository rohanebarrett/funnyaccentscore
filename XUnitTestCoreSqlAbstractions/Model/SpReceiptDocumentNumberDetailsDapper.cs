using Funny.Accents.Core.Reflection.Abstractions.Enumerations;
using Funny.Accents.Core.Reflection.Abstractions.Services;

namespace XUnitTestCoreSqlAbstractions.Model
{
    class SpReceiptDocumentNumberDetailsDapper : DapperReflector<SpReceiptDocumentNumberDetailsParameters>
    {
        public SpReceiptDocumentNumberDetailsDapper(string command,
            SpReceiptDocumentNumberDetailsParameters spDefinition, 
            ParameterOrientation orientation) : base(command, spDefinition, orientation)
        {
        }
    }
}
