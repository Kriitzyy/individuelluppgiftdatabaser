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

            return await UserRegistrationObject.RegisterNewUser(username, passwordhash, email);
        }

        // User login
        public async Task<Clients?> UserLogin(string username, string passwordhash, string email) {

            return await UserLoginObject.UserLogin(username, passwordhash, email);
        }

        // Logout and switch to another user
        public async Task<Clients?> LogoutAndSwitchUser(string username, string passwordhash, string email) {

            LoggedInUser = null;
            Console.WriteLine("Logged out. You can now switch to another user.");

            var newUser = await UserLoginObject.UserLogin(username, passwordhash, email);

            if (newUser != null) {

                LoggedInUser = newUser;
                Console.WriteLine($"Successfully switched to user: {newUser.username}");
                return newUser;
            }
            else {

                Console.WriteLine("Failed to switch user. Invalid login credentials.");
                return null;
            }
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
