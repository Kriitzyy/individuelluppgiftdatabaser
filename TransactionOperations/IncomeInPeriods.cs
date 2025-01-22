using Npgsql;
using System.Threading.Tasks;
using System;

// Denna fil hanterar användarens Income
// I perioder, år, månad, vecka och dag

namespace CoreofApplication {

    public class IncomeTransaction {

        // Metod för Income perioder 
        public static async Task<decimal> GetIncomePeriods(int ClientId, DateTime startDate, DateTime endDate) {

            decimal totalIncome = 0;

            try {

                // Hämtar connection
                using (var connection = Connection.GetConnection()) {
                    
                    // Öppnar connection
                    await connection.OpenAsync();

                    // SQL query för att räkna ihop positiva transaktioner (inkomst) inom viss datum
                    string sqlQuery = "SELECT SUM(amount) FROM transactions WHERE client_id = @client_id " +
                                      "AND amount > 0 AND transaction_date >= @startDate AND transaction_date <= @endDate";

                    using (var cmd = new NpgsqlCommand(sqlQuery, connection)) {

                        cmd.Parameters.AddWithValue("@client_id", ClientId);
                        cmd.Parameters.AddWithValue("@startDate", startDate);
                        cmd.Parameters.AddWithValue("@endDate", endDate);

                        // Kör queryn och hämta totala inkomst
                        var result = await cmd.ExecuteScalarAsync();

    	                // Om resultat inte är null ge Value till totaincome
                        if (result != DBNull.Value) {

                            totalIncome = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex) {

                // Om operationen misslyckades 
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return totalIncome;
        }

        // Metod för att hämta start och slut datum för specifika perioder
        public static (DateTime startDate, DateTime endDate) GetPeriods(string period) {

            DateTime currentDate = DateTime.Now;
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            switch (period.ToLower()) {

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
