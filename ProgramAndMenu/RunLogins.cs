using System;
using System.Threading.Tasks;
using BCrypt.Net;
using CoreofApplication;
using Client;

namespace CoreofApplication
{
    public class UserService
    {
        private readonly PostgresClientService _postgresClientService;
        private readonly UserLogoutService _userLogoutService;

        public UserService(PostgresClientService postgresClientService, UserLogoutService userLogoutService)
        {
            _postgresClientService = postgresClientService;
            _userLogoutService = userLogoutService;
        }

        // Method to register a new user
        public async Task RegisterUser()
        {
            
            
        }

        // Method to log in an existing user
        public async Task LoginUser()
        {
        }

        // Method to handle user logout
        public void LogoutUser()
        {
        
        }

        // Method to switch to another user
        public async Task SwitchUser()
        {
            
        }
    }
}
