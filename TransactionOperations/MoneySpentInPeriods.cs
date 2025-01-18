using System;
using System.Threading.Tasks;
using Npgsql;
using Client; // Import the namespace where your Transaction class is located

// Är columnerna fel eller används dom dör datum? 
namespace CoreofApplication
{
    public class MoneySpentCalculator
    {
        // Method to calculate total money spent by a user, based on the Transaction object and specific time period
        public static async Task<decimal> MoneySpentPeriods(Transaction transaction, string periodType, int? year = null, 
        int? month = null, int? week = null, DateTime? day = null)
        {
            decimal totalSpent = 0;

            try
            {
                // Use the GetConnection method to get a connection
                using (var connection = Connection.GetConnection()) // Replaced with Connection.GetConnection()
                {
                    await connection.OpenAsync();

                    // Base SQL query for selecting total money spent by client
                    string sqlQuery = "SELECT SUM(amount) FROM transactions WHERE client_id = @clientId AND amount < 0";

                    // Append conditions based on the periodType
                    if (periodType == "year" && year.HasValue)
                    {
                        sqlQuery += " AND EXTRACT(YEAR FROM transaction_date) = @year";
                    }
                    else if (periodType == "month" && year.HasValue && month.HasValue)
                    {
                        sqlQuery += " AND EXTRACT(YEAR FROM transaction_date) = @year AND EXTRACT(MONTH FROM transaction_date) = @month";
                    }
                    else if (periodType == "week" && year.HasValue && week.HasValue)
                    {
                        sqlQuery += " AND EXTRACT(YEAR FROM transaction_date) = @year AND EXTRACT(WEEK FROM transaction_date) = @week";
                    }
                    else if (periodType == "day" && day.HasValue)
                    {
                        sqlQuery += " AND DATE(transaction_date) = @day"; // Filter by exact day
                    }

                    using (var cmd = new NpgsqlCommand(sqlQuery, connection))
                    {
                        // Add the client ID (from the Transaction object) as a parameter
                        cmd.Parameters.AddWithValue("@clientId", transaction.ClientId);

                        // Add the period parameters based on the selected periodType
                        if (year.HasValue) cmd.Parameters.AddWithValue("@year", year.Value); 
                        if (month.HasValue) cmd.Parameters.AddWithValue("@month", month.Value);
                        if (week.HasValue) cmd.Parameters.AddWithValue("@week", week.Value);
                        if (day.HasValue) cmd.Parameters.AddWithValue("@day", day.Value.Date); // Make sure to pass only the date part

                        // Execute the query and get the result
                        var result = await cmd.ExecuteScalarAsync();

                        // If result is not DBNull, assign the value to totalSpent
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
}
