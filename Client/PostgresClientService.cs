using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;

namespace Client
{
    public class PostgresClientService : IClientService
    {
        private static Clients? LoggedInUser = null; // Håller reda på den inloggade användaren

        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        // Lägg till en instans av UserRegistrationService
        private readonly UserRegistrationService UserRegistrationObject = new UserRegistrationService();

        // Lägg till en instans av UserLoginService
        private readonly UserLoginService UserLoginObject = new UserLoginService();

        // Använd den nya tjänsten för att registrera användare
        public async Task<Clients?> RegisterNewUser(string username, string passwordhash, string email)
        {
            return await UserRegistrationObject.RegisterNewUser(username, passwordhash, email);
        }

        // Använd UserLoginService för inloggning
        public async Task<Clients?> UserLogin(string username, string passwordhash, string email)
        {
            return await UserLoginObject.UserLogin(username, passwordhash, email);
        }

        // Logga ut och byt användare
        public async Task<Clients?> LogoutAndSwitchUser(string username, string passwordhash, string email)
        {
            // Logga ut den nuvarande användaren
            LoggedInUser = null;
            Console.WriteLine("Logged out. You can now switch to another user.");

            // Försök att logga in med den nya användaren
            var newUser = await UserLoginObject.UserLogin(username, passwordhash, email);

            if (newUser != null)
            {
                // Om den nya användaren loggas in, sätt den som inloggad
                LoggedInUser = newUser;
                Console.WriteLine($"Successfully switched to user: {newUser.username}");
                return newUser;
            }
            else
            {
                Console.WriteLine("Failed to switch user. Invalid login credentials.");
                return null;
            }
        }

        // Logga ut användaren
        public Clients? UserLogout()
        {
            LoggedInUser = null;
            return null;
        }

        // Hämta den nuvarande inloggade användaren
        public Clients? GetLoggedInUser()
        {
            return LoggedInUser;
        }
    }
}
