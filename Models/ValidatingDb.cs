namespace SomeStrangeDotNetProject.Models
{
    public class ValidatingDb(string connctionString)
    {
        private readonly string connctionString = connctionString;

        public bool Validate()
        {
            return true;
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
