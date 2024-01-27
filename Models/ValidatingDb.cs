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
            try
            {
                conn.Open();
            }
            catch
            {
                return false;
            }
            conn.Close();
            return true;
        }

        private bool DataTablesExist(SqlConnection conn)
        {
            conn.Open();
            string sql = "SELECT COUNT(*) FROM sys.tables WHERE name IN ('Trees', 'Objects')";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                int count = (int)cmd.ExecuteScalar();
                if(count== 2)
                {
                    return true;
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
