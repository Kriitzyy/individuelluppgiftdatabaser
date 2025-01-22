using System;
using CoreofApplication;

// Class som hanterar transaktionerna 
namespace Client {

    public class Transaction {

        public int TransactionId { get; init; } 
        public int ClientId { get; set; } 
        public decimal amount { get; set; }
        public string source { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now; 
    }
}

