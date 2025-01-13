using System;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

// Räknar ut användarens current balance, 5!

namespace CoreofApplication
{
    public class GetTransaction
    {
        private readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        // Retrieve transactions for a specific user by client_id
        public async Task<decimal> GetUserCurrentBalance(int ClientId)
        {
            decimal currentBalance = 0;

            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT SUM(amount) FROM transactions WHERE client_id = @clientId";

                    using (var cmd = new NpgsqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientId", ClientId);

                        // Execute the query and retrieve the sum of amounts
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != DBNull.Value)
                        {
                            currentBalance = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving balance: {ex.Message}");
            }

            return currentBalance;
        }
    }
}