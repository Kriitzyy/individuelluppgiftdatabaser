using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreofApplication;
using Npgsql;
using BCrypt.Net;
using dotenv.net;
using Client;

namespace CoreofApplication
{
    class Program
    {
        public static List<UserTransaction> TransactionList = new List<UserTransaction>();
        public static Clients? LoggedInUser = null;  // Track logged-in user

        public static async Task Main(string[] args)
        {
            // Testing connection
            Connection.TestConnection();

            try
            {
                var clientService = new PostgresClientService();

                bool LoginBool = true;
                int UserChoice;

                while (LoginBool)
                {
                    Console.Clear();
                    DisplayMenu.DisplayLoginOptions();

                    string UserInput = Console.ReadLine()!.ToLower();

                    if (int.TryParse(UserInput, out UserChoice))
                    {
                        Console.Clear();

                        if (UserChoice == 1)
                        {
                            // Register new user
                            var registeredUser = await clientService.RegisterNewUser("", "", "");

                            if (registeredUser != null)
                            {
                                Console.WriteLine($"Registration successful! Welcome, {registeredUser.username}");
                            }
                            else
                            {
                                Console.WriteLine("Registration failed. Please try again.");
                            }
                        }
                        else if (UserChoice == 2)
                        {
                            // Login user
                            var loggedInUser = await clientService.UserLogin("", "");

                            if (loggedInUser != null)
                            {
                                Console.WriteLine($"Successfully logged in as {loggedInUser.username}");
                                LoggedInUser = loggedInUser;  // Set the logged-in user
                            }
                            else
                            {
                                Console.WriteLine("Login failed. Please check your credentials and try again.");
                            }
                        }
                        else if (UserChoice == 3)
                        {
                            // Switch user functionality
                            if (LoggedInUser != null)
                            {
                                Console.WriteLine("Do you want to switch to another user? (Y/N)");
                                string switchChoice = Console.ReadLine()!.ToUpper();

                                if (switchChoice == "Y")
                                {
                                    // Call the SwitchUser method from the PostgresClientService
                                    var newUser = await clientService.SwitchUser("", "");

                                    if (newUser != null)
                                    {   
                                        LoggedInUser = newUser;
                                        Console.WriteLine($"Successfully logged in as {newUser.username}.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to switch to a new user. Please try again.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Staying logged in.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No user is currently logged in.");
                            }
                        }
                        else if (UserChoice == 4)
                        {
                            // Logout current user
                            if (LoggedInUser != null)
                            {
                                Console.WriteLine($"Logging out {LoggedInUser.username}...");
                                clientService.UserLogout();
                                LoggedInUser = null;
                                Console.Clear();
                                Console.WriteLine("Successfully logged out.");
                            }
                            else
                            {
                                Console.WriteLine("No user is currently logged in.");
                            }
                        }
                        else if (UserChoice == 5)
                        {
                            // Exit the application
                            LoginBool = false;
                            Console.Clear();
                            Console.WriteLine("Exiting the application.");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Please enter a valid number (1-5).");
                    }

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Error during registration or login: {ex.Message}");
            }

            // Main user transaction loop
            bool stillRunning = true;
            int userMenuOptions;

            while (stillRunning)
            {
                Console.Clear();
                DisplayMenu.DisplayMainMenu();

                string getUserInput = Console.ReadLine()!.ToLower();

                if (int.TryParse(getUserInput, out userMenuOptions))
                {
                    switch (userMenuOptions)
                    {
                        case 1:
                            // Add your code here for handling a specific transaction/feature
                            break;

                        case 2:
                            // Money deposit
                            UserTransactionMethods.UsersMoneyDeposit(TransactionList);
                            break;

                        case 3:
                            // Delete transaction
                            UserTransactionMethods.UsersDeleteTransaction(TransactionList);
                            break;

                        case 4:
                            // View current balance
                            UserTransactionMethods.UsersCurrentBalance(TransactionList);
                            break;

                        case 5:
                            // View money spent
                            UserTransactionMethods.UsersMoneySpent(TransactionList);
                            break;

                        case 6:
                            // View money income
                            UserTransactionMethods.UsersMoneyIncome(TransactionList);
                            break;

                        case 7:
                            DisplayMenu.UserNeedHelp();
                            break;

                        case 8:
                            Console.Clear();
                            Console.WriteLine("Exiting SecureSwe Bank...");
                            stillRunning = false;
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid selection. Please choose a valid option between 1-7.");
                            break;
                    }
                }
                else
                {
                    // Handle invalid input from the user
                    Console.Clear();
                    Console.WriteLine("Invalid input. Please choose a valid option between 1-7.");
                }

                // Pause to allow the user to read the result
                Console.WriteLine("\nPress any key to continue to the menu...");
                Console.ReadKey();
            }
        }
    }
}
