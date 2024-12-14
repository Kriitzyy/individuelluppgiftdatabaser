using System;
using System.Collections.Generic;

namespace SecureSweBank
{
    public class UserTransaction
    {
        // Properties of a transaction 
        public decimal Amount { get; set; }
        public string Source { get; set; }
        public DateTime Date { get; set; }
        public static List<UserTransaction> DeletedMoney = new List<UserTransaction>();

        // Constructor
        public UserTransaction(decimal amount, string source)
        {
            Amount = amount;
            Source = source;
            Date = DateTime.Now;
        }
    }
}