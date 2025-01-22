using System;
using System.Threading.Tasks;
using Npgsql;
using CoreofApplication;
using Client;

// En fil som hanterar logga ut och byta användare 

namespace Client {

    // Klass som hanterar att logga ut och byta användare
    public class UserLogoutService {

        // Referens till PostgresClientService för eventuella databas anrop
        private readonly PostgresClientService _postgresClientService = new PostgresClientService();

        // Metod för att logga ut en användare
        public void UserLogout() {

            // Kollar om användaren är inloggad 
            if (Program.LoggedInUser != null) {

                // Om en användare är inloggad, logga ut genom att sätta till null
                Program.LoggedInUser = null; 
                
                Console.WriteLine("You have been logged out successfully.");
            }
            else {

                // Om ingen användare är inloggad, visa ett meddelande
                Console.WriteLine("No user is currently logged in.");
            }
        }

        // Metod för att logga ut och direkt därpå byta till en annan användare eller befintlig användare
        public async Task<bool> LogoutAndSwitchUser(string usernameOrEmail, string password) {

            // Val för att registrera eller logga in
            int Choice; 

            // Kontrollera om det finns en inloggad användare
            if (Program.LoggedInUser != null) { 

                // Om inloggad, loggas användaren ut
                UserLogout(); 

                Console.ReadKey();
            }
            else {

                // Om användaren inte är inloggad visas ett felmeddelande och avslutas
                Console.WriteLine("No user is logged in.. try again");
                return false; 
            }

            // Visa meddelande om att användaren loggas ut
            Console.WriteLine("Press any key to continue..."); 
            Console.ReadKey();

            // Ge användaren möjligheten att registrera sig eller logga in
            Console.WriteLine("[1] - Register user");
            Console.WriteLine("[2] - Login");

            string UserInput = Console.ReadLine()!.ToLower();

            // Kontrollera om inmatningen är ett giltigt nummer
            if (int.TryParse(UserInput, out Choice)) {

                // Om användaren vill registrera sig.
                if (Choice == 1) { 

                    Console.WriteLine("Enter a username:");
                    string username = Console.ReadLine()!.ToLower();

                    Console.WriteLine("Enter a password:");
                    string passwordhash = Console.ReadLine()!.ToLower();

                    Console.WriteLine("Enter an email:");
                    string email = Console.ReadLine()!.ToLower();
                    
                    var userRegistrationService = new UserRegistrationService();

                    // Anropar metoden för att registrera en ny användare
                    var registeredUser = await userRegistrationService.RegisterNewUser(username, passwordhash, email);

                    if (registeredUser != null) {
                        
                        // Om registreringen lyckas logga in användaren
                        Program.LoggedInUser = registeredUser; 
                        return true;
                    }
                    else {

                        // Om registreringen misslyckas, visas felmeddelande
                        Console.WriteLine("Registration failed.. try again");
                        return false; 
                    }
                }

                // Om användaren vill logga in
                else if (Choice == 2) {

                    // Log in an existing user
                    Console.WriteLine("Enter username or email:");

                    string loginUsernameOrEmail = Console.ReadLine()!.ToLower();

                    Console.WriteLine("Enter password:");
                    string loginPassword =  Console.ReadLine()!.ToLower();

                    var userLoginService = new UserLoginService();

                    // Anropar metoden för att logga in
                    var loggedInUser = await userLoginService.UserLogin(loginUsernameOrEmail, loginPassword);

                    if (loggedInUser != null) {

                        // Om inloggningen lyckas, uppdatera den inloggade användaren
                        Program.LoggedInUser = loggedInUser;  
                        return true;  
                    }
                    else {
                        
                        // Om inloggning failar skickas användaren tillbaka med ett felmeddelande
                        Console.WriteLine("Login failed. Please check your credentials.");
                        return false;  
                    }
                }
                else {

                    // Om användaren skriver fel nummer 
                    Console.WriteLine("Ensure typing the right number!");
                }
            }
            else {

                // Om användaren angav något annat än ett nummer
                Console.WriteLine("Choose between 1-2!");
            }

            return false;  // Om operationen misslyckas
        }

        // Metod för att hämta den aktuella inloggade användaren från Program-klassen
        public Clients? GetLoggedInUser() {
            // Returnera den inloggade användaren
            return Program.LoggedInUser; 
        }
    }
}
