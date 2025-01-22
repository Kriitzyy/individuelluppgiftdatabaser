using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;
using Client;

namespace Client
{
    public class UserLoginService
    {
        // Kopplar till Connection.GetConnection() och använder den för att öppna anslutning
        public async Task<Clients?> UserLogin(string usernameOrEmail, string password)
        {
            try
            {
                using (var connection = Connection.GetConnection()) // Använd Connection.GetConnection() istället
                {
                    await connection.OpenAsync();

                    // SQL-fråga som kollar efter antingen användarnamn eller email
                    string sqlQuery = @"SELECT id, username, passwordhash, email 
                                        FROM clients WHERE username = @username OR email = @username";

                    using (var loginCommand = new NpgsqlCommand(sqlQuery, connection))
                    {
                        // Lägg till parameter för både användarnamn eller email
                        loginCommand.Parameters.AddWithValue("@username", usernameOrEmail ?? string.Empty);

                        using (var reader = await loginCommand.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                string storedHashPassword = reader["passwordhash"].ToString();

                                // Jämför inmatat lösenord med det lagrade hashade lösenordet
                                if (BCrypt.Net.BCrypt.Verify(password, storedHashPassword))
                                {
                                    var loggedInUser = new Clients
                                    {
                                        Id = Convert.ToInt32(reader["id"]),
                                        username = reader["username"].ToString(),
                                        passwordhash = storedHashPassword,
                                        email = reader["email"].ToString()
                                    };

                                    Console.WriteLine("The login is successful!");
                                    return loggedInUser;
                                }
                            }
                            else
                            {
                                Console.WriteLine("The user was not found.. try again!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
            }

            return null;
        }
    }
}
