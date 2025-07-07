using System;
using System.IO;  
using Npgsql;

// En fil med connection stringen i en metod 
// Eftersom att jag slipper då skriva Connection string hela tiden
// Anropar jag metoden istället

// Connection Class
public class Connection {

    // Privat variabel som lagrar anslutningssträngen
    private static string ConnectionString = "Host=localhost;Username=postgres;Password=123;Database=bankapp";

    // Publik metod som returnerar en ny databasanslutning
    public static NpgsqlConnection GetConnection() { 

        try {

            // Skapar och returnerar ett nytt NpgsqlConnection-objekt med hjälp av anslutningssträngen
            return new NpgsqlConnection(ConnectionString); 
        }
        catch (Exception ex) {
            
            // Hanterar fel om det skulle uppstå vid anslutning
            throw new Exception("Error creating database connection: " + ex.Message);
        }
    }

}