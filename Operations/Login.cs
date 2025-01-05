using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;

namespace Client {

    public class UserLoginService {
        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        public async Task<Clients?> UserLogin(string username, string passwordhash, string email) {
            try {
                using (var Connection = new NpgsqlConnection(ConnectionString)) {

                    await Connection.OpenAsync();
                    // SQL-fråga som söker efter användaren baserat på username eller email
                    string SqlQuery = @"SELECT id, username, passwordhash, email 
                    FROM clients WHERE username = @username OR email = @email";

                    using (var LoginCommand = new NpgsqlCommand(SqlQuery, Connection)) {

                        LoginCommand.Parameters.AddWithValue("@username", username ?? string.Empty);
                        LoginCommand.Parameters.AddWithValue("@email", email ?? string.Empty);

                        using (var reader = await LoginCommand.ExecuteReaderAsync()) {

                            if (await reader.ReadAsync()) {

                                string StoredHashPassword = reader["passwordhash"].ToString();

                                if (BCrypt.Net.BCrypt.Verify(passwordhash, StoredHashPassword)) {

                                    var LoggedInUser = new Clients {

                                        Id = Convert.ToInt32(reader["id"]),
                                        username = reader["username"].ToString(),
                                        passwordhash = StoredHashPassword,
                                        email = reader["email"].ToString()
                                    };

                                    Console.WriteLine("The login is successful!");
                                    
                                    Console.WriteLine("\nPress any key to continue....");
                                    return LoggedInUser;
                                }
                                else {
                                    Console.WriteLine("The password was wrong.. try again!");
                                }
                            }
                            else {
                                Console.WriteLine("The user was not found.. try again!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {

                Console.WriteLine($"Error during login: {ex.Message}");
            }

            return null;
        }
    }
}