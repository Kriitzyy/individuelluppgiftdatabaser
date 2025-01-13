using System; 
using System;
using System.Threading.Tasks;
using Client;


namespace Client
{
    public class UserSwitchService
    {
        private readonly PostgresClientService clientService = new PostgresClientService();

        // Switch user functionality
        public async Task<Clients?> SwitchUser()
        {
            try
            {
                // If a user is logged in, log them out before switching
                if (PostgresClientService.LoggedInUser != null)
                {
                    Console.WriteLine($"You are currently logged in as: {PostgresClientService.LoggedInUser.username}");
                    Console.WriteLine("Do you want to switch to another user? (Y/N)");
                    string switchChoice = Console.ReadLine()!.ToUpper();

                    if (switchChoice == "Y")
                    {
                        // Log out the current user
                        PostgresClientService.LoggedInUser = null;
                        Console.Clear();
                        Console.WriteLine("Logged out successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Staying logged in as the current user.");
                        return PostgresClientService.LoggedInUser;  // Return the current user if they don't want to switch
                    }
                }

                // Directly prompt for registration of a new user
                Console.WriteLine("You have logged out successfully.");
                Console.WriteLine("Proceeding to register a new user...");

                // Ask for registration details
                Console.WriteLine("Enter your desired username: ");
                string username = Console.ReadLine()!;
                Console.WriteLine("Enter your password: ");
                string password = Console.ReadLine()!;
                Console.WriteLine("Enter your email: ");
                string email = Console.ReadLine()!;

                // Register new user and return the registered client
                return await clientService.RegisterNewUser(username, password, email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during user switch: {ex.Message}");
                return null;
            }
        }
    }
}
