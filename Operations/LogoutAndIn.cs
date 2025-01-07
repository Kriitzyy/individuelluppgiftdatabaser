using System;
using System.Threading.Tasks;
using Npgsql;

// Testa denna koden, för den verkar att du ska logga in på en samma användare istället för en ny (meningen är att byta användare)
namespace Client {
    public class UserLogoutService {

        // Skapa instans av PostgresClientService för att logga in en annan användare
        private readonly PostgresClientService _postgresClientService = new PostgresClientService();

        // Logga ut användaren genom att sätta LoggedInUser till null
        public void UserLogout() {

            if (_postgresClientService != null) {
                
                // Sätt användaren till null (logout), den är nu null i PostgresClientService
                _postgresClientService.UserLogout();
                Console.WriteLine("You have been logged out successfully.");
            }
            else
            {
                Console.WriteLine("No user is logged in.");
            }
        }

        // Logga in som en annan användare (efter utloggning)
        public async Task<bool> UserChangeLogin(string username, string passwordhash, string email)
        {
            // Första steget: Logga ut den nuvarande användaren och byt till en ny användare
            var newUser = await _postgresClientService.LogoutAndSwitchUser(username, passwordhash, email);

            if (newUser != null)
            {
                Console.WriteLine($"You have successfully logged in as {newUser.username}");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to log in with the provided credentials.");
                return false;
            }
        }

        // Hämta den nuvarande inloggade användaren
        public Clients? GetLoggedInUser()
        {
            return _postgresClientService.GetLoggedInUser();
        }
    }
}
