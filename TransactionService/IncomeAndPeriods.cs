using System;
using Npgsql;
using System.Threading.Tasks;

// Användarens inkomst för år, månad, vecka, dag. 2! 
namespace CoreofApplication
{
    public class IncomeTransaction
    {
        private static readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        // Generalized method to get total income for a user within a given date range
        public static async Task<decimal> GetIncomeAsync(int clientId, DateTime startDate, DateTime endDate)
        {
            decimal totalIncome = 0;

            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    // SQL query to sum up positive transaction amounts (income) within the date range
                    string sqlQuery = "SELECT SUM(amount) FROM transactions WHERE client_id = @clientId " +
                                      "AND amount > 0 AND transaction_date >= @startDate AND transaction_date <= @endDate";

                    using (var cmd = new NpgsqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientId", clientId);
                        cmd.Parameters.AddWithValue("@startDate", startDate);
                        cmd.Parameters.AddWithValue("@endDate", endDate);

                        // Execute query and retrieve the total income
                        var result = await cmd.ExecuteScalarAsync();

                        // If result is not null, assign the value to totalIncome
                        if (result != DBNull.Value)
                        {
                            totalIncome = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return totalIncome;
        }

        // Helper method to get the start and end dates for a specific period
        public static (DateTime startDate, DateTime endDate) GetPeriods(string period)
        {
            DateTime currentDate = DateTime.Now;
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            switch (period.ToLower())
            {
                case "year":
                    startDate = new DateTime(currentDate.Year, 1, 1);
                    endDate = new DateTime(currentDate.Year, 12, 31, 23, 59, 59);
                    break;
                case "month":
                    startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    break;
                case "week":
                    startDate = currentDate.AddDays(-(int)currentDate.DayOfWeek); // Sunday of the current week
                    endDate = startDate.AddDays(6); // Saturday of the current week
                    break;
                case "day":
                    startDate = currentDate.Date;
                    endDate = currentDate.Date.AddDays(1).AddSeconds(-1); // Last second of the day
                    break;
                default:
                    throw new ArgumentException("Invalid period type. Use 'year', 'month', 'week', or 'day'.");
            }

            return (startDate, endDate);
        }
    }
}
