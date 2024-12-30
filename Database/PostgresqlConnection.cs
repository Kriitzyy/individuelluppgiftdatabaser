using Npgsql;
using System;

public class Connection {

        public static void Go() {

        var ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                conn.Open();
                Console.WriteLine("Connection to PostgreSQL is successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }
        }
    }
}

/* 
Skapa tabeller för klasserna du har, Client och Clientservice och om mer kommer,
sen kan dom implementeras här i vscode 
*/ 

    
