/* Osäkert om detta ska användas
testa att implementera koden igenom classen direkt så mindre problem
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client;
using CoreofApplication;

namespace Client
{
    public interface ITransactionService
    {
        // Deposit money into the user's account
        Task<bool> DepositTransactions(Transaction transaction);

        // Delete a transaction by its ID
        Task<bool> DeleteTransactions(Transaction transaction);

        // Get the current balance of a user
        Task<decimal?> GetCurrentBalance(Transaction transaction);

        // Get income transactions for a client within a period type (e.g., "year", "month")
        Task<decimal> GetIncomePeriods(int ClientId, string periodType); // Updated to match PostgresTransactionService

        // Get all money spent transactions for a client
        Task<decimal> GetMoneySpentPeriods(Transaction transaction, string periodType, int? year = null, 
        int? month = null, int? week = null, DateTime? day = null);

        void Exit();
    }
}
*/