using BuildingBlock.Base.Abstractions;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace BuildingBlock.Dapper
{
    public class SqlServerStrategy : IDbStrategy
    {
        private string _connectionString;
        public SqlServerStrategy(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection GetConnection()
            => new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=FlavorWorldMicDb;Trusted_Connection=True;");
        //return new SqlConnection(FormatConnectionString(_connectionString));


        private string AddQuotesIfMissing(string connectionString)
        {
            if (!connectionString.StartsWith("\""))
                connectionString = "\"" + connectionString;

            if (!connectionString.EndsWith("\""))
                connectionString += "\"";

            return connectionString;
        }

        public string RemoveQuotesIfPresent(string connectionString)
        {
            if (connectionString.StartsWith("\""))
                connectionString = connectionString.Substring(1);

            if (connectionString.EndsWith("\""))
                connectionString = connectionString.Substring(0, connectionString.Length - 1);

            return connectionString;
        }

        private string FormatConnectionString(string originalConnectionString)
        {
            string serverValue = ExtractValue(originalConnectionString, "Server");
            string databaseValue = ExtractValue(originalConnectionString, "Database");
            string newConnectionString = $"Server={serverValue};Database={databaseValue};Trusted_Connection=True;";

            return newConnectionString;
        }

        private string ExtractValue(string connectionString, string key)
        {
            string pattern = $"{key}=([^;]+);";

            Match match = Regex.Match(connectionString, pattern);

            if (match.Success)
                return match.Groups[1].Value;
            else
                return string.Empty;
        }
        private string EscapeBackslashes(string input)
            => input.Replace(@"\", @"\");
    }
}
