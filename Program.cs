using System;
using System.Threading.Tasks;
using CoreofApplication;
using Client;

namespace CoreofApplication
{
    class Program
    {
        public static async Task Main(string[] args)
        {

            var transaction = new Transaction
            {
                ClientId = 2,  // The Client ID to retrieve balance for
                amount = 100.00m,  // Example amount (not necessary for retrieving balance)
                source = "Loan"  // Example description (not necessary for retrieving balance)
            };

            // Instantiate the GetTransaction class
            var getTransaction = new GetTransaction();

            // Get the current balance for the client
            decimal balance = await getTransaction.GetCurrentBalance(transaction);

            // Print the balance to the console
            Console.WriteLine($"The current balance for client {transaction.ClientId} is: {balance:C}");

            // Initialize services
            Connection.TestConnection();
            var postgresClientService = new PostgresClientService();
            var userLogoutService = new UserLogoutService();
            var userService = new UserService(postgresClientService, userLogoutService);

            int LoginChoice;
            bool LoginBool = false;

            // Login menu loop
            while (!LoginBool)
            {
                DisplayMenu.DisplayLoginOptions(); // Display login options

                string LogInInput = Console.ReadLine()!.ToLower();

                if (int.TryParse(LogInInput, out LoginChoice))
                {
                    if (LoginChoice == 1) // Register a new user
                    {
                        await userService.RegisterUser();
                    }
                    else if (LoginChoice == 2) // Log in an existing user
                    {
                        await userService.LoginUser();
                        LoginBool = true; // Login successful
                    }
                    else if (LoginChoice == 3) // Log out the current user and switch
                    {
                        await userService.SwitchUser();
                    }
                    else if (LoginChoice == 4) // Log out the current user
                    {
                        userService.LogoutUser();
                        LoginBool = false; // Exit the login loop
                    }
                }
            }

            // Main menu loop
            bool stillrunning = true;
            while (stillrunning)
            {
                Console.Clear();
                DisplayMenu.DisplayMainMenu();

                string GetUserInput = Console.ReadLine()!.ToLower();
                int usersmenuoptions;

                if (int.TryParse(GetUserInput, out usersmenuoptions))
                {
                    switch (usersmenuoptions)
                    {
                        case 1:
                            Console.WriteLine("Transaction operations here");
                            break;

                        case 2:
                            Console.WriteLine("Transaction operations here");
                            break;

                        case 3:
                            Console.WriteLine("Transaction operations here");
                            break;

                        case 4:
                            Console.WriteLine("Transaction operations here");
                            break;

                        case 5:
                            Console.WriteLine("Transaction operations here");
                            break;

                        case 6:
                            DisplayMenu.UserNeedHelp();
                            break;

                        case 7:
                            Console.Clear();
                            Console.WriteLine("Exiting SecureSwe Bank...");
                            stillrunning = false;
                            break;

                        default:
                            Console.WriteLine("Please ensure entering a choice between 1-7!");
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please, ensure you are choosing between 1-7!");
                }

                Console.WriteLine("\nPress any key to continue to the menu...");
                Console.ReadKey();
            }
        }
    }
}
