using System;
using System.IO;  // To read the SQL file
using Npgsql;

public class Connection
{
    // Privat statisk anslutningssträng som används internt
    private static string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";

    // Publik metod som returnerar en ny databasanslutning
    public static NpgsqlConnection GetConnection()
    {
        try
        {
            return new NpgsqlConnection(ConnectionString);
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating database connection: " + ex.Message);
        }
    }

    // Exempel på metod för att testa anslutningen
    public static void TestConnection()
    {
        using (var conn = GetConnection())
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