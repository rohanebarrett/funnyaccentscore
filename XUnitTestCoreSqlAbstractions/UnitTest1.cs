using Funny.Accents.Core.Database.Helpers;
using Funny.Accents.Core.Reflection.Abstractions.Enumerations;
using Xunit;
using Xunit.Abstractions;
using XUnitTestCoreSqlAbstractions.Model;

namespace XUnitTestCoreSqlAbstractions
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void Test1()
        {
            const string spCommand = "[inventory].[usp_receipt_document_number_details_create]";
            const string dbConString = "Data Source=CGIVMAPP_TEST\\SQL_TEST_2012;Initial Catalog=BinGo V5;User ID=SQL_ADMIN;Password=newday_99";

            var spParameters = new SpReceiptDocumentNumberDetailsParameters
            {
                DocumentNumber = "443030_9",
                DocumentNumberType = "PO_RECEIPT"
            };

            var spExecutionDetails 
                = new SpReceiptDocumentNumberDetailsDapper(spCommand,spParameters, ParameterOrientation.Ordinal);

            var (spCommandFromMethod, sqlParameters) = spExecutionDetails.GetSpDetails();

            _output.WriteLine($"SpCommand: {spCommand}");

            //foreach (var param in sqlParameters)
            //{
            //    _output.WriteLine($"SpParameter: Name -> {param.ParameterName} | Value -> {param.Value}");
            //}

            var outputParameters = DatabaseUtil.ExecuteStoredProcedureNonQuery(dbConString, 
                spCommandFromMethod, sqlParameters);

            var status = outputParameters.Get<int>("@_receipt_document_number_details_create_status");
            var message = outputParameters.Get<string>("@_receipt_document_number_details_create_message");
            
            foreach (var pName in DatabaseUtil.GetParameterMapping(outputParameters))
            {
                _output.WriteLine($"{pName.Key} | {pName.Value}");
            }

            Assert.True(spCommandFromMethod != null && spCommandFromMethod.Equals(spCommand));

        }

        [Fact]
        public void Sql_Reflector_Return_Output_Parameters()
        {
            const string spCommand = "[inventory].[usp_receipt_document_number_details_create]";
            const string dbConString = "Data Source=CGIVMAPP_TEST\\SQL_TEST_2012;Initial Catalog=BinGo V5;User ID=SQL_ADMIN;Password=newday_99";

            var spParameters = new SpReceiptDocumentNumberDetailsParameters
            {
                DocumentNumber = "443030_9",
                DocumentNumberType = "PO_RECEIPT"
            };

            var spExecutionDetails
                = new SpReceiptDocumentNumberDetails(spCommand, spParameters, ParameterOrientation.Ordinal);

            var (spCommandFromMethod,sqlSignature, sqlParameters) = spExecutionDetails.GetSpDetails();

            _output.WriteLine($"SpCommand: {spCommand}");

            var outputParameters = DatabaseUtil.ExecuteStoredProcedureNonQuery(dbConString,
                spCommandFromMethod, sqlParameters.ToArray());

            foreach (var pName in DatabaseUtil.GetParameterMapping(outputParameters,false,
                nameFormatting: p => p.Replace("@_","")))
            {
                _output.WriteLine($"{pName.Key} | {pName.Value}");
            }

            Assert.True(spCommandFromMethod != null && spCommandFromMethod.Equals(spCommand));
        }
    }
}
