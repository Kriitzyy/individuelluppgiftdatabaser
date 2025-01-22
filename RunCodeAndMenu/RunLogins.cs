using System;
using System.Threading.Tasks;
using CoreofApplication;
using Client;

// Denna fil hanterar integreringen med användaren 
// i main blir det mycket kod med integrering så det krävdes egen fil

namespace CoreofApplication {

    public class UserService {

        // Deklarerar privata fält för tjänsterna som används
        private readonly PostgresClientService _postgresClientService;
        private readonly UserLogoutService _userLogoutService;

        // Kontruktur som tar in tjänster för att hantera utloggning och databas anslutning
        public UserService(PostgresClientService postgresClientService, UserLogoutService userLogoutService) {

            // Kollar om värdet är null
            _postgresClientService = postgresClientService ?? throw new ArgumentNullException(nameof(postgresClientService)); 
            _userLogoutService = userLogoutService ?? throw new ArgumentNullException(nameof(userLogoutService)); 
        }

        // Metod för att registrera en ny användare
        public async Task CallingRegisterUser() {

            Console.Clear(); 

            Console.WriteLine("Enter Your username:"); 
            string NewUser = Console.ReadLine()!.ToLower(); 

            Console.Clear();

            Console.WriteLine("Enter Password:"); 
            string ThePassword = Console.ReadLine()!.ToLower(); 

            Console.Clear();

            Console.WriteLine("Enter Email:"); 
            string RegisterEmail = Console.ReadLine()!.ToLower(); 

            // Skapar ett objekt av UserRegistrationService 
            var userRegistrationObject = new UserRegistrationService();

            // och anropar RegisterNewUser för att registrera användaren
            var registeredUser = await userRegistrationObject.RegisterNewUser(NewUser, ThePassword, RegisterEmail);

            Console.Clear();

            // Om registreringen lyckas
            if (registeredUser != null) {
                
                Console.Clear();
                Console.WriteLine($"User {registeredUser.username} registered successfully with ID {registeredUser.Id}"); // Meddelande om lyckad registrering
                // Uppdaterar  logged-in användaren

                Program.LoggedInUser = registeredUser; 
            }
            else {

                // Om registreringen misslyckas
                Console.WriteLine("Registration failed.");
            }
        }

        // Metod för att logga in med existerande användare
        public async Task<Clients?> CallingLoginUser() { 

            Console.Clear();

            Console.WriteLine("Enter Your username or email:"); 
            string UserNameOrEmail = Console.ReadLine()!.ToLower(); 

            Console.Clear();

            Console.WriteLine("Enter Password:"); 
            string PasswordInput = Console.ReadLine()!.ToLower(); 

            var userLoginService = new UserLoginService(); 

            var registeredUser = await userLoginService.UserLogin(UserNameOrEmail, PasswordInput); 

            Console.Clear();

            // Om användaren är korrekt inloggad
            if (registeredUser != null) {

                Program.LoggedInUser = registeredUser; // Uppdaterar den användaren som är inloggad
                Console.WriteLine($"Welcome, {registeredUser.username}!"); 
                return registeredUser;
            }
            else {

                // Om inloggning misslyckas
                Console.WriteLine("Login failed. Try again.");
                return null;
            }
        }

        // Metod för att logga ut användaren
        public void CallingLogoutUser() { 

            // Anropar UserLogout metod för eventuell utloggning
            _userLogoutService.UserLogout(); 

            Program.LoggedInUser = null; 

        }

        // Metod för att byta användare och logga ut
        public async Task CallingUserChange() {

            Console.Clear();

            // Anropar LogoutAndSwitchUser metoden från UserLogoutService
            bool isSwitched = await _userLogoutService.LogoutAndSwitchUser(" ", " "); 

            Console.Clear();

            // Om användaren byttes 
            if (isSwitched) {

                // Hämta den nyligen inloggade användaren och uppdatera Program.LoggedInUser
                Program.LoggedInUser = _userLogoutService.GetLoggedInUser();

                if (Program.LoggedInUser != null) {

                    // Om bytet lyckades
                    Console.WriteLine($"Successfully switched to {Program.LoggedInUser.username}."); 
                }
                else {

                   Program.LoggedInUser = null; // Tömmer den globala användarstatusen
                   Console.WriteLine("No user is logged in after the switch."); // Meddelande om inget användare är inloggad efter byte
                }
            }
            else {

                // Om det inte gick att byta användare
                Console.WriteLine("Failed to switch user. Try again.");
            }
        }        
    }
}
