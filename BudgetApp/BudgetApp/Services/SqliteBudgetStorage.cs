using BudzetDomowy.Models;
using Microsoft.Data.Sqlite;

namespace BudzetDomowy.Services
{
    public class SqliteBudgetStorage : IBudgetStorage
    {
        public void Save(string path, BudgetData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            string connectionString = $"Data Source={path}";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            CreateTablesIfNeeded(connection);

            using var tx = connection.BeginTransaction();

            // Czyścimy tabele, żeby zapis był "od zera" (najprostszy wariant na laby)
            ExecuteNonQuery(connection, "DELETE FROM Categories;", tx);
            ExecuteNonQuery(connection, "DELETE FROM Limits;", tx);
            ExecuteNonQuery(connection, "DELETE FROM Transactions;", tx);

            // Categories
            foreach (var c in data.Categories)
            {
                using var cmd = connection.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = "INSERT INTO Categories (Id, Name) VALUES (@id, @name);";
                cmd.Parameters.AddWithValue("@id", c.Id);
                cmd.Parameters.AddWithValue("@name", c.Name);
                cmd.ExecuteNonQuery();
            }

            // Limits
            foreach (var l in data.Limits)
            {
                using var cmd = connection.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
INSERT INTO Limits (Year, Month, CategoryId, LimitCents)
VALUES (@year, @month, @catId, @limitCents);";
                cmd.Parameters.AddWithValue("@year", l.Year);
                cmd.Parameters.AddWithValue("@month", l.Month);
                cmd.Parameters.AddWithValue("@catId", l.CategoryId);
                cmd.Parameters.AddWithValue("@limitCents", ToCents(l.LimitAmount));
                cmd.ExecuteNonQuery();
            }

            // Transactions
            foreach (var t in data.Transactions)
            {
                using var cmd = connection.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
INSERT INTO Transactions (Id, Date, AmountCents, Description, CategoryId, Type)
VALUES (@id, @date, @amountCents, @desc, @catId, @type);";
                cmd.Parameters.AddWithValue("@id", t.Id);
                cmd.Parameters.AddWithValue("@date", t.Date.ToString("O")); // zapis daty w formacie, który da się odtworzyć
                cmd.Parameters.AddWithValue("@amountCents", ToCents(t.Amount));
                cmd.Parameters.AddWithValue("@desc", t.Description ?? "");
                cmd.Parameters.AddWithValue("@catId", t.CategoryId);
                cmd.Parameters.AddWithValue("@type", t.Type);
                cmd.ExecuteNonQuery();
            }

            tx.Commit();
        }

        public BudgetData Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Nie znaleziono pliku bazy SQLite.", path);

            string connectionString = $"Data Source={path}";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            CreateTablesIfNeeded(connection);

            var data = new BudgetData();

            // Categories
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Name FROM Categories;";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    data.Categories.Add(new Category(id, name));
                }
            }

            // Limits
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT Year, Month, CategoryId, LimitCents FROM Limits;";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int year = reader.GetInt32(0);
                    int month = reader.GetInt32(1);
                    int catId = reader.GetInt32(2);
                    long limitCents = reader.GetInt64(3);

                    data.Limits.Add(new BudgetLimit(year, month, catId, FromCents(limitCents)));
                }
            }

            // Transactions
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Date, AmountCents, Description, CategoryId, Type FROM Transactions;";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string dateText = reader.GetString(1);
                    long amountCents = reader.GetInt64(2);
                    string desc = reader.GetString(3);
                    int catId = reader.GetInt32(4);
                    string type = reader.GetString(5);

                    data.Transactions.Add(new TransactionDto
                    {
                        Id = id,
                        Date = DateTime.Parse(dateText),
                        Amount = FromCents(amountCents),
                        Description = desc,
                        CategoryId = catId,
                        Type = type
                    });
                }
            }

            return data;
        }

        private static void CreateTablesIfNeeded(SqliteConnection connection)
        {
            // Tabele tworzą się tylko jeśli ich nie ma
            ExecuteNonQuery(connection, @"
CREATE TABLE IF NOT EXISTS Categories(
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL
);");

            ExecuteNonQuery(connection, @"
CREATE TABLE IF NOT EXISTS Limits(
    Year INTEGER NOT NULL,
    Month INTEGER NOT NULL,
    CategoryId INTEGER NOT NULL,
    LimitCents INTEGER NOT NULL
);");

            ExecuteNonQuery(connection, @"
CREATE TABLE IF NOT EXISTS Transactions(
    Id INTEGER PRIMARY KEY,
    Date TEXT NOT NULL,
    AmountCents INTEGER NOT NULL,
    Description TEXT NOT NULL,
    CategoryId INTEGER NOT NULL,
    Type TEXT NOT NULL
);");
        }

        private static void ExecuteNonQuery(SqliteConnection connection, string sql, SqliteTransaction? tx = null)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        private static long ToCents(decimal amount)
            => (long)Math.Round(amount * 100m, MidpointRounding.AwayFromZero);

        private static decimal FromCents(long cents)
            => cents / 100m;
    }
}
