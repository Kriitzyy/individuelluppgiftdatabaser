using System;
using System.Threading.Tasks;
using CoreofApplication;
using Npgsql;

// Räknar ut hur mycket användaren har spenderat, i perioder 
// År, månad, vecka, och dag 
public class MoneySpentCalculator
{
    // Method to calculate total money spent by a user
    public static async Task<decimal> CalculateTotalMoneySpent(int clientId)
    {
        decimal totalSpent = 0;

        try
        {
            using (var connection = Connection.GetConnection()) // Use the centralized connection utility
            {
                await connection.OpenAsync();

                // Simple SQL query to get the total money spent by the user
                string sqlQuery = "SELECT SUM(amount) FROM transactions WHERE client_id = @clientId";

                using (var cmd = new NpgsqlCommand(sqlQuery, connection))
                {
                    // Add the client ID as a parameter
                    cmd.Parameters.AddWithValue("@clientId", clientId);

                    // Execute the query and get the result
                    var result = await cmd.ExecuteScalarAsync();

                    // Check if the result is not DBNull
                    if (result != DBNull.Value)
                    {
                        totalSpent = Convert.ToDecimal(result);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while calculating total money spent: {ex.Message}");
        }

        return totalSpent;
    }
}