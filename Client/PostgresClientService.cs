using System;
using System.Threading.Tasks;
using Npgsql;
using Client;

namespace Client
{
    public class PostgresClientService : IClientService
    {
        public static Clients? LoggedInUser = null;

        public readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        private readonly UserRegistrationService UserRegistrationObject = new UserRegistrationService();
        private readonly UserLoginService UserLoginObject = new UserLoginService();

        // Register new user
        public async Task<Clients?> RegisterNewUser(string username, string passwordhash, string email)
        {
            try
            {
                Console.WriteLine("Enter your username: ");
                string UserUsername = Console.ReadLine()!.ToLower();

                Console.WriteLine("Enter your password: ");
                string UserPassword = Console.ReadLine()!.ToLower();

                Console.WriteLine("Enter your email: ");
                string UserEmail = Console.ReadLine()!.ToLower();

                var newUser = await UserRegistrationObject.RegisterNewUser(UserUsername, UserPassword, UserEmail);

                return newUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during registration: {ex.Message}");
                return null;
            }
        }

        // Login user
        public async Task<Clients?> UserLogin(string usernameOrEmail, string password)
        {
            try
            {
                Console.WriteLine("Enter your username or email: ");
                string UserInput = Console.ReadLine()!.ToLower();

                Console.WriteLine("Enter your password: ");
                string UserPassword = Console.ReadLine()!.ToLower();

                var loggedInUser = await UserLoginObject.UserLogin(UserInput, UserPassword);

                if (loggedInUser != null)
                {
                    Console.Clear();
                    Console.WriteLine($"Successfully logged in as: {loggedInUser.username}");
                    LoggedInUser = loggedInUser;  // Update the logged-in user
                }
                else
                {
                    Console.WriteLine("Failed to log in. Please check your credentials.");
                }

                return loggedInUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                return null;
            }
        }

        // Switch user
        public async Task<Clients?> SwitchUser(string usernameOrEmail, string passwordhash)
        {
            try
            {
                // If a user is logged in, ask if they want to switch
                if (LoggedInUser != null)
                {
                    Console.WriteLine($"You are currently logged in as: {LoggedInUser.username}");
                    Console.WriteLine("Do you want to switch to another user? (Y/N)");
                    string switchChoice = Console.ReadLine()!.ToUpper();

                    if (switchChoice == "Y")
                    {
                        // Log out the current user
                        LoggedInUser = null;
                        Console.WriteLine("Logged out successfully.");
                        
                    }
                    else if (switchChoice == "N")
                    {
                        Console.WriteLine("Staying logged in as the current user.");
                        return LoggedInUser;  // Return the current user if they don't want to switch
                    }
                }

                // Ask the user to log in or register if no user is logged in
                Console.WriteLine("Do you want to log in with an existing account or register a new user? (1/2)");
                Console.WriteLine("\n1. Log in");
                Console.WriteLine("2. Register new user");
                string choice = Console.ReadLine()!.ToLower();

                if (choice == "1")
                {
                    // Login with passed credentials (if available)

                    return await UserLogin(usernameOrEmail, passwordhash);  // Login with passed credentials from Main method
                }
                else if (choice == "2")
                {
                    // Register new user with passed credentials
                   

                    return await RegisterNewUser("", "", "");  // Register new user (email will be passed in the future)
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please select 1 or 2.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during user switch: {ex.Message}");
                return null;
            }
        }

        // Log out the current user
        public Clients? UserLogout()
        {
            Console.Clear();
            Console.WriteLine("Logging out...");
            LoggedInUser = null;  // Reset the logged-in user
            return null;
        }
    }
}
