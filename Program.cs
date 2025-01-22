using System;
using System.Threading.Tasks;
using CoreofApplication;
using Client;

namespace CoreofApplication {

    class Program {

        // Variabel som håller koll på den nuvarande inloggade användaren
        public static Clients? LoggedInUser = null;

        public static async Task Main(string[] args) {

            // Initierar tjänstern
            Connection.GetConnection(); // Skapar anslutning till databasen (För att säkerställa den funkar)
            var postgresClientService = new PostgresClientService(); // Skapar instans av tjänst för att hantera klienter
            var userLogoutService = new UserLogoutService(); // Skapar instans av tjänst för utloggning
            var userService = new UserService(postgresClientService, userLogoutService); // Skapar användartjänst med de tidigare tjänsterna

            int LoginChoice;
            bool LoginBool = false;

            // Loop för inloggningsmeny
            while (!LoginBool) {

                // Visar alternativ för inloggning
                DisplayMenu.DisplayLoginOptions();

                string LogInInput = Console.ReadLine()!.ToLower();

                // Omvandlar användarens inmatning till ett heltal
                if (int.TryParse(LogInInput, out LoginChoice)) {

                    if (LoginChoice == 1) {

                        // Om användaren väljer alternativ 1, kör registrering
                        await userService.CallingRegisterUser();
                    }
                    else if (LoginChoice == 2)  {

                        // Om användaren väljer alternativ 2, kör inloggning
                        var registeredUser = await userService.CallingLoginUser();

                        if (Program.LoggedInUser != null)  {

                            // Om inloggning lyckades, sätt inloggningsstatus och avsluta inloggningsloopen
                            LoginBool = true;
                        }
                        else {

                            // Om inloggningen misslyckas 
                            Console.WriteLine("Login failed, try again...");
                        }
                    }

                    else if (LoginChoice == 3) {

                        // Om användaren vill byta användare
                        await userService.CallingUserChange();

                        if (Program.LoggedInUser != null) {

                            // Om bytet lyckades, visa det nya användarnamnet
                            Console.WriteLine($"FChanged to: {Program.LoggedInUser.username}.");
                        }
                        else {

                            // Om inget användarnamn är inloggat efter bytet
                            Console.WriteLine("Inget användarnamn är inloggat efter bytet.");
                        }
                    }

                    else if (LoginChoice == 4) {

                        // Om användaren vill avsluta programmet
                        Console.WriteLine("Ending SecureSweBank!");
                        Environment.Exit(0); // Stänger av programmet med en framgångskod
                    }
                }
                else {

                    // Om användaren inte har angett ett giltigt alternativ
                    Console.WriteLine("Choose between 1-4.");
                }
            }

            // Loop för huvudmenyn
            bool stillRunning = true;

            while (stillRunning) {

                Console.Clear();

                // Visar transaktionsmenyn
                DisplayMenu.DisplayTransactionMenu();

                if (Program.LoggedInUser == null) {

                    // Om ingen användare är inloggad, visa ett meddelande och avsluta loopen
                    Console.WriteLine("Login to go back to the main menu...");
                    break; 
                }
                
                string GetUserInput = Console.ReadLine()!.ToLower();
                int usersMenuOptions;

                // omvandlar användarens inmatning till ett heltal
                if (int.TryParse(GetUserInput, out usersMenuOptions)) {

                    Console.Clear();

                    // Hantera användarens val i huvudmenyn
                    switch (usersMenuOptions) {

                        case 1:

                            // Om användaren väljer alternativ 1
                            await RunTransactions.CallingDeposit();
                            break;

                        case 2:
                            // Om användaren väljer alternativ 2, 
                            await RunTransactions.CallingDelete();
                            break;

                        case 3:
                            // Om användaren väljer alternativ 3,
                            await RunTransactions.CallingCurrentBalance();
                            break;

                        case 4:
                            // Om användaren väljer alternativ 4, 
                            await RunTransactions.CallingMoneySpent();
                            break;

                        case 5:
                            // Om användaren väljer alternativ 5, 
                            await RunTransactions.CallingIncomePeriods();
                            break;

                        case 6:
                            // Om användaren väljer alternativ 6, 
                            DisplayMenu.DisplayUserNeedHelp();
                            break;

                        case 7:
                            // Om användaren väljer alternativ 7, avsluta programmet
                            Console.Clear();
                            Console.WriteLine("Ending SecureSwe Bank...");
                            stillRunning = false; // Sätt loopstatus till false för att avsluta programmet/loopen
                            return;

                        default:

                            // Om användaren väljer ett ogiltigt alternativ, be om ett val mellan 1-7
                            Console.WriteLine("Ensure choosing between 1-7!");
                            break;
                    }
                }
                else {

                    // Om användaren inte anger ett giltigt alternativ
                    Console.Clear();
                    Console.WriteLine("Ensure choosing between 1-7");
                }

                // Vänta på användarinmatning för att fortsätta
                Console.WriteLine("\nPress any key to continue....");
                Console.ReadKey();
            }
        }
    }
}
