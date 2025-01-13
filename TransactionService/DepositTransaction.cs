using System;
using Npgsql;
using CoreofApplication;
using System.Threading.Tasks;

// Sparar transaktionerna till en databas, 3! 

namespace CoreofApplication
{
    public class DepositTransaction
    {
        // Asynchronous method to deposit money into the user's account
        public static async Task UsersMoneyDeposit(string ConnectionString, int ClientId)
        {
            Console.WriteLine("Write the amount of money you want to deposit:");
            string userInput = Console.ReadLine()!.ToLower();

            if (decimal.TryParse(userInput, out decimal Amount))
            {
                Console.WriteLine("Where does the money come from? (Salary, Loan, Sale, etc.)");
                string Source = Console.ReadLine()!;

                try
                {
                    using (var connection = new NpgsqlConnection(ConnectionString))
                    {
                        await connection.OpenAsync();

                        string sqlQuery = "INSERT INTO transactions (client_id, transaction_date, amount, description) " +
                                          "VALUES (@clientId, @transactionDate, @amount, @description)";

                        using (var cmd = new NpgsqlCommand(sqlQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@clientId", ClientId);
                            cmd.Parameters.AddWithValue("@transactionDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@amount", Amount);
                            cmd.Parameters.AddWithValue("@description", Source);

                            await cmd.ExecuteNonQueryAsync(); // Insert the transaction
                            Console.WriteLine("\nThe deposit is successful. Let's continue!");
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
                Console.WriteLine("The amount is invalid. Enter a valid amount!");
            }
        }
    }
}
