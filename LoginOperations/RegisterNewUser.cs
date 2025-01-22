using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;

// Denna filen används för att registerera användaren
// OBS Ny användare bara 

namespace Client {

    public class UserRegistrationService {
        
        // Metod för användar registrering
        public async Task<Clients?> RegisterNewUser(string username, string passwordhash, string email) {

            try {

                // Hämtar anslutningen via metod i klassen Connection
                using (var connection = Connection.GetConnection()) { 
                    
                    // Använder metod för att öppna anslutningen
                    await connection.OpenAsync(); 
                    
                    string UserPassword = BCrypt.Net.BCrypt.HashPassword(passwordhash); // Hashar koden med BCrypt

                    // SQL query som lägger in användarens username, password och email i tabellen clients.
                    using (var Query = new NpgsqlCommand("INSERT INTO clients (username, passwordhash, email) VALUES" 
                        + "(@username, @passwordhash, @email) RETURNING Id, username, passwordhash, email", connection)) {
                            
                        // Lägg till parametrar till SQL för att undvika SQL injection
                        Query.Parameters.AddWithValue("@username", username);
                        Query.Parameters.AddWithValue("@passwordhash", UserPassword);
                        Query.Parameters.AddWithValue("@email", email);

                        // Hämtar resultaten från databasen 
                        using (var Reader = await Query.ExecuteReaderAsync()) {
                            
                            // Läs en gång för att få resultatet
                            if (await Reader.ReadAsync()) { 

                                var ClientData = new Clients {
                                    
                                    Id = Reader.GetInt32(0), // Hämtar ID från databasens första kolumn
                                    username = Reader.GetString(1), // Hämtar användarnamn från andra kolumn
                                    passwordhash = Reader.GetString(2), // Hämtar hashad lösenord från tredje kolumn
                                    email = Reader.GetString(3) // Hämtar e-post från fjärde kolumn

                                };

                                // Skriver ut när användaren blivit registrerad
                                Console.WriteLine($"User Registered: {ClientData.username}, ID: {ClientData.Id}");
                                return ClientData; // Retunerar användaren
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                
                // Ifall programmet kraschar får användaren meddelande och varför det hände
                // Får en felmeddelande om något går fel
                Console.WriteLine("Error has occured " + ex.Message); 
            }

            return null;  // null om inget resultat hittades, alltså om användaren inte registrerades
        }
    }
}
