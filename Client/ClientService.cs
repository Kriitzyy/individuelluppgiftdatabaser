using System;
using System.Threading.Tasks;
using Client;
// Ändrad
public interface IClientService 
{
    Task<Clients?> RegisterNewUser(string username, string passwordhash, string email);
    Task<Clients?> UserLogin(string usernameOrEmail, string passwordhash);
    Clients? UserLogout();  // This method should handle logging out the user
    Task<Clients?> SwitchUser(string usernameOrEmail, string passwordhash);  // For switching between users
}
