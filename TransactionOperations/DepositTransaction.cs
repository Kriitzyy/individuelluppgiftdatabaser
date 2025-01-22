using System;
using System.Threading.Tasks;
using Npgsql;
using CoreofApplication;
using Client;

// Denna fil hanterar så användaren kan lägga till 
// pengar i tabellen

namespace CoreofApplication {

    // Deposit class
    public class DepositClass {

        // Metod som hanterar insättning av pengar i tabellen
        public static async Task<bool> DepositTransactions(Transaction transaction) {
            
            try {
                
                // Hämtar connection metod
                using (var connection = Connection.GetConnection()) {
                    
                    // Öppnar connection
                    await connection.OpenAsync(); 

                    // SQL Query som lägger in pengarna i tabellen och kolumnerna 
                    string sqlQuery = "INSERT INTO transactions (client_id, transaction_date, amount, description) " +
                                      "VALUES (@client_id, @transaction_date, @amount, @description)";

                    using (var cmd = new NpgsqlCommand(sqlQuery, connection)) {

                        // Lägger till parametrar till SQL-kommandot från Transaction objektet
                        cmd.Parameters.AddWithValue("@client_id", transaction.ClientId); 
                        cmd.Parameters.AddWithValue("@transaction_date", DateTime.Now); // Datum now (Nuet)
                        cmd.Parameters.AddWithValue("@amount", transaction.amount); 
                        cmd.Parameters.AddWithValue("@description", transaction.source);

                        // Kör SQL-kommandon
                        int rowsAffected = await cmd.ExecuteNonQueryAsync(); // Utför kommandot

                        // Returnera true om transaktionen 
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex) {

                // Om ett fel inträffar fångas det
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false; // Returnera false om något gick fel
            }
        }
    }
}
