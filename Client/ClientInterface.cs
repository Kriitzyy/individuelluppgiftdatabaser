using System; 
using Npgsql;
// Client Interface med alla alla Login operationer
namespace Client  {


public interface IClientService {
    Task<Clients?> RegisterNewUser(string username, string passwordhash, string email);
    Task<Clients?> UserLogin(string usernameOrEmail, string password);
    Clients? UserLogout();
    Task<Clients?> LogoutAndSwitchUser(string usernameOrEmail, string password);
    }
}