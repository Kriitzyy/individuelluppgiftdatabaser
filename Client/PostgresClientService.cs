using System;
using System.Threading.Tasks;
using Npgsql;

namespace Client {

    public class PostgresClientService : IClientService {

        private static Clients? LoggedInUser = null;
        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        private readonly UserRegistrationService UserRegistrationObject = new UserRegistrationService();
        private readonly UserLoginService UserLoginObject = new UserLoginService();

        // Register new user
        public async Task<Clients?> RegisterNewUser(string username, string passwordhash, string email) { 

           // return await UserRegistrationObject.RegisterNewUser(username, passwordhash, email);
           return null;
        }

        // User login
        public async Task<Clients?> UserLogin(string usernameOrEmail, string password) {

            return await UserLoginObject.UserLogin(usernameOrEmail, password);
        }

        // Logout and switch to another user
        public async Task<Clients?> LogoutAndSwitchUser(string usernameOrEmail, string password) {
            
                        return null;
            }

        // Log out the user
        public Clients? UserLogout() {

            LoggedInUser = null;
            return null;
        }

        // Get the current logged-in user
        public Clients? GetLoggedInUser() {
            
            return LoggedInUser;
        }
    }
}
