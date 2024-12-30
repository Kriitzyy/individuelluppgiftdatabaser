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

                    string UserPassword = BCrypt.Net.BCrypt.HashPassword(passwordhash);

                    using (var cmd = new NpgsqlCommand("INSERT INTO Clients (username, passwordhash, email) VALUES"
                        + "(@username, @password, @email) RETURNING Id, username, passwordhash, email", Connection)) {

                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", UserPassword);
                        cmd.Parameters.AddWithValue("@email", email);

                        using (var Reader = await cmd.ExecuteReaderAsync()) {
                            if (await Reader.ReadAsync()) {  // Läs en gång för att få resultatet

                                var ClientData = new Clients {
                                    Id = Reader.GetInt32(0), // Hämtar ID från databasens första kolumn
                                    username = Reader.GetString(1), // Hämtar användarnamn från andra kolumn
                                    passwordhash = Reader.GetString(2), // Hämtar hashad lösenord från tredje kolumn
                                    email = Reader.GetString(3) // Hämtar e-post från fjärde kolumn
                                };

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