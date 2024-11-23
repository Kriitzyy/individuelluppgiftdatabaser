using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using Microsoft.VisualBasic;

namespace SecureSweBank { // Namespace puts together codes.

    public class UserTransaction { 
        // Properties of a transaction 
        public decimal Amount { get; set; }
        public string Source { get; set; } 
        public DateTime Date { get; set; }
    
        public UserTransaction(decimal amount, string source) {
            Amount = amount;
            Source = source; 
            Date = DateTime.Now; 
        }
      
        public static void UsersMoneyDeposit(List<UserTransaction> TransactionList) { // NEtod to Deposit money. Works FINE

            Console.WriteLine("Write the amount of money you want to deposit:");
            string UserInput = Console.ReadLine()!.ToLower();

            if (decimal.TryParse(UserInput, out decimal amount)) {
                Console.WriteLine("Where does the money come from? (Salary, Loan, Sale, etc.)");
                string Source = Console.ReadLine()!;

                // Adding the new transaction to the list as a object
                TransactionList.Add(new UserTransaction(amount, Source));

                Console.WriteLine("\nThe deposit is successful. Let's continue!");
            }
            else {
                // if the user input was not a valid number 
                Console.WriteLine("The amount is invalid. Enter a valid amount!");
            }
        }

        // Method to delete a transaction. 
        // Works good it you put 500, and delete 500, 
        // you can only delete the money you put in
        // Change to, you can delete how much you want leftovers are left. 
        // So the money spent yearly, etc works
        public static void UsersDeleteTransaction(List<UserTransaction> TransactionList) {
 
            if (TransactionList.Count == 0) { // If the list if empty the user can't delete money
                Console.WriteLine("There's no transactions to delete..");
                return; 
            }
        
            Console.WriteLine("Here are the current transactions:");
        
            foreach (var UserTransaction in TransactionList) { 
                // Showing the current transactions thru a foreach loop
                Console.WriteLine($"\nAmount: {UserTransaction.Amount}, Source: {UserTransaction.Source}, Date: {UserTransaction.Date}");
            }

            Console.WriteLine("Enter the amount you want to delete:");
            string DeleteUsersAmount = Console.ReadLine()!.ToLower();

            Console.WriteLine("\nEnter the source you want to delete:");
            string DeleteUserSource = Console.ReadLine()!.ToLower();

            if (decimal.TryParse(DeleteUsersAmount, out decimal amountToDelete))
            { 
                bool DeleteButton = false;

                for (int i = 0; i < TransactionList.Count; i++)
                {
                    if (TransactionList[i].Amount == amountToDelete  && // Checking if the amount is stored as index
                    TransactionList[i].Source.Equals(DeleteUserSource, StringComparison.OrdinalIgnoreCase)){ 

                    TransactionList.RemoveAt(i); // Removing the users input with index

                    Console.WriteLine("Transaction deleted successfully!");
                    DeleteButton = true; 
                    return; 
                    }
                    else {
                        Console.WriteLine("Please enter the right amount!");
                    }
                }
                
                if (!DeleteButton) { // If no transactions was found with that amount and source
                    Console.WriteLine("No transactions were found with the specified amount and source.");
                }
            }
            else
            {   
                Console.WriteLine("Write the correct amount you want to delete!");
            }
            
        }
      
      // Does it need to be changed? 
      // Because the user can see what he put in
      // Put if he deletes money will the leftovers be shown?
        public static void UsersCurrentBalance(List<UserTransaction> TransactionList){  // Fucntion to view current balance

            if (TransactionList.Count == 0){ // If the user doesnt have money

                Console.WriteLine("There are no transactions...");
                return;
            }
          
            foreach (var UsersTransactions in TransactionList){ // Foreach loop to show the current amount with source and date

                Console.WriteLine($"Amount: {UsersTransactions.Amount}, Source: {UsersTransactions.Source}, Date: {UsersTransactions.Date}");
            }
            
        }

