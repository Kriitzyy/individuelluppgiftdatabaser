using System;
using Npgsql;

namespace QuizApp
{
    public class LoadQuizapp
    {
        public static void SetUpDatabase()
        {
            using (var connection = Connection.GetConnection())
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connected to the database.");

                    // Skapa tabellen för klienter
                    string createClientsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS clients (
                            id SERIAL PRIMARY KEY,
                            username VARCHAR(255) NOT NULL,
                            passwordhash VARCHAR(255) NOT NULL,
                            email VARCHAR(255) NOT NULL
                        );
                    ";
                    ExecuteQuery(connection, createClientsTableQuery);

                    // Lägg till testdata i clients
                    string insertClientQuery = @"
                        INSERT INTO clients (username, passwordhash, email) 
                        VALUES 
                            ('test1', 'foreign123', 'key')
                        ON CONFLICT DO NOTHING;
                    ";
                    ExecuteQuery(connection, insertClientQuery);

                    // Skapa tabellen för transaktioner
                    string createTransactionsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS transactions (
                            id SERIAL PRIMARY KEY,
                            client_id INT NOT NULL,
                            transaction_date DATE NOT NULL DEFAULT CURRENT_DATE,
                            amount DECIMAL(10, 2) NOT NULL,
                            description TEXT,
                            CONSTRAINT fk_client FOREIGN KEY (client_id) REFERENCES clients(id)
                        );
                    ";
                    ExecuteQuery(connection, createTransactionsTableQuery);

                    // Lägg till testdata i transactions
                    string insertTransactionQuery = @"
                        INSERT INTO transactions (client_id, amount, description) 
                        VALUES 
                            (1, 100.00, 'Loan')
                        ON CONFLICT DO NOTHING;
                    ";
                    ExecuteQuery(connection, insertTransactionQuery);

                    Console.WriteLine("Tabeller skapade och testdata insatt.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade: {ex.Message}");
                }
            }
        }

        private static void ExecuteQuery(NpgsqlConnection connection, string query)
        {
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
