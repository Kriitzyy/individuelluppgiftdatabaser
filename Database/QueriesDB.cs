using System;
using Npgsql;
// EJ testad, kolla upp koden om den funkar 
public class DatabaseOperations
{
    private static string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

    // Generisk metod för att exekvera SQL-frågor
    public static void ExecuteQuery(string query, Action<NpgsqlCommand> parameterizeCommand = null)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    parameterizeCommand?.Invoke(command);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Operation executed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    // Metod för att lägga till en klient
    public static void InsertClient(string username, string passwordHash, string email)
    {
        string query = @"INSERT INTO clients (username, passwordhash, email)
                        VALUES (@username, @passwordhash, @email)";

        ExecuteQuery(query, command =>
        {
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@passwordhash", passwordHash);
            command.Parameters.AddWithValue("@email", email);
        });
    }

    // Metod för att uppdatera en klient
    public static void UpdateClientEmail(int clientId, string newEmail)
    {
        string query = @"UPDATE clients SET email = @newEmail WHERE id = @clientId";

        ExecuteQuery(query, command =>
        {
            command.Parameters.AddWithValue("@clientId", clientId);
            command.Parameters.AddWithValue("@newEmail", newEmail);
        });
    }

    // Metod för att lägga till en transaktion
    public static void InsertTransaction(int clientId, decimal amount, string description)
    {
        string query = @"INSERT INTO transactions (client_id, amount, description, transaction_date)
                        VALUES (@client_id, @amount, @description, CURRENT_DATE)";

        ExecuteQuery(query, command =>
        {
            command.Parameters.AddWithValue("@client_id", clientId);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@description", description);
        });
    }

    // Metod för att uppdatera en transaktion
    public static void UpdateTransactionDescription(int transactionId, string newDescription)
    {
        string query = @"UPDATE transactions SET description = @newDescription WHERE id = @transactionId";

        ExecuteQuery(query, command =>
        {
            command.Parameters.AddWithValue("@transactionId", transactionId);
            command.Parameters.AddWithValue("@newDescription", newDescription);
        });
    }

    // Metod för att ta bort en klient
    public static void DeleteClient(int clientId)
    {
        string query = @"DELETE FROM clients WHERE id = @clientId";

        ExecuteQuery(query, command =>
        {
            command.Parameters.AddWithValue("@clientId", clientId);
        });
    }

    // Metod för att ta bort en transaktion
    public static void DeleteTransaction(int transactionId)
    {
        string query = @"DELETE FROM transactions WHERE id = @transactionId";

        ExecuteQuery(query, command =>
        {
            command.Parameters.AddWithValue("@transactionId", transactionId);
        });
    }
}
