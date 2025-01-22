using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;
using Client;

// Denna fil hanterar användarens logga in med email eller username
// Användaren måste existera i databas för inloggning

namespace Client {

    // Klass som ansvarar för inloggningen
    public class UserLoginService { 

        // Metod för användarinloggning
        public async Task<Clients?> UserLogin(string usernameOrEmail, string password) {

            try {

                // Skapar en databasanslutning med hjälp av Connection-klassen - metod
                using (var connection = Connection.GetConnection()) { 

                    // Öppnar connection
                    await connection.OpenAsync(); 

                    // SQL-fråga för att hitta användare baserat på deras användarnamn eller e-post
                    string sqlQuery = @"SELECT id, username, passwordhash, email 
                                        FROM clients WHERE username = @username OR email = @username";

                    // Skapar ett objekt för att köra SQL-frågan
                    using (var loginCommand = new NpgsqlCommand(sqlQuery, connection)) {

                        // Lägger till parameter för både användarnamn eller email
                        loginCommand.Parameters.AddWithValue("@username", usernameOrEmail ?? string.Empty);

                        // kör kommando och läser in resultaten
                        using (var reader = await loginCommand.ExecuteReaderAsync()) {

                            // Kollar om det finns en användare som matchar
                            if (await reader.ReadAsync()) {

                                // Hämtar det hashade lösenordet från databasen
                                string storedHashPassword = reader["passwordhash"].ToString(); // Läser koden

                                // Verifierar att det angivna lösenordet matchar det hashade lösenordet i databasen
                                if (BCrypt.Net.BCrypt.Verify(password, storedHashPassword)) {

                                    // Om lösenordet stämmer, skapas ett Clients-objekt för inloggad användare
                                    var loggedInUser = new Clients {

                                        Id = Convert.ToInt32(reader["id"]),
                                        username = reader["username"].ToString(),
                                        passwordhash = storedHashPassword,
                                        email = reader["email"].ToString()
                                    };

                                    Console.WriteLine("The login is successful!");
                                    return loggedInUser;  // Returnerar den inloggade användaren
                                }
                            }
                            else {

                                // Om ingen användare hittades
                                Console.WriteLine("The user was not found.. try again!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {

                // Om operationen misslyckas finns det en catch

                Console.WriteLine($"Error during login: {ex.Message}");
            }

            return null;
        }
    }
}
