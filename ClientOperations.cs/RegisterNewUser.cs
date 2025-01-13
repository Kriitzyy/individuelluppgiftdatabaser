using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;
// Ändrad
namespace Client
{
    public class UserRegistrationService
    {
        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        public async Task<Clients?> RegisterNewUser(string username, string passwordhash, string email)
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    // Hash the password before inserting into the database
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordhash);
                    Console.WriteLine($"Hashed password (stored): {hashedPassword}");  // Log for debugging

                    // Insert the user into the clients table
                    using (var query = new NpgsqlCommand("INSERT INTO clients (username, passwordhash, email) " +
                        "VALUES (@username, @passwordhash, @email) RETURNING Id, username, passwordhash, email", connection))
                    {
                        query.Parameters.AddWithValue("@username", username);
                        query.Parameters.AddWithValue("@passwordhash", hashedPassword);
                        query.Parameters.AddWithValue("@email", email);

                        using (var reader = await query.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var clientData = new Clients
                                {
                                    Id = reader.GetInt32(0),
                                    username = reader.GetString(1),
                                    passwordhash = reader.GetString(2),
                                    email = reader.GetString(3)
                                };

                                Console.WriteLine($"User Registered: {clientData.username}, ID: {clientData.Id}");
                                return clientData;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception message for troubleshooting
                Console.WriteLine($"Error occurred during registration: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            return null;  // If no user was registered, return null
        }
    }
}