        // Not working for some reason, 
        // When the delete is fixed, 
        // Fix so the user can see how much he deleted
        public static void UsersMoneySpent(List<UserTransaction> TransactionList){ // Fucntion for the money spent 

            bool UserIsRunning = true; // Boolean for the while loop

            while (UserIsRunning){

            int UsersMenuOption; 

            DisplayMenu.DisplayMoneySpentMenu(); // Menu for money spent

            string UserChoice = Console.ReadLine()!.ToLower(); 
            
            if (int.TryParse(UserChoice, out UsersMenuOption)) { 

                DateTime TheDate = DateTime.Now; // The date

                decimal UsersTotalSpent =  0; // Users total spent

                foreach (var UserTransaction in TransactionList) {
                        
                    if (UserTransaction.Amount < 0) { 
                        switch (UsersMenuOption) {
                            case 1:  
                            if (UserTransaction.Date.Year == TheDate.Year) { // This case checks if the transaction occurred in the current month of the current year
                                //UsersTotalSpent += UserTransaction.Amount; 
                                UsersTotalSpent += Math.Abs(UserTransaction.Amount); // Add to total spent (using Math.Abs for negative values)

                            }
                            break;   

                            case 2:
                            if (UserTransaction.Date.Year == TheDate.Year && UserTransaction.Date.Month == TheDate.Month) {
                            // This case checks if the transaction occurred in the current month of the current yea
                                UsersTotalSpent += UserTransaction.Amount;
                            }
                            break;

                            case 3:
                              if (UserTransaction.Date.Year == TheDate.Year && 
                              YearMonthWeekDay(UserTransaction.Date) == YearMonthWeekDay(TheDate)) { 
                              UsersTotalSpent += UserTransaction.Amount;
                              }
                            break;

                            case 4:
                            if (UserTransaction.Date.Date == TheDate.Date) {   
                                UsersTotalSpent += UserTransaction.Amount;
                            }
                            break;

                            case 5:   
                            Console.WriteLine("Exiting....");
                            UserIsRunning = false;
                            break;
                            
                            default:
                            Console.WriteLine("Invalid choice, choose between 1-5");
                            break;
                        }
                   }
                    
                 } 
             }   
             else {
                Console.WriteLine("Invalid input, choose between 1-51");
             }
         }
     }
        
    
        // Function for users income
        public static void UsersMoneyIncome(List<UserTransaction> transactionList){

            bool UserIsRunning = true; // Boolean for the while loop

            while (UserIsRunning){

            DisplayMenu.DisplayIncomeMenu();// Displaying income menu, yearly etc

            string UserInput = Console.ReadLine()!.ToLower(); 

            if (int.TryParse(UserInput, out int UserChoice)){

            DateTime currentDate = DateTime.Now; // The date 

            decimal TotalIncome = 0;  // Ensuring consistent use of 'TotalIncome' with capital 'T'

            for (int i = 0; i < transactionList.Count; i++)
            {
                if (transactionList[i].Amount > 0)
                {
                    switch (UserChoice)
                    {
                        case 1: // Yearly income
                            if (transactionList[i].Date.Year == currentDate.Year)
                            {
                                TotalIncome += transactionList[i].Amount;
                            }
                            break;

                        case 2: // Monthly income
                            if (transactionList[i].Date.Year == currentDate.Year &&
                                transactionList[i].Date.Month == currentDate.Month)
                            {
                                TotalIncome += transactionList[i].Amount;
                            }
                            break;

                        case 3: // Weekly income
                            if (YearMonthWeekDay(transactionList[i].Date) == YearMonthWeekDay(currentDate))
                            {
                                TotalIncome += transactionList[i].Amount;
                            }
                            break;

                        case 4: // Daily income
                            if (transactionList[i].Date.Date == currentDate.Date)
                            {
                                TotalIncome += transactionList[i].Amount;
                            }
                            break;

                        case 5:
                            Console.WriteLine("Exiting income view...");
                            UserIsRunning = false;
                            break;

                        default:
                            Console.WriteLine("Invalid choice, please choose between 1-5.");
                            break;
                    }
                }
            }

            if (UserChoice >= 1 && UserChoice <= 4) 
            {
                Console.WriteLine($"\nTotal income for the selected period: {TotalIncome:C}");
            }
        }
        else
        {
            Console.WriteLine("Invalid input, please enter a number between 1-5.");
        }
    }
}        

        public static void UserNeedHelp() { 
   
            DisplayMenu.DisplayUserNeedHelp();

            string Feedback = Console.ReadLine()!.ToLower();
        }
        
        // Function for Year month week and day
        public static int YearMonthWeekDay(DateTime UsersDate) { 

            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var calendar = cultureInfo.Calendar; /// The calender 
            var weekOfYear = calendar.GetWeekOfYear(UsersDate, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday); /// The first day of the week
            return weekOfYear; 
        }
       
    }
}
