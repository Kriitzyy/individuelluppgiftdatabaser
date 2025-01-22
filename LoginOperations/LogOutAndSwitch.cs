using System;
using System.Threading.Tasks;
using Npgsql;
using CoreofApplication;
using Client;

namespace Client {
    public class UserLogoutService {

        private readonly PostgresClientService _postgresClientService = new PostgresClientService();

        // Log out the current user
        public void UserLogout() {
            // Instead of querying the database, check the global logged-in user
            if (Program.LoggedInUser != null) {
                // Reset the global logged-in user
                Program.LoggedInUser = null;
                Console.WriteLine("You have been logged out successfully.");
            }
            else {
                Console.WriteLine("No user is currently logged in.");
            }
        }

        // Log in as a new user after logging out the current user
        public async Task<bool> LogoutAndSwitchUser(string usernameOrEmail, string password) {
            int Choice; 
            
            if (Program.LoggedInUser != null) {

                UserLogout();

                Console.ReadKey();
            }
            else {
                
                Console.WriteLine("No user is logged in.. try again");
                return false; 
            }

            // Call UserLogout method to log out current user first

            Console.WriteLine("Logging out... Press any key to continue!");
            Console.ReadKey();

            Console.WriteLine("[1] - Register user");
            Console.WriteLine("[2] - Login");
            string UserInput = Console.ReadLine();

            if (int.TryParse(UserInput, out Choice)) {

                if (Choice == 1) {
                    // Register a new user
                    Console.WriteLine("Enter a username:");
                    string username = Console.ReadLine();
                    Console.WriteLine("Enter a password:");
                    string passwordhash = Console.ReadLine();
                    Console.WriteLine("Enter an email:");
                    string email = Console.ReadLine();

                    var userRegistrationService = new UserRegistrationService();
                    var registeredUser = await userRegistrationService.RegisterNewUser(username, passwordhash, email);

                    if (registeredUser != null) {
                        
                        Console.WriteLine(" ");

                        // Update the global logged-in user in Program
                        Program.LoggedInUser = registeredUser; 

                        return true;
                    }
                    else {
                        Console.WriteLine("Registration failed.. try again");
                        return false; 
                    }
                }
                else if (Choice == 2) {
                    // Log in an existing user
                    Console.WriteLine("Enter username or email:");
                    string loginUsernameOrEmail = Console.ReadLine();
                    Console.WriteLine("Enter password:");
                    string loginPassword = Console.ReadLine();

                    var userLoginService = new UserLoginService();
                    var loggedInUser = await userLoginService.UserLogin(loginUsernameOrEmail, loginPassword);

                    if (loggedInUser != null) {

                        Console.WriteLine($"You have successfully logged in as {loggedInUser.username}");

                        // Update the global logged-in user in Program
                        Program.LoggedInUser = loggedInUser; 
                        return true;  // Return true if login is successful
                    }
                    else {
                        Console.WriteLine("Login failed. Please check your credentials.");
                        return false;  // Return false if login fails
                    }
                }
                else {
                    Console.WriteLine("Ensure typing the right number!");
                }
            }
            else {
                Console.WriteLine("Choose between 1-2!");
            }

            return false; 
        }

        // Get the current logged-in user directly from Program
        public Clients? GetLoggedInUser() {
            return Program.LoggedInUser; // Use the global logged-in user from Program
        }
    }
}
