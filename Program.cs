using System;
using System.Collections.Generic;
using CoreofApplication;
using Npgsql;
using BCrypt;
using Client; // Use the namespace for access to Transaction class


namespace CoreofApplication // Namespace puts together codes.
{
    class Program {
        // Allting som ska användas ska anropas här, dela upp det och använd metoder så det blir mindre val
        public static List<UserTransaction> TransactionList = new List<UserTransaction>(); 
            public static async Task Main(string[] args){
            
            var postgresClientService = new PostgresClientService();


            // var newClient = await postgresClientService.RegisterNewUser(username, password, email);

         
            int LoginChoice;
            bool LoginBool = false; 
            Connection.Go();

            while (!LoginBool) {
                DisplayMenu.DisplayWelcomeTheme(); 

                DisplayMenu.DisplayLoginOptions(); // Users login options

                String LogInInput = Console.ReadLine()!.ToLower();

                if (int.TryParse(LogInInput, out LoginChoice)) {

                    if (LoginChoice == 1) {

                        Console.WriteLine("Enter your username for a new user:");
                        string UserName = Console.ReadLine()!.ToLower();

                        Console.WriteLine("Enter your password:");
                        string Password = Console.ReadLine()!.ToLower();

                        Console.WriteLine("Enter your email:");
                        string Email = Console.ReadLine()!.ToLower();

                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

                        var newClient = await postgresClientService.RegisterNewUser(UserName, Password, Email);

                        if (newClient != null) {

                            Console.WriteLine("User registered successfully!");
                            Console.WriteLine($"ID: {newClient.Id}");
                            Console.WriteLine($"Password: {newClient.passwordhash}"); // Koden är hashad vilket gör att den it syns bara massa symboler
                            Console.WriteLine($"Username: {newClient.username}");
                            Console.WriteLine($"Email: {newClient.email}");
                        }
                        else {

                            Console.WriteLine("Registration failed, please try again");
                        }
                    }
                    else if (LoginChoice == 2) {

                        // Metod för att logga in med existernade user 
                    }
                    else if (LoginChoice == 3) {

                        // Metod för logga ut och logga in med exiterande user
                    }
                    else if (LoginChoice == 4) {

                        // Metod för att göra en exit
                    }

                }
            } 
            
            bool stillrunning = true; 
            int usersmenuoptions;

            while (stillrunning){ 


                DisplayMenu.DisplayMainMenu(); 
                
                string GetUserInput = Console.ReadLine()!.ToLower(); // Reading user input and converts to lowercased
 
                if (int.TryParse(GetUserInput, out usersmenuoptions)){ 

                    switch (usersmenuoptions){ 
                        case 1:
                        Console.WriteLine(""); // User logs in a account
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
                            break;// ending the case 

                        case 6:
                            UserTransactionMethods.UsersMoneyIncome(TransactionList); 
                            break;

                        case 7:
                            DisplayMenu.UserNeedHelp();
                            break;

                        case 8:
                            Console.WriteLine("Exiting SecureSwe Bank..."); 
                            stillrunning = false; 
                            break; 

                        default: 
                            Console.WriteLine("ensure entering a choice between 1-7!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please, ensure you are choosing between 1-7!"); 
                }

                Console.WriteLine("\nPress any key to continue to the menu!..."); 

                Console.ReadKey(); 
            }   
        }
    }
}