using System; 
using Npgsql;

namespace Client  {
public interface IClientService {
    Task<Clients?> RegisterNewUser(string UserName, string password, string Email);
    Clients? UserLogin(string username, string password);
    void UserLogout();
    Clients? GetExistingUser();
    }
}