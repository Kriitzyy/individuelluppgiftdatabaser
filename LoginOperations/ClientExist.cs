/* OSÄKERT OM MAN SKA HA 


using System; 
using Npgsql;
using CoreofApplication;
using Client;

public class TheClient{
    public static async Task<bool> ClientExists(int clientId)
{
    try
    {
        using (var connection = Connection.GetConnection()) // Assuming you have a connection class
        {
            await connection.OpenAsync();
            string sqlQuery = "SELECT COUNT(1) FROM clients WHERE id = @client_id";
            using (var cmd = new NpgsqlCommand(sqlQuery, connection))
            {
                cmd.Parameters.AddWithValue("@client_id", clientId);
                var result = await cmd.ExecuteScalarAsync();
                return (long)result > 0;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while checking client: {ex.Message}");
        return false;
    }
}

}
*/