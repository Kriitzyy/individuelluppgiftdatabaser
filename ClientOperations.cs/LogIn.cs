using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;
// Ändrad

namespace Client
{
    public class UserLoginService
    {
        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        public async Task<Clients?> UserLogin(string usernameOrEmail, string password)
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    // Query to get user by username or email
                    using (var query = new NpgsqlCommand("SELECT Id, username, passwordhash, email FROM clients " +
                                                         "WHERE username = @usernameOrEmail OR email = @usernameOrEmail", connection))
                    {
                        query.Parameters.AddWithValue("@usernameOrEmail", usernameOrEmail);

                        using (var reader = await query.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Create a Clients object from database values
                                var clientData = new Clients
                                {
                                    Id = reader.GetInt32(0),
                                    username = reader.GetString(1),
                                    passwordhash = reader.GetString(2),
                                    email = reader.GetString(3)
                                };

                                // Verify the password using BCrypt
                                if (BCrypt.Net.BCrypt.Verify(password, clientData.passwordhash))
                                {
                                    Console.WriteLine("Password verified.");
                                    return clientData;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid password.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No user found with the provided username or email.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error during login: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            return null; // Return null if login failed
        }
    }
}
