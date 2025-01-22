using System;
using Npgsql;
using System.Threading.Tasks;
using Client;

// Denna fil innehåller användarens nuvarande balans

namespace CoreofApplication {

    // Class för Currentbalance 
    public class GetTransaction { 

        // Metod med current balance
        public async Task<decimal> GetCurrentBalance(Transaction transaction) {

            // Variabel för att hålla den aktuella balansen
            decimal currentBalance = 0; 

            try {

                // Hämtar connection metoden
                using (var connection = Connection.GetConnection()) {

                    // Öppnar anslutningen till databasen
                    await connection.OpenAsync(); 

                    // Query för att räkna ihop alla transaktioner för specifik client (Id)
                    string sqlQuery = "SELECT SUM(amount) FROM transactions WHERE client_id = @client_id";

                    using (var cmd = new NpgsqlCommand(sqlQuery, connection)) {

                        // Sätter in klientens id i frågan
                        cmd.Parameters.AddWithValue("@client_id", transaction.ClientId);  

                        // kör frågan och hämtar summan av alla belopp
                        var result = await cmd.ExecuteScalarAsync();

                        // Om resultatet inte är DBNull, tilldela värdet till currentBalance
                        if (result != DBNull.Value) {

                        // Konvertera resultatet till decimal och tilldela till currentBalance
                            currentBalance = Convert.ToDecimal(result); 
                        }
                    }
                }
            }
            catch (Exception ex) {
                
                // Om det uppstår ett fel under processen, fångas undantaget och ett felmeddelande skrivs ut
                Console.WriteLine($"Error retrieving balance: {ex.Message}");
            }

            // Skickar tillbaka den beräknade balansen
            return currentBalance;
           
        }
    }
}
