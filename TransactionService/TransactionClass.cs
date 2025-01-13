using System;
using System.Collections.Generic;
using CoreofApplication; 
// Transaction Klassen 

    public class UserTransaction
    {
        // Properties of a transaction 
        public static List<UserTransaction> DeletedMoney = new List<UserTransaction>();
        public int TransactionId { get; set; }
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
        public int DeletedAmount { get; set; }
        public string Source  { get; set; }
        public DateTime TransactionDate { get; set; }

        public UserTransaction(int ClientId, decimal amount, string source)
        {
            this.ClientId = ClientId;
            this.Amount = amount;
            this.Source  = source;
            this.TransactionDate = DateTime.Now;
            
        }
    
    }

