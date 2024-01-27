using System.Data.SqlClient;

namespace SomeStrangeDotNetProject.Models
{
    public class ValidatingDb(string connectionString)
    {
        private readonly string connectionString = connectionString;

        public bool Validate()
        {
            
        }

        private bool DatabaseExist(SqlConnection conn)
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

        private bool DataTablesExist()
        {

        }

        private void CreateTablesIfNotExist()
        {

        }

        private bool TablesSignatureFits()
        {

        }
    }
}
