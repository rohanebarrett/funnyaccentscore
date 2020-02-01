using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Funny.Accents.Core.Database.Model;

namespace Funny.Accents.Core.Database.Helpers
{
    public static class DatabaseUtil
    {
        public static List<T> GetGenericDataViaStoredProcedure<T>(string connectionString,
            string storedProcedureName, DynamicParameters parameterList = null)
        {
            using (var cnn = new SqlConnection(connectionString))
            {
                try
                {
                    return cnn.Query<T>(storedProcedureName, parameterList
                        , commandType: CommandType.StoredProcedure).ToList();
                }
                catch (Exception ex)
                {
                    return new List<T>();
                }
            }/*End of SqlConnection using statement*/
        }/*End of GetGenericDataViaStoredProcedure method*/

        public static DatabaseResponse<T> GetStoredProcedureWithOutPutParameters<T>(string connectionString,
            string storedProcedureName, DynamicParameters parameterList = null)
            where T : class
        {
            using (var cnn = new SqlConnection(connectionString))
            {
                try
                {
                    var dbData = cnn.Query<T>(storedProcedureName, parameterList
                        , commandType: CommandType.StoredProcedure).ToList();

                    return new DatabaseResponse<T>
                    {
                        DbData = dbData,
                        DbParameters = GetParameterMapping(parameterList)
                    };
                }
                catch (Exception ex)
                {
                    return new DatabaseResponse<T>();
                }
            }/*End of SqlConnection using statement*/
        }/*End of GetGenericDataViaStoredProcedure method*/

        public static void ExecuteStoredProcedure(string connectionString, string storedProcedureName
            , DynamicParameters parameterList = null)
        {
            using (var cnn = new SqlConnection(connectionString))
            {
                cnn.Execute(storedProcedureName, parameterList
                    , commandType: CommandType.StoredProcedure);
            }/*End of SqlConnection using statement*/
        }/*End of GetGenericDataViaStoredProcedure method*/

        public static DynamicParameters ExecuteStoredProcedureNonQuery(string connectionString,
            string storedProcedureName, DynamicParameters parameterList = null)
        {
            using (var cnn = new SqlConnection(connectionString))
            {
                cnn.Execute(storedProcedureName, parameterList
                    , commandType: CommandType.StoredProcedure);
            }/*End of SqlConnection using statement*/

            return parameterList;
        }/*End of GetGenericDataViaStoredProcedure method*/

        public static SqlParameter[] ExecuteStoredProcedureNonQuery(string connectionString,
            string storedProcedureName, SqlParameter[] sqlParameters = null)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    if (sqlParameters != null)
                    {
                        cmd.Parameters.AddRange(sqlParameters);
                    }

                    sqlConnection.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }
            }
            return sqlParameters;
        }

        public static async Task<DynamicParameters> ExecuteStoredProcedureNonQueryAsync(
            string connectionString, string storedProcedureName, DynamicParameters parameterList = null)
        {
            using (var cnn = new SqlConnection(connectionString))
            {
                await cnn.ExecuteAsync(storedProcedureName, parameterList
                     , commandType: CommandType.StoredProcedure);
            }/*End of SqlConnection using statement*/

            return parameterList;
        }/*End of GetGenericDataViaStoredProcedure method*/

        public static async Task<SqlParameter[]> ExecuteStoredProcedureNonQueryAsync(string connectionString,
            string storedProcedureName, SqlParameter[] sqlParameters = null)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    if (sqlParameters != null)
                    {
                        cmd.Parameters.AddRange(sqlParameters);
                    }

                    sqlConnection.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return sqlParameters;
        }

        public static IDictionary<string, object> GetParameterMapping(DynamicParameters parameters
            , Func<string, string> nameFormatting = null)
        {
            var outputList = new Dictionary<string, object>();

            foreach (var pName in parameters.ParameterNames)
            {
                var outputValue = parameters.Get<object>(pName);

                outputList.Add(nameFormatting != null ? nameFormatting.Invoke(pName) : pName, outputValue);
            }

            return outputList;
        }

        public static IDictionary<string, object> GetParameterMapping(IEnumerable<SqlParameter> sqlParameters,
            bool isOutputParameter = false,
            Func<string, string> nameFormatting = null)
        {
            var outputList = new Dictionary<string, object>();
            sqlParameters.Where(p => isOutputParameter == false || p.Direction == ParameterDirection.Output)
                 //.Select(p => new
                 //{
                 //    Name = nameFormatting != null ? nameFormatting.Invoke(p.ParameterName) : p.ParameterName,
                 //    p.Value
                 //});
                 .ToList()
                 .ForEach(p =>
                 {
                     outputList.Add(nameFormatting != null ? nameFormatting.Invoke(p.ParameterName) : p.ParameterName, p.Value);
                 });

            return outputList;
        }

        public static void WriteToDatabase(
            string connString, string dbTable, DataTable dataTable)
        {
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    foreach (DataColumn c in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                    }
                    bulkCopy.DestinationTableName = dbTable;
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }/*End of WriteToDatabase method*/
    }/*End of DatabaseUtil class*/
}/*End of CmkUtilities namespace*/