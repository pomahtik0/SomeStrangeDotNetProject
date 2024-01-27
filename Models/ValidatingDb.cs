using System.Data.SqlClient;

namespace SomeStrangeDotNetProject.Models
{
    public class ValidatingDb(string connectionString)
    {
        private readonly string connectionString = connectionString;

        public bool Validate()
        {
            using(var connection = new SqlConnection(connectionString))
            {
                if(!DatabaseExist(connection)) return false;
                CreateTablesIfNotExist(connection);
                if(!TablesSignatureFits(connection)) return false;
            }
            return true;
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
                conn.Close();
                if (count == 2)
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
            conn.Open();

            string sql = "DROP TABLE IF EXISTS table1, table2";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }

            sql = "CREATE TABLE [dbo].[Objects] ( [Id] INT IDENTITY (1, 1) NOT NULL, [Key] VARCHAR (120) NULL, [Value] VARCHAR (255) NULL, [Separator] VARCHAR (7) NOT NULL, [Parent_id] INT NULL, [Tree_id] INT NOT NULL, PRIMARY KEY CLUSTERED ([Id] ASC), CONSTRAINT [Selfrefference_FK] FOREIGN KEY ([Parent_id]) REFERENCES [dbo].[Objects] ([Id]) )";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }

            sql = "CREATE TABLE [dbo].[Trees] ([Id] INT IDENTITY (1, 1) NOT NULL, [Name] VARCHAR (100) NOT NULL, [Root_id] INT NULL, PRIMARY KEY CLUSTERED ([Id] ASC), CONSTRAINT [FK_Trees_ToObjects] FOREIGN KEY ([Root_id]) REFERENCES [dbo].[Objects] ([Id]) )";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }

            sql = "ALTER TABLE [dbo].[Objects] ADD CONSTRAINT [FK_ToRootTable] FOREIGN KEY ([Tree_id]) REFERENCES [dbo].[Trees] ([Id]) ON DELETE CASCADE";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private bool TablesSignatureFits(SqlConnection conn)
        {
            return true; // have no idea yet
        }
    }
}
