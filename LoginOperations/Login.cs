using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;
using Client;
using System.Diagnostics; // Make sure the Clients class is referenced

namespace Client {

    public class UserLoginService {
        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        public async Task<Clients?> UserLogin(string username, string passwordhash, string email) {
            
            try {
                using (var Connection = new NpgsqlConnection(ConnectionString)) {

                    await Connection.OpenAsync();

                    // SQL query that checks for either username or email
                    string SqlQuery = @"SELECT id, username, passwordhash, email 
                    FROM clients WHERE username = @username OR email = @email";

                    using (var LoginCommand = new NpgsqlCommand(SqlQuery, Connection)) {

                        // Add parameters for both username and email
                        LoginCommand.Parameters.AddWithValue("@username", username ?? string.Empty);
                        LoginCommand.Parameters.AddWithValue("@email", email ?? string.Empty);

                        using (var reader = await LoginCommand.ExecuteReaderAsync()) {

                            if (await reader.ReadAsync()) {

                                string StoredHashPassword = reader["passwordhash"].ToString();

                                // Compare entered plain password with stored hashed password
                                if (BCrypt.Net.BCrypt.Verify(passwordhash, StoredHashPassword)) {

                                    var LoggedInUser = new Clients {

                                        Id = Convert.ToInt32(reader["id"]),
                                        username = reader["username"].ToString(),
                                        passwordhash = StoredHashPassword,
                                        email = reader["email"].ToString()
                                    };

                                    Console.WriteLine("The login is successful!");
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
