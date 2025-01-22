using System;
using Npgsql;
using CoreofApplication;
using System.Threading.Tasks;
using Client;

// Funkar bra, raderar utifrån ID nummer !!!

namespace CoreofApplication
{
    public class DeleteTransaction
    {
        // Modified to take the Transaction object as a parameter and use client_id to delete transactions
        public static async Task DeleteTransactions(Transaction transaction)
        {
            try
            {             
                using (var connection = Connection.GetConnection()) // Use Connection.GetConnection()
                {
                    await connection.OpenAsync();

                    // Use client_id to delete transactions associated with a client
                    string deleteQuery = "DELETE FROM transactions WHERE client_id = @client_id";

                    using (var cmd = new NpgsqlCommand(deleteQuery, connection))
                    {
                        // Use the ClientId from the Transaction object for the query
                        cmd.Parameters.AddWithValue("@client_id", transaction.ClientId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Transactions deleted successfully!");
                        }
                        else
                        {
                            Console.WriteLine("No transactions found for the specified client ID.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
