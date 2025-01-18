/* Osäkert om detta ska användas
testa att implementera koden igenom classen direkt så mindre problem
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Client;
using CoreofApplication;

namespace Client
{
    public class PostgresTransactionService : ITransactionService
    {
        private readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Mo20042004;Database=bankapp";
        private readonly GetTransaction _getTransaction;

        public PostgresTransactionService()
        {
            _getTransaction = new GetTransaction(); // Initialize GetTransaction to access balance logic
        }

        // Deposit money into the user's account (Insert into DB)
        public async Task<bool> DepositTransactions(Transaction transaction)
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync(); // Open connection asynchronously

                    string sqlQuery = "INSERT INTO transactions (client_id, transaction_date, amount, description) " +
                                      "VALUES (@ClientId, @transactionDate, @amount, @description)";

                    using (var cmd = new NpgsqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@client_id", transaction.ClientId);
                        cmd.Parameters.AddWithValue("@transaction_date", DateTime.Now); // Use current date for the transaction
                        cmd.Parameters.AddWithValue("@amount", transaction.amount);     // Amount to deposit
                        cmd.Parameters.AddWithValue("@description", transaction.source); // Transaction description

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        // Delete a transaction by its ID (Delete from DB)
        public async Task<bool> DeleteTransactions(Transaction transaction)
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync(); // Open connection asynchronously

                    string deleteQuery = "DELETE FROM transactions WHERE transaction_id = @transactionId";

                    using (var cmd = new NpgsqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@transactionId", transaction.TransactionId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        // Get current balance of a user using GetTransaction class
        public async Task<decimal?> GetCurrentBalance(Transaction transaction)
        {
            try
            {
                // Use the GetTransaction class to retrieve the current balance
                decimal currentBalance = await _getTransaction.GetCurrentBalance(transaction);
                return currentBalance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving balance: {ex.Message}");
                return null;
            }
        }

        // Get income transactions for a client between two dates
        public async Task<decimal> GetIncomePeriods(int ClientId, string periodType)
        {
            try
            {
                // Get the start and end dates based on the provided period type
                var (startDate, endDate) = IncomeTransaction.GetPeriods(periodType);

                // Use the IncomeTransaction class to get the total income for the given period
                decimal totalIncome = await IncomeTransaction.GetIncomePeriods(ClientId, startDate, endDate);
                return totalIncome;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving income periods: {ex.Message}");
                return 0; // Return 0 if there's an error
            }
        }

        // Get all money spent transactions for a client
       public async Task<decimal> GetMoneySpentPeriods(Transaction transaction, string periodType, int? year = null, 
        int? month = null, int? week = null, DateTime? day = null)
        {
        try
        {
        // Use the existing MoneySpentCalculator to calculate the total money spent
        decimal totalSpent = await MoneySpentCalculator.MoneySpentPeriods(
        transaction, periodType, year, month, week, day);

        return totalSpent;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred while getting money spent periods: {ex.Message}");
        return 0;
    }
}


        public void Exit()
        {
            // Exit method logic here (if needed)
        }
    }
}
*/ 