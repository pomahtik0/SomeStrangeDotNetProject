using System.Data.SqlClient;

namespace SomeStrangeDotNetProject.Models
{
    public class ValidatingDb(string connectionString)
    {
        private readonly string connectionString = connectionString;

        public bool Validate()
        {
            
        }

        private bool DatabaseExist()
        {

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
