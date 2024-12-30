using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;

namespace Client  {

    public class PostgresClientService : IClientService {
        private static Clients? LoggedInUser = null; // Håller reda på den inloggade användaren

        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        // Lägg till en instans av UserRegistrationService
        private readonly UserRegistrationService UserRegistrationObject = new UserRegistrationService();

        // Använd den nya tjänsten för att registrera användare
        public async Task<Clients?> RegisterNewUser(string username, string passwordhash, string email) {
            // Anropa RegisterNewUser-metoden i UserRegistrationService
            return await UserRegistrationObject.RegisterNewUser(username, passwordhash, email);
        }

        // Implementera UserLogin
        public Clients? UserLogin(string username, string passwordhash) {
            // Login logik här
            return null; // Lägg till detta för att matcha returtypen
        }

        // Implementera UserLogout
        public void UserLogout() { 
            LoggedInUser = null; 
        }

        // Implementera GetLoggedInUser
        public Clients? GetExistingUser() { // Ändrad metodnamn för att matcha gränssnittet
            return LoggedInUser;
        }
    }
}