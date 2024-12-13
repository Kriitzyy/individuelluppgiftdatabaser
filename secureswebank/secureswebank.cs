using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using System.Linq; // Dessa är classer/metoder dom har specialla funktioner i sig som gör det möjligt att använda funktionerna när du har classen 
using Microsoft.VisualBasic;
using System.Reflection.Metadata;
using System.ComponentModel.DataAnnotations;

namespace SecureSweBank { // Namespace puts together codes.

    public class UserTransaction { 
        // Properties of a transaction 
        public decimal Amount { get; set; }
        public string Source { get; set; } 
        public DateTime Date { get; set; }
        public static List<UserTransaction> DeletedMoney = new List<UserTransaction>(); 
        
            public UserTransaction(decimal amount, string source) {
            Amount = amount;
            Source = source; 
            Date = DateTime.Now; 
            List<UserTransaction> DeletedMoney = new();
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

            if (decimal.TryParse(DeleteUsersAmount, out decimal amountToDelete)){ 

                bool DeleteButton = false;

                for (int i = 0; i < TransactionList.Count; i++){

                    if (TransactionList[i].Amount == amountToDelete  && // Checking if the amount is stored as index
                    TransactionList[i].Source.Equals(DeleteUserSource, StringComparison.OrdinalIgnoreCase)){ 

                    DeletedMoney.Add(TransactionList[i]);

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
            else{   

                Console.WriteLine("Write the correct amount you want to delete!");
            }
            
        }
      
        public static void UsersCurrentBalance(List<UserTransaction> TransactionList){  // Fucntion to view current balance

            if (TransactionList.Count == 0){ // If the user doesnt have money

                Console.WriteLine("There are no transactions...");
                return;
            }
          
            foreach (var UsersTransactions in TransactionList){ // Foreach loop to show the current amount with source and date

                Console.WriteLine($"Amount: {UsersTransactions.Amount}, Source: {UsersTransactions.Source}, Date: {UsersTransactions.Date}");
            }
            
        }
        public static void UsersMoneySpent(List<UserTransaction> TransactionList){

            if (TransactionList.Count == 0 && DeletedMoney.Count == 0){

                Console.WriteLine("No transactions available to calculate money spent.");
                return;
            }

            bool UserIsUsing = true;

            while (UserIsUsing){

            DisplayMenu.DisplayMoneySpentMenu(); // Display the menu for spending options

            Console.WriteLine("Choose an option (1-5):");

            string UsersChoice = Console.ReadLine()!.ToLower();

                if (int.TryParse(UsersChoice, out int usersMenuOption)){

                    DateTime currentDate = DateTime.Now; // Current date

                    decimal totalSpent = 0; // Total money spent

                    Func<UserTransaction, bool> filter = t => false; // Default to no filtering

                        switch (usersMenuOption){

                            case 1: // Yearly spending
                            filter = t => t.Date.Year == currentDate.Year;
                            break;

                            case 2: // Monthly spending
                            filter = t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month;
                            break;

                            case 3: // Weekly spending
                            filter = t => t.Date.Year == currentDate.Year && YearMonthWeekDay(t.Date) == YearMonthWeekDay(currentDate);
                            break;

                            case 4: // Daily spending
                            filter = t => t.Date.Date == currentDate.Date;
                            break;

                            case 5: // Exit
                            Console.WriteLine("Exiting money spent view...");
                            UserIsUsing = false;
                            continue;

                            default:
                            Console.WriteLine("Invalid choice, please choose a number between 1 and 5.");
                            continue;
                        }

                        if (usersMenuOption >= 1 && usersMenuOption <= 4){

                            // Calculate spending from both lists
                            totalSpent += CalculateUsersSpentMoney(TransactionList, filter); // Spending from the main list
                            totalSpent += CalculateMoneySpent(filter); // Spending from the deleted list

                            // Display the result
                            Console.WriteLine($"\nTotal money spent under {UsersChoice} period is: {totalSpent:C}");
                        }
                    }
                    else{
                        Console.WriteLine("Invalid input, please enter a number between 1-5.");
                }
            }
        }
        public static decimal CalculateMoneySpent(Func<UserTransaction, bool> filter){

            decimal total = 0;

            foreach (var transaction in DeletedMoney){

                if (filter(transaction) && transaction.Amount > 0) {// Deleted money was originally a deposit (positive)

                    total += Math.Abs(transaction.Amount); // Treat it as spending
            }
        }
        return total;
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

            for (int i = 0; i < transactionList.Count; i++){

                if (transactionList[i].Amount > 0){

                    switch (UserChoice){

                        case 1: // Yearly income
                            if (transactionList[i].Date.Year == currentDate.Year){
                                TotalIncome += transactionList[i].Amount;
                            }
                            break;

                        case 2: // Monthly income
                            if (transactionList[i].Date.Year == currentDate.Year &&
                                transactionList[i].Date.Month == currentDate.Month){
                                TotalIncome += transactionList[i].Amount;
                            }
                            break;

                        case 3: // Weekly income
                            if (YearMonthWeekDay(transactionList[i].Date) == YearMonthWeekDay(currentDate)){
                                TotalIncome += transactionList[i].Amount;
                            }
                            break;

                        case 4: // Daily income
                            if (transactionList[i].Date.Date == currentDate.Date){
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
            
            if (UserChoice >= 1 && UserChoice <= 4){
                Console.WriteLine($"\nTotal income for the selected period: {TotalIncome:C}");
            }
        }
            else {
            Console.WriteLine("Invalid input, please enter a number between 1-5.");
        }
    }
}
        public static void UserNeedHelp() { 
   
            DisplayMenu.DisplayUserNeedHelp();

            string Feedback = Console.ReadLine()!.ToLower();
        }
        public static int YearMonthWeekDay(DateTime UsersDate) { 

            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var calendar = cultureInfo.Calendar; /// The calender 
            var weekOfYear = calendar.GetWeekOfYear(UsersDate, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday); /// The first day of the week
            return weekOfYear; 
        }
        private static decimal CalculateUsersSpentMoney(List<UserTransaction> transactionList, Func<UserTransaction, bool> filter){

        decimal total = 0;

        foreach (var transaction in transactionList){

        if (filter(transaction) && transaction.Amount < 0) // Ensure we're dealing with expenses
        {
            total += Math.Abs(transaction.Amount); // Add absolute value of the negative amount
        }
    }
    return total;
    }
  }
}

