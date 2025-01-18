using System;
using Npgsql;
using System.Threading.Tasks;
using Client;

// Funkar som den ska!! testad i main
// Id numret håller koll vem de e som ska kolla 
// current balance 

namespace CoreofApplication
{
    public class GetTransaction
    {
        // Retrieve current balance of a user by client_id
        public async Task<decimal> GetCurrentBalance(Transaction transaction)
        {
            decimal currentBalance = 0;

            try
            {
                // Open a connection to the database using the GetConnection method
                using (var connection = Connection.GetConnection())
                {
                    await connection.OpenAsync();

                    // Query to sum all transactions of a specific client
                    string sqlQuery = "SELECT SUM(amount) FROM transactions WHERE client_id = @client_id";

                    using (var cmd = new NpgsqlCommand(sqlQuery, connection))
                    {
                        // Use the clientId from the passed transaction object
                        cmd.Parameters.AddWithValue("@client_id", transaction.ClientId);  // Correct access to ClientId

                        // Execute the query and retrieve the sum of amounts
                        var result = await cmd.ExecuteScalarAsync();

                        // If the result is not DBNull, assign the value to currentBalance
                        if (result != DBNull.Value)
                        {
                            currentBalance = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // In case of error, print the exception message
                Console.WriteLine($"Error retrieving balance: {ex.Message}");
            }

            return currentBalance;  // Return the calculated balance
        }
    }
}
