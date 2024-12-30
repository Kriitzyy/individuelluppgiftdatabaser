using System; 
using System.Data.Common;
using System.Text.RegularExpressions;
using Npgsql;
using BCrypt;

namespace Client {

    public class Clients {
    public int Id { get; set; }
    public string username { get; set;}
    public string passwordhash { get; set;}
    public string email { get; set;}
    
    }
}
