using System;
using System.Threading.Tasks;
using CoreofApplication;
using Npgsql;

namespace Client {
    // Klassen implementerar ett klientservicegränssnitt
    public class PostgresClientService : IClientService {

        // Anslutningssträng för PostgreSQL-databasen
        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        // Objekt för användarregistreringstjänsten
        private readonly UserRegistrationService UserRegistrationObject = new UserRegistrationService();

        // Objekt för användarinloggningstjänsten
        private readonly UserLoginService UserLoginObject = new UserLoginService();

        // Register new user metod
        public async Task<Clients?> RegisterNewUser(string username, string passwordhash, string email) { 

            return await UserRegistrationObject.RegisterNewUser(username, passwordhash, email);
        }

        // Metod för användarinloggning
        public async Task<Clients?> UserLogin(string usernameOrEmail, string password) {
            // Anropar UserLoginService för att logga in användaren
            return await UserLoginObject.UserLogin(usernameOrEmail, password);
        }

        // Metod för att logga ut och byta till en annan användare (ej implementerad)
        public async Task<Clients?> LogoutAndSwitchUser(string usernameOrEmail, string password) {
            
            // Returnerar null då funktionen inte är implementerad
            return null;
        }

        // Metod för att logga ut användaren
        public Clients? UserLogout() {
            // Sätter den inloggade användaren i Program till null, vilket betyder att ingen användare är inloggad
            Program.LoggedInUser = null;
            return null;
        }

    }
}
