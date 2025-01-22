using System;
using System.Threading.Tasks;
using Npgsql;
using CoreofApplication;
using Client;
using CoreofApplication; 

// Funkar som den ska!! testad i main
 
namespace CoreofApplication
{
    // A class to handle the database operation for depositing transactions
    public class DepositTransaction
    {
        // Asynchronous method to deposit money into the user's account
        public static async Task<bool> DepositTransactions(Transaction transaction)
        {
            
            try
            {
                using (var connection = Connection.GetConnection()) // Use Connection.GetConnection()
                {
                    await connection.OpenAsync();

                    string sqlQuery = "INSERT INTO transactions (client_id, transaction_date, amount, description) " +
                                      "VALUES (@client_id, @transaction_date, @amount, @description)";

                    using (var cmd = new NpgsqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@client_id", transaction.ClientId);
                        cmd.Parameters.AddWithValue("@transaction_date", DateTime.Now);  // Assuming current date as the transaction date
                        cmd.Parameters.AddWithValue("@amount", transaction.amount);
                        cmd.Parameters.AddWithValue("@description", transaction.source);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync(); // Execute the SQL query

                        // Return true if the transaction was successfully inserted
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
           public static async Task<List<Transaction>> GetTransactionsByClientId(int clientId)
        {
            var transactions = new List<Transaction>();

            try
            {
                using (var connection = Connection.GetConnection())
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT transaction_date, amount, description " +
                                      "FROM transactions WHERE client_id = @client_id";

                    using (var cmd = new NpgsqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@client_id", clientId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                transactions.Add(new Transaction
                                {
                                    TransactionDate = reader.GetDateTime(0),
                                    amount = reader.GetDecimal(1),
                                    source = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching transactions: {ex.Message}");
            }

            return transactions; // Return the list of transactions
        }
    }
}
