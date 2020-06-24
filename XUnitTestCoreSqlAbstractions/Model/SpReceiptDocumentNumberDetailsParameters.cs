using Funny.Accents.Core.Reflection.Abstractions.Attribues;
using System.Data;

namespace XUnitTestCoreSqlAbstractions.Model
{
    class SpReceiptDocumentNumberDetailsParameters
    {
        [SpAttributes(ParameterName = "@_document_number",
            Size = 20,
            SqlDbType = SqlDbType.NVarChar,
            DbType = DbType.String,
            OrdinalPosition = 1,
            Direction = ParameterDirection.Input)]
        public string DocumentNumber { get; set; }

        [SpAttributes(ParameterName = "@_document_number_type",
            Size = 20,
            SqlDbType = SqlDbType.NVarChar,
            DbType = DbType.String,
            OrdinalPosition = 2,
            Direction = ParameterDirection.Input)]
        public string DocumentNumberType { get; set; }

        [SpAttributes(ParameterName = "@_receipt_document_number_details_create_status",
            SqlDbType = SqlDbType.Int,
            DbType = DbType.Int32,
            OrdinalPosition = 3,
            Direction = ParameterDirection.Output)]
        public int ReceiptDocumentNumberDetailsStatus { get; set; }

        [SpAttributes(ParameterName = "@_receipt_document_number_details_create_message",
            Size = 4000,
            SqlDbType = SqlDbType.NVarChar,
            DbType = DbType.String,
            OrdinalPosition = 4,
            Direction = ParameterDirection.Output)]
        public string ReceiptDocumentNumberDetailsMessage { get; set; }
    }/*End of SpReceiptDocumentNumberDetailsCreate class*/
}
