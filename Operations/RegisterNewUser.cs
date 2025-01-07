using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;

namespace Client {

    public class UserRegistrationService {

        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        // Flyttad RegisterNewUser-metod
        public async Task<Clients?> RegisterNewUser(string username, string passwordhash, string email) {

            try {

                using (var Connection = new NpgsqlConnection(ConnectionString)) {
                    
                    await Connection.OpenAsync();  // Använd async-metod för att öppna anslutningen asynkront
                    
                    string UsersPassword = "Password.Secured";

                    string UserPassword = BCrypt.Net.BCrypt.HashPassword(passwordhash); 

                    using (var Query = new NpgsqlCommand("INSERT INTO clients (username, passwordhash, email) VALUES" 
                        + "(@username, @passwordhash, @email) RETURNING Id, username, passwordhash, email", Connection)) {

                        Query.Parameters.AddWithValue("@username", username);
                        Query.Parameters.AddWithValue("@passwordhash", UserPassword);
                        Query.Parameters.AddWithValue("@email", email);

                        using (var Reader = await Query.ExecuteReaderAsync()) {
                            if (await Reader.ReadAsync()) {  // Läs en gång för att få resultatet

                                var ClientData = new Clients {
                                    Id = Reader.GetInt32(0), // Hämtar ID från databasens första kolumn
                                    username = Reader.GetString(1), // Hämtar användarnamn från andra kolumn
                                    passwordhash = Reader.GetString(2), // Hämtar hashad lösenord från tredje kolumn
                                    email = Reader.GetString(3) // Hämtar e-post från fjärde kolumn
                                };

                                FileManager.SaveUserToFile(ClientData.username, ClientData.passwordhash, ClientData.email);

                                Console.WriteLine($"User Registered: {ClientData.username}, ID: {ClientData.Id}");
                                return ClientData;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                // Ifall programmet kraschar får användaren meddelande och varför det hände
                Console.WriteLine("Error has occured " + ex.Message); 
            }

            return null;  // null om inget resultat hittades
        }
    }
}