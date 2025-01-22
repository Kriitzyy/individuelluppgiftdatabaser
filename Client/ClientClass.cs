using System; 
using System.Data.Common;
using System.Text.RegularExpressions;

// Användar Class används för Login operationerna 
// Med username, password och email.

namespace Client {
    public class Clients {
    public int Id { get; init; }
    public string username { get; set; }
    public string passwordhash { get; set; }
    public string email { get; set; }

    
    }
}
