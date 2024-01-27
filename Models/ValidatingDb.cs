using System.Data.SqlClient;

namespace SomeStrangeDotNetProject.Models
{
    public class ValidatingDb(string connectionString)
    {
        private readonly string connectionString = connectionString;

        public bool Validate()
        {
            
        }

        private bool DatabaseExist(SqlConnection conn) // possibly wrong
        {
            conn.Open();
            string sql = "SELECT OBJECT_ID(N'mydatabase', N'DB')";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool DataTablesExist(SqlConnection conn)
        {
            conn.Open();
            string sql1 = "SELECT OBJECT_ID(N'dbo.Trees', N'U')";
            string sql2 = "SELECT OBJECT_ID(N'dbo.Objects', N'U')";
            using (SqlCommand cmd1 = new SqlCommand(sql1, conn))
            {
                object result1 = cmd1.ExecuteScalar();
                if (result1 != null)
                {
                    using (var cmd2 = new SqlCommand(sql1, conn))
                    {
                        object result2 = cmd2.ExecuteScalar();
                        if (result2 != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private void CreateTablesIfNotExist(SqlConnection conn)
        {
            if (DataTablesExist(conn)) return;
        }

        private bool TablesSignatureFits()
        {

        }
    }
}
