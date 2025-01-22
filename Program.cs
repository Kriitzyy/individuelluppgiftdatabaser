using System;
using System.Threading.Tasks;
using CoreofApplication;
using Client;

namespace CoreofApplication
{
    class Program
    {
        public static Clients? LoggedInUser = null; // Holds the current logged-in user state

        public static async Task Main(string[] args)
        {
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
                        await userService.CallingRegisterUser();
                    }
                    else if (LoginChoice == 2) // Log in an existing user
                    {
                        var registeredUser = await userService.CallingLoginUser();

                        if (Program.LoggedInUser != null) // Check against UserService.LoggedInUser
                        {
                            Console.WriteLine($"Login is successful! Welcome {Program.LoggedInUser.username}.");
                            LoginBool = true; // Exit the login menu loop after successful login
                        }
                        else
                        {
                            Console.WriteLine("Login failed, please try again.");
                        }
                    }
                    else if (LoginChoice == 3) // Log out the current user and switch
                    {
                        await userService.CallingUserChange();

                        if (Program.LoggedInUser != null)
                        {
                            Console.WriteLine($"Successfully switched to {Program.LoggedInUser.username}.");
                        }
                        else
                        {
                            Console.WriteLine("No user is logged in after switching.");
                        }
                    }
                    else if (LoginChoice == 4) // Log out the current user
                    {
                        userService.CallingLogoutUser();

                        if (Program.LoggedInUser == null)
                        {
                            Console.WriteLine("You have successfully logged out.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid option (1-4).");
                }
            }

            // Main menu loop
            bool stillRunning = true;
            while (stillRunning)
            {
                Console.Clear();
                DisplayMenu.DisplayMainMenu();

                if (Program.LoggedInUser == null)
                {
                    Console.WriteLine("Please log in to access the main menu.");
                    break; // Exit the loop and redirect to login
                }
                
                string GetUserInput = Console.ReadLine()!.ToLower();
                int usersMenuOptions;
                

                if (int.TryParse(GetUserInput, out usersMenuOptions))
                {
                    switch (usersMenuOptions)
                    {
                        case 1:
                            await RunTransactions.CallingDeposit();
                            break;

                        case 2:
                            await RunTransactions.CallingDelete();
                            break;

                        case 3:
                            await RunTransactions.CallingCurrentBalance();
                            break;

                        case 4:
                            await RunTransactions.CallingMoneySpent();
                            break;

                        case 5:
                            await RunTransactions.CallingIncomePeriods();
                            break;

                        case 6:
                            DisplayMenu.DisplayUserNeedHelp();
                            break;

                        case 7:
                            Console.Clear();
                            Console.WriteLine("Exiting SecureSwe Bank...");
                            stillRunning = false;
                            return;

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
