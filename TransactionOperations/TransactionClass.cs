using System;
using CoreofApplication;

namespace Client
{
    public class Transaction
    {
        // TransactionId behövs för att identifiera
        /*
        Ja, även om du har en relation mellan klienter och transaktioner via ClientId, 
        så behövs TransactionId som en unik identifierare för varje transaktion
        */
        public int TransactionId { get; init; } 
        public int ClientId { get; set; } // Linking transaction to a client
        public decimal amount { get; set; }
        public string source { get; set; } // Could represent the source (deposit, payment, etc.)
        public DateTime TransactionDate { get; set; } = DateTime.Now; // Default to current time
    }
}

