using System;
using System.Collections.Generic;
using CoreofApplication;
using Npgsql;
using BCrypt;
using dotenv.net;
using Client;

namespace CoreofApplication
{
    class Program
    {
        // Allting som ska användas ska anropas här, dela upp det och använd metoder så det blir mindre val
        public static List<UserTransaction> TransactionList = new List<UserTransaction>();

        public static async Task Main(string[] args)
        {
            Connection.TestConnection();
            
            var postgresClientService = new PostgresClientService();
            var userLogoutService = new UserLogoutService(); // Create instance for handling user logout and login change

            int LoginChoice;
            bool LoginBool = false;


            while (!LoginBool)
            {
                DisplayMenu.DisplayLoginOptions(); // Users login options

                string LogInInput = Console.ReadLine()!.ToLower();

                if (int.TryParse(LogInInput, out LoginChoice))
                {
                    if (LoginChoice == 1)
                    {
                        Console.Clear();
                        Console.WriteLine("Enter your username for a new user:");
                        string username = Console.ReadLine()!.ToLower();

                        Console.WriteLine("Enter your password:");
                        string passwordhash = Console.ReadLine()!.ToLower();

                        Console.WriteLine("Enter your email:");
                        string email = Console.ReadLine()!.ToLower();

                        string HashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordhash);

                        var newClient = await postgresClientService.RegisterNewUser(username, passwordhash, email);

                        if (newClient != null)
                        {
                            Console.Clear();
                            Console.WriteLine("\nUser registered successfully!");
                            Console.WriteLine($"ID: {newClient.Id}");
                            Console.WriteLine($"Username: {newClient.username}");
                            Console.WriteLine($"Email: {newClient.email}");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Registration failed, please try again");
                        }
                    }
                    else if (LoginChoice == 2)
                    {
                        Console.Clear();
                        Console.WriteLine("Enter your username or email to login: ");
                        string UserNameOrEmail = Console.ReadLine()!.ToLower();

                        Console.WriteLine("Enter your password: ");
                        string LoginPassword = Console.ReadLine()!.ToLower();

                        var LoggedInUser = await postgresClientService.UserLogin(UserNameOrEmail, LoginPassword, UserNameOrEmail);

                        if (LoggedInUser != null)
                        {
                            Console.Clear();
                            Console.WriteLine("Login successful!");
                            Console.WriteLine($"Welcome, {LoggedInUser.username}!");
                            LoginBool = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Login failed.. try again!");
                        }
                    }
                    else if (LoginChoice == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Logging out current user...");
                        userLogoutService.UserLogout(); // Log out the current user

                        Console.WriteLine("\nPlease enter credentials for the new user:");
                        Console.WriteLine("Enter your username or email:");
                        string usernameOrEmail = Console.ReadLine()!.ToLower();

                        Console.WriteLine("Enter your password:");
                        string password = Console.ReadLine()!.ToLower();

                        bool loginSuccess = await userLogoutService.UserChangeLogin(usernameOrEmail, password, usernameOrEmail);

                        if (loginSuccess)
                        {
                            Console.Clear();
                            Console.WriteLine("Successfully logged in with new credentials.");
                            LoginBool = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Failed to log in with the new credentials.");
                        }
                    }
                    else if (LoginChoice == 4)
                    {
                        Console.Clear();
                        Console.WriteLine("Logging out from the current user.");
                        postgresClientService.UserLogout(); // Log out the current user

                        Console.WriteLine("\nWould you like to log in as another user? Yes or No?");
                        string UserRespons = Console.ReadLine()!.ToLower();

                        if (UserRespons == "yes")
                        {
                            Console.Clear();
                            Console.WriteLine("Enter the username or email for the new user:");
                            string usernameOrEmail = Console.ReadLine()!.ToLower();

                            Console.WriteLine("Enter the password:");
                            string password = Console.ReadLine()!.ToLower();

                            Clients? newClient = await postgresClientService.LogoutAndSwitchUser(usernameOrEmail, password, usernameOrEmail);

                            if (newClient != null)
                            {
                                Console.Clear();
                                Console.WriteLine($"Successfully switched to {newClient.username}.");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Failed to login with the new credentials.. returning to login menu..");
                                LoginBool = false;
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("You have chosen to exit. Exiting SecureSwebank..");
                            LoginBool = false; // Exit the main menu
                        }
                    }
                }
            }

            bool stillrunning = true;
            int usersmenuoptions;

            while (stillrunning)
            {
                Console.Clear();
                DisplayMenu.DisplayMainMenu();

                string GetUserInput = Console.ReadLine()!.ToLower();

                if (int.TryParse(GetUserInput, out usersmenuoptions))
                {
                    switch (usersmenuoptions)
                    {
                        case 1:
                            break;

                        case 2:
                            UserTransactionMethods.UsersMoneyDeposit(TransactionList);
                            break;

                        case 3:
                            UserTransactionMethods.UsersDeleteTransaction(TransactionList);
                            break;

                        case 4:
                            UserTransactionMethods.UsersCurrentBalance(TransactionList);
                            break;

                        case 5:
                            UserTransactionMethods.UsersMoneySpent(TransactionList);
                            break;

                        case 6:
                            UserTransactionMethods.UsersMoneyIncome(TransactionList);
                            break;

                        case 7:
                            DisplayMenu.UserNeedHelp();
                            break;

                        case 8:
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
