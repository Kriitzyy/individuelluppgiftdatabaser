using System;
using System.Threading.Tasks;
using Npgsql;

namespace Client {

    public class UserLogoutService {

        private readonly PostgresClientService _postgresClientService = new PostgresClientService();

        // Log out the current user
        public void UserLogout() {

            if (_postgresClientService != null) {

                _postgresClientService.UserLogout();
                Console.WriteLine("You have been logged out successfully.");
            }
            else {

                Console.WriteLine("No user is currently logged in.");
            }
        }

        // Log in as a new user after logging out the current user
        public async Task<bool> UserChangeLogin(string username, string passwordhash, string email) {

            var newUser = await _postgresClientService.LogoutAndSwitchUser(username, passwordhash, email);

            if (newUser != null) {

                Console.WriteLine($"You have successfully logged in as {newUser.username}");
                return true;
            }
            else {

                Console.WriteLine("Failed to log in with the provided credentials.");
                return false;
            }
        }

        // Retrieve the current logged-in user
        public Clients? GetLoggedInUser() {
            
            return _postgresClientService.GetLoggedInUser();
        }
    }
}
