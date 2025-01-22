using System;
using System.Threading.Tasks;
using CoreofApplication;
using Client;

// Login Funkar bra!! 
// Fixa bara så inte koden dupliceras 
// Och ändra Namner på objekt, kod efter vad de gör
// för tydlighet

namespace CoreofApplication
{
    public class UserService
    {
        private readonly PostgresClientService _postgresClientService;
        private readonly UserLogoutService _userLogoutService;

        public UserService(PostgresClientService postgresClientService, UserLogoutService userLogoutService)
        {
            _postgresClientService = postgresClientService ?? throw new ArgumentNullException(nameof(postgresClientService));
            _userLogoutService = userLogoutService ?? throw new ArgumentNullException(nameof(userLogoutService));
        }

        // Method to register a new user
        public async Task CallingRegisterUser()
        {
            Console.WriteLine("Enter Your username:");
            string NewUser = Console.ReadLine(); // Read the username input

            Console.WriteLine("Enter Password:");
            string ThePassword = Console.ReadLine(); // Read the password input

            Console.WriteLine("Enter Email:");
            string RegisterEmail = Console.ReadLine(); // Read the email input

            // Call RegisterNewUser to register the user
            var userRegistrationObject = new UserRegistrationService();

            var registeredUser = await userRegistrationObject.RegisterNewUser(NewUser, ThePassword, RegisterEmail);

            if (registeredUser != null)
            {
                Console.WriteLine($"User {registeredUser.username} registered successfully with ID {registeredUser.Id}");

                Program.LoggedInUser = registeredUser; // Update the global logged-in user state

            }
            else
            {
                Console.WriteLine("Registration failed.");
            }
        }

        // Method to log in an existing user
        public async Task<Clients?> CallingLoginUser()
        {
            Console.WriteLine("Enter Your username or email:");
            string UserNameOrEmail = Console.ReadLine(); // Read the username or email input

            Console.WriteLine("Enter Password:");
            string PasswordInput = Console.ReadLine(); // Read the password input

            var userLoginService = new UserLoginService();

            var registeredUser = await userLoginService.UserLogin(UserNameOrEmail, PasswordInput);

            if (registeredUser != null)
            {
                Program.LoggedInUser = registeredUser; // Set the logged-in user in the Program class
                Console.WriteLine($"Welcome, {registeredUser.username}!");
                return registeredUser;
            }
            else
            {
                Console.WriteLine("Login failed. Try again.");
                return null;
            }
        }

        // Method to handle user logout
        public void CallingLogoutUser()
        {
            _userLogoutService.UserLogout(); // Use UserLogoutService for logout

            Program.LoggedInUser = null; // Clear the logged-in user state in Program class

            Console.WriteLine("You have successfully logged out.");
        }

        // Method to switch to another user
        public async Task CallingUserChange()
        {
             // Call the LogoutAndSwitchUser method from UserLogoutService
            bool isSwitched = await _userLogoutService.LogoutAndSwitchUser(" ", " ");

            if (isSwitched)
            {
                // After switching, retrieve the newly logged-in user and update the Program.LoggedInUser
                Program.LoggedInUser = _userLogoutService.GetLoggedInUser(); // Ensure you're using the global state

                if (Program.LoggedInUser != null)
                {
                    
                    Console.WriteLine($"Successfully switched to {Program.LoggedInUser.username}.");
                }
                else
                {
                   Program.LoggedInUser = null; // Clear the state explicitly
                   Console.WriteLine("No user is logged in after the switch.");
                }
            }
            else
            {
                Console.WriteLine("Failed to switch user. Try again.");
            }
        }
        
        //  Nödvändig?? 
        public Clients? GetLoggedInUser()
        {
            return Program.LoggedInUser; // Ensure it references the global user state in Program
        }
    }
}
