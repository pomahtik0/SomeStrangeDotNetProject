namespace SomeStrangeDotNetProject.Models
{
    public class ValidatingDb(string connctionString)
    {
        private readonly string connctionString = connctionString;
    }
}
