using System;
using Npgsql;
using CoreofApplication;
using System.Threading.Tasks;
using Client;

// Denna fil hanterar om användaren vill radera transaktioner 

namespace CoreofApplication {

    // Delete class
    public class DeleteTransaction { 

        // Delete metod
        public static async Task DeleteTransactions(Transaction transaction) {

            try {     

                // Hämtar Connection metod
                using (var connection = Connection.GetConnection()) { 
                    
                    // Öppnar connection
                    await connection.OpenAsync(); 

                    // SQL query som raderar transaktioner relaterad till användaren 
                    // Och kontrollerar om det är samma id nummer
                    string deleteQuery = "DELETE FROM transactions WHERE client_id = @client_id";

                    using (var cmd = new NpgsqlCommand(deleteQuery, connection)) {

                        // Använder ClientId från Transaction objektet för att göra queryn
                        cmd.Parameters.AddWithValue("@client_id", transaction.ClientId);

                        // Kör queryn och får tillbaka antalet rader som påverkades
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        // Om det finns transaktioner kopplade till klientens id 
                        if (rowsAffected > 0) { 

                            // Meddelande om att transaktionerna raderades
                            Console.WriteLine("Transactions deleted successfully! Press any key to continue.."); 
                            Console.ReadKey();
                        }
                        else {
                            
                            // Meddelande om att inga transaktioner hittades
                            Console.WriteLine("No transactions found for the specified client ID.");
                        }
                    }
                }
            }
            catch (Exception ex) {
                
                // Om operationen misslyckas fångas undantaget och ett felmeddelande skrivs ut
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
