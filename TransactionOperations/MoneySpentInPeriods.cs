using System;
using System.Threading.Tasks;
using Npgsql;
using Client; 

namespace CoreofApplication {

    // Klass för spenderade pengar
    public class MoneySpentCalculator {

        // Metod för att beräkna totala spenderade pengar av en användare.
        public static async Task<decimal> MoneySpentPeriods(Transaction transaction, string periodType, int? year = null, 
        int? month = null, int? week = null, DateTime? day = null, bool includeDeleted = false)  {

            decimal totalSpent = 0;

            try {

                // Hämtar metod för anslutning till databas
                using (var connection = Connection.GetConnection()) {

                    // Öppnar anslutningen
                    await connection.OpenAsync();

                    // SQL-fråga för att hämta totalt spenderade pengar av en användare
                    string sqlQuery = "SELECT SUM(amount) FROM transactions WHERE client_id = @client_id AND amount < 0";

                    // Lägger till villkor för raderade transaktioner OM includeDeleted är true
                    if (includeDeleted) {

                        // Lägger till filter för raderade transaktioner 
                        sqlQuery += " AND is_deleted = TRUE"; 
                    }

                    // Lägger till villkor baserat på periodTyp
                    if (periodType == "year" && year.HasValue) {

                        // Lägger till villkor för året om det är angivet
                        sqlQuery += " AND EXTRACT(YEAR FROM transaction_date) = @year";
                    }

                    else if (periodType == "month" && year.HasValue && month.HasValue) {

                        // Lägger till villkor för månaden om både år och månad ärskriva
                        sqlQuery += " AND EXTRACT(YEAR FROM transaction_date) = @year AND EXTRACT(MONTH FROM transaction_date) = @month";
                    }

                    else if (periodType == "week" && year.HasValue && week.HasValue) {

                        // Lägger till villkor för veckan om både år och vecka är skrivna
                        sqlQuery += " AND EXTRACT(YEAR FROM transaction_date) = @year AND EXTRACT(WEEK FROM transaction_date) = @week";
                    }

                    else if (periodType == "day" && day.HasValue) {

                        // Lägger till villkor för exakt dag day parameter är angiven
                        sqlQuery += " AND DATE(transaction_date) = @day"; 
                    }

                    // Förbereder SQL-kommandot med frågan
                    using (var cmd = new NpgsqlCommand(sqlQuery, connection)) {

                        // Lägger till klient-ID från Transaktionsobjektet som en parameter i frågan
                        cmd.Parameters.AddWithValue("@client_id", transaction.ClientId);

                        // Lägger till periodparametrar baserat på vald periodtyp
                        if (year.HasValue) cmd.Parameters.AddWithValue("@year", year.Value); 
                        if (month.HasValue) cmd.Parameters.AddWithValue("@month", month.Value);
                        if (week.HasValue) cmd.Parameters.AddWithValue("@week", week.Value);
                        if (day.HasValue) cmd.Parameters.AddWithValue("@day", day.Value.Date);

                        // Kör frågan och får resultatet
                        var result = await cmd.ExecuteScalarAsync();

                        // Om resultatet inte är DBNull, tilldela värdet till totalSpent
                        if (result != DBNull.Value) {

                            totalSpent = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex) {

                // Loggar eventuella fel som uppstår under metodens körning
                Console.WriteLine($"Something went wrong when running the option: {ex.Message}");
            }

            // Returnerar den totala summan spenderade pengar
            return totalSpent;
        }
    }
}
