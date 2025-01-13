using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Npgsql;

// Koden kanske försvinner för att man ska göra
// transaction operationerna i Npgsql
namespace CoreofApplication {
    public static class UserTransactionMethods {
        public static void UsersMoneyDeposit(List<UserTransaction> TransactionList) {

            Console.WriteLine("Write the amount of money you want to deposit:");
            string UserInput = Console.ReadLine()!.ToLower();

            if (decimal.TryParse(UserInput, out decimal amount)) {

                Console.WriteLine("Where does the money come from? (Salary, Loan, Sale, etc.)");
                string Source = Console.ReadLine()!;

                int ClientId = 2; // tillfällig ta bort när klar, så att add funkar 

                // Adding the new transaction to the list as an object
                TransactionList.Add(new UserTransaction(ClientId, amount, Source)); // Denna att den ska funka 
                Console.WriteLine("\nThe deposit is successful. Let's continue!");
            }
            else {
                Console.WriteLine("The amount is invalid. Enter a valid amount!");
            }
        }

        public static void UsersDeleteTransaction(List<UserTransaction> TransactionList) {

            if (TransactionList.Count == 0) {

                Console.WriteLine("There's no transactions to delete..");
                return;
            }

            Console.WriteLine("Here are the current transactions:");

            foreach (var UserTransaction in TransactionList) {

                Console.WriteLine($"\nAmount: {UserTransaction.Amount}, Description: {UserTransaction.Source}, Date: {UserTransaction.TransactionDate}");
            }

            Console.WriteLine("Enter the amount you want to delete:");
            string DeleteUsersAmount = Console.ReadLine()!.ToLower();

            Console.WriteLine("\nEnter the description you want to delete:");
            string DeleteUserDescription = Console.ReadLine()!.ToLower();

            if (decimal.TryParse(DeleteUsersAmount, out decimal amountToDelete)) {

                bool DeleteButton = false;

                for (int i = 0; i < TransactionList.Count; i++) {

                    if (TransactionList[i].Amount == amountToDelete &&
                        TransactionList[i].Source.Equals(DeleteUserDescription, StringComparison.OrdinalIgnoreCase)) {

                        UserTransaction.DeletedMoney.Add(TransactionList[i]);
                        TransactionList.RemoveAt(i);

                        Console.WriteLine("Transaction deleted successfully!");
                        DeleteButton = true;
                        return;
                    }
                }

                if (!DeleteButton) {

                    Console.WriteLine("No transactions were found with the specified amount and description.");
                }
            }
            else {

                Console.WriteLine("Write the correct amount you want to delete!");
            }
        }

        public static void UsersCurrentBalance(List<UserTransaction> TransactionList) {

            if (TransactionList.Count == 0) {

                Console.WriteLine("There are no transactions...");
                return;
            }

            foreach (var UsersTransactions in TransactionList) {

                Console.WriteLine($"Amount: {UsersTransactions.Amount}, Description: {UsersTransactions.Source}, Date: {UsersTransactions.TransactionDate}");
            }
        }

        public static void UsersMoneySpent(List<UserTransaction> TransactionList) {

            if (TransactionList.Count == 0 && UserTransaction.DeletedMoney.Count == 0) {

                Console.WriteLine("No transactions available to calculate money spent.");
                return;
            }

            bool UserIsUsing = true;

            while (UserIsUsing) {

                DisplayMenu.DisplayMoneySpentMenu();

                Console.WriteLine("Choose an option (1-5):");
                string UsersChoice = Console.ReadLine()!.ToLower();

                if (int.TryParse(UsersChoice, out int usersMenuOption)) {

                    DateTime currentDate = DateTime.Now;
                    decimal totalSpent = 0;

                    Func<UserTransaction, bool> filter = t => false;

                    switch (usersMenuOption) {

                        case 1: filter = t => t.TransactionDate.Year == currentDate.Year; break;
                        case 2: filter = t => t.TransactionDate.Year == currentDate.Year && t.TransactionDate.Month == currentDate.Month; break;
                        case 3: filter = t => t.TransactionDate.Year == currentDate.Year && YearMonthWeekDay(t.TransactionDate) == YearMonthWeekDay(currentDate); break;
                        case 4: filter = t => t.TransactionDate.Date == currentDate.Date; break;
                        case 5:
                            Console.WriteLine("Exiting money spent view...");
                            UserIsUsing = false;
                            continue;
                        default:
                            Console.WriteLine("Invalid choice, please choose a number between 1 and 5.");
                            continue;
                    }

                    if (usersMenuOption >= 1 && usersMenuOption <= 4) {

                        totalSpent += CalculateUsersSpentMoney(TransactionList, filter);
                        totalSpent += CalculateMoneySpent(filter);

                        Console.WriteLine($"\nTotal money spent under {UsersChoice} period is: {totalSpent:C}");
                    }
                }
                else {

                    Console.WriteLine("Invalid input, please enter a number between 1-5.");
                }
            }
        }

        public static void UsersMoneyIncome(List<UserTransaction> transactionList) {

            bool UserIsRunning = true; // Boolean for the while loop

            while (UserIsRunning) {

                DisplayMenu.DisplayIncomeMenu(); // Displaying income menu, yearly etc

                string UserInput = Console.ReadLine()!.ToLower();

                if (int.TryParse(UserInput, out int UserChoice)) {

                    DateTime currentDate = DateTime.Now; // The date 

                    decimal TotalIncome = 0;  // Ensuring consistent use of 'TotalIncome' with capital 'T'

                    for (int i = 0; i < transactionList.Count; i++) {

                        if (transactionList[i].Amount > 0) {

                            switch (UserChoice){ 

                                case 1: // Yearly income
                                    if (transactionList[i].TransactionDate.Year == currentDate.Year) {

                                        TotalIncome += transactionList[i].Amount;
                                    }
                                    break;

                                case 2: // Monthly income
                                    if (transactionList[i].TransactionDate.Year == currentDate.Year &&
                                        transactionList[i].TransactionDate.Month == currentDate.Month) {
                                        
                                        TotalIncome += transactionList[i].Amount;
                                    }
                                    break;

                                case 3: // Weekly income
                                    if (YearMonthWeekDay(transactionList[i].TransactionDate) == YearMonthWeekDay(currentDate)) {

                                        TotalIncome += transactionList[i].Amount;
                                    }
                                    break;

                                case 4: // Daily income
                                    if (transactionList[i].TransactionDate.Date == currentDate.Date) {

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

                    if (UserChoice >= 1 && UserChoice <= 4) {

                        Console.WriteLine($"\nTotal income for the selected period: {TotalIncome:C}");
                    }
                }
                else {
                    Console.WriteLine("Invalid input, please enter a number between 1-5.");
                }
            }
        }

        private static decimal CalculateUsersSpentMoney(List<UserTransaction> transactionList, Func<UserTransaction, bool> filter) {
            decimal total = 0;

            foreach (var transaction in transactionList) {

                if (filter(transaction) && transaction.Amount < 0) {

                    total += Math.Abs(transaction.Amount);
                }
            }

            return total;
        }

        public static decimal CalculateMoneySpent(Func<UserTransaction, bool> filter) {
            decimal total = 0;

            foreach (var transaction in UserTransaction.DeletedMoney) {

                if (filter(transaction) && transaction.Amount > 0) {

                    total += Math.Abs(transaction.Amount);
                }
            }

            return total;
        }

        public static int YearMonthWeekDay(DateTime UsersDate) {
            
            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var calendar = cultureInfo.Calendar;
            var weekOfYear = calendar.GetWeekOfYear(UsersDate, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }
    }
}