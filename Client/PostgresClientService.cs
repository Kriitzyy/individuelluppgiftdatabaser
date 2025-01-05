using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;

// Denna filen hanterar alla operationer som är relaterade till databasen
namespace Client {
    
    public class PostgresClientService : IClientService {
        private static Clients? LoggedInUser = null; // Håller reda på den inloggade användaren

        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        // Lägg till en instans av UserRegistrationService
        private readonly UserRegistrationService UserRegistrationObject = new UserRegistrationService();

        // Lägg till en instans av UserLoginService
        private readonly UserLoginService UserLoginObject = new UserLoginService();

        // Använd den nya tjänsten för att registrera användare
        public async Task<Clients?> RegisterNewUser(string username, string passwordhash, string email) {
            // Anropa RegisterNewUser-metoden i UserRegistrationService
            return await UserRegistrationObject.RegisterNewUser(username, passwordhash, email);
        }

        // Använd UserLoginService för inloggning
        public async Task<Clients?> UserLogin(string username, string passwordhash, string email) {
            // Anropa UserLogin-metoden från UserLoginService
            return await UserLoginObject.UserLogin(username, passwordhash, email);
        }

        // Logga ut och byt användare
        public Clients? LogoutAndSwitchUser(string username, string passwordhash) {

            return null; 
            LoggedInUser = null;
            Console.WriteLine("Logged out. You can now switch to another user.");
        }

        // Implementera GetLoggedInUser
        public Clients? UserLogout() {
            return null;
        }
    }
}
