using System; 
using Npgsql;

namespace Client  {
public interface IClientService {
    Task<Clients?> RegisterNewUser(string username, string passwordhash, string email);
    Task<Clients?> UserLogin(string username, string passwordhash, string email);
    Clients? UserLogout();
    Clients? LogoutAndSwitchUser(string username, string passwordhash);
    }
}