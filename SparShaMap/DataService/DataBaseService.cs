using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace SparShaMap.DataService
{
    public class DataBaseService
    {
        public string getConnection()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            return configuration["ConnectionStrings:DefaultConnection"];
        }
        public string getPath()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            return configuration["UploadPath:DefaultUploadPath"];
        }
        public string getServerKey()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            return configuration["PushNotification:Token"];
        }
        public async Task<List<Dictionary<string, object>>> SelectQuery(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(getConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        if (dt.Rows.Count <= 0) throw new ArgumentException("ไม่พบข้อมูล");
                        foreach (DataRow dr in dt.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                        }
                        return rows;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public List<Dictionary<string, object>> SelectQueryNoAsync(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(getConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        if (dt.Rows.Count <= 0) throw new ArgumentException("ไม่พบข้อมูล");
                        foreach (DataRow dr in dt.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                        }
                        return rows;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public async Task<string> QueryData(string sql)
        {
            using (var connection = new SqlConnection(getConnection()))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        await connection.OpenAsync();
                        command.CommandText = sql;
                        var result = await command.ExecuteNonQueryAsync();
                        return result.ToString();
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }
                }
            }
        }
        public async Task<string> GetPrimaryKey(string TableNmae)
        {
            try
            {
                string sql = "SELECT column_name as PRIMARYKEYCOLUMN "
            + "FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC "
            + "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU "
            + "ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND "
            + "TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME AND "
            + "KU.table_name='" + TableNmae + "' "
            + "ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION;";
                var result = await SelectQuery(sql);
                return result.SelectMany(x => x).Where(x => x.Key == "PRIMARYKEYCOLUMN").FirstOrDefault().Value.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<Dictionary<string, object>>> GetAllTable()
        {
            try
            {
                string sql = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";
                return await SelectQuery(sql);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public async Task<string> GetDataBaseName()
        {
            try
            {
                string sql = "SELECT db_name() as DATABASENAME ";
                var result = await SelectQuery(sql);
                return result.SelectMany(x => x).Where(x => x.Key == "DATABASENAME").FirstOrDefault().Value.ToString();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<string> InsertData(string sql)
        {
            try
            {
                List<string> ListID = new List<string>();
                using (var connection = new SqlConnection(getConnection()))
                {
                    string[] listSql = sql.Split(';');
                    foreach (var Sql in listSql)
                    {
                        if (Sql != "")
                        {
                            using (SqlCommand cmd = new SqlCommand(Sql, connection))
                            {
                                connection.Open();
                                using (TransactionScope scope = new TransactionScope())
                                {
                                    var modified = cmd.ExecuteScalar();
                                    if (connection.State == System.Data.ConnectionState.Open)
                                    {
                                        connection.Close();
                                    }
                                    scope.Complete();
                                    ListID.Add(modified.ToString());
                                }
                            }
                        }
                    }
                }
                return ListID;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
