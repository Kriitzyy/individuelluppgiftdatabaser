using System;
using System.Collections.Generic;

namespace CoreofApplication
{
    public class UserTransaction
    {
        // Properties of a transaction 
        public static List<UserTransaction> DeletedMoney = new List<UserTransaction>();
        public int TransactionId { get; set; }
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
        public string Source  { get; set; }
        public string TransactionType { get; set; }
       public DateTime TransactionDate { get; set; }

        // Constructors
        public UserTransaction(decimal amount, string source)
        {
            ClientId = ClientId;
            Amount = amount;
            Source  = source;
            TransactionType = TransactionType;
            TransactionDate = DateTime.Now;
            
        }

    
    }
}