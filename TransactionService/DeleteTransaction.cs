using System;
using Npgsql;
using CoreofApplication;
using System.Threading.Tasks;

// Raderar användarens transactions, 4! 

namespace CoreofApplication
{
    public class DeleteTransaction
    {
        public static async Task UsersDeleteTransactionAsync(string ConnectionString)
        {
            Console.WriteLine("Enter the amount you want to delete:");
            string DeleteUsersAmount = Console.ReadLine()!.ToLower();

            Console.WriteLine("\nEnter the description you want to delete:");
            string DeleteUserDescription = Console.ReadLine()!.ToLower();

            if (decimal.TryParse(DeleteUsersAmount, out decimal AmountToDelete))
            {
                try
                {
                    using (var connection = new NpgsqlConnection(ConnectionString))
                    {
                        await connection.OpenAsync();

                        string deleteQuery = "DELETE FROM transactions WHERE amount = @amount AND description = @description";

                        using (var cmd = new NpgsqlCommand(deleteQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@amount", AmountToDelete);
                            cmd.Parameters.AddWithValue("@description", DeleteUserDescription);

                            int rowsAffected = await cmd.ExecuteNonQueryAsync();
                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("Transaction deleted successfully!");
                            }
                            else
                            {
                                Console.WriteLine("No transaction found with the specified amount and description.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid amount.");
            }
        }
    }
}
