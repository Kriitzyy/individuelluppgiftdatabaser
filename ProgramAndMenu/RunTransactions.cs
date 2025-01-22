using System;
using Npgsql;
using Client;
using CoreofApplication;
using System.Linq.Expressions;

class RunTransactions 
{ 

    // Calling Deposit method after checking if the user is logged in

    // Funkar bra, och testad
    public static async Task CallingDeposit() 
    {
        // Check if the user is logged in
        if (Program.LoggedInUser == null)
        {
            Console.WriteLine("No user is logged in. Please log in first. ");
            return; // If no user is logged in, return from the method
        }

        // Ask the user for the deposit amount
        Console.WriteLine("Enter the amount you want to deposit: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
        {
            // Ask for a description or source of the transaction
            Console.WriteLine("Enter a description for the deposit:");
            string description = Console.ReadLine() ?? "No description provided";  // Default description if none is provided

            // Create a transaction object
            var DepostTransaction = new Transaction
            {
                ClientId = Program.LoggedInUser.Id,  // Use the logged-in user's ID
                amount = depositAmount,
                source = description
            };

            // Call the DepositTransactions method
            bool success = await DepositTransaction.DepositTransactions(DepostTransaction);

            if (success)
            {
                Console.WriteLine("Deposit was successful!");
            }
            else
            {
                Console.WriteLine("Failed to process the deposit.");
            }
        }
        else
        {
            Console.WriteLine("Invalid amount entered. Please enter a valid decimal number.");
        }
    }

    // Funkar bra, och testad
    // MEN om användaren trycker no ska han skickas tillbaka 
    // Till transaktion menyn
    public static async Task CallingDelete() 
    {
        if (Program.LoggedInUser == null) 
        {
            Console.WriteLine("No user is logged in. Please log in first.");
            return; // If no user is logged in, return from the method
        }

        Console.WriteLine("Are you sure you want to delete all your transactions? (yes/no)");
        string confirmation = Console.ReadLine()?.ToLower();

        if (confirmation == "yes")
        {
            // Create a transaction object with the logged-in user's ClientId
            var DeleteObject = new Transaction
            {
                ClientId = Program.LoggedInUser.Id // Use the logged-in user's ClientId
            };

            // Call the DeleteTransactions method
            await DeleteTransaction.DeleteTransactions(DeleteObject); // Delete the transactions

            Console.WriteLine("All transactions for your account have been deleted.");
        }
        else if (confirmation == "no") 
        {
            Console.WriteLine("Going back to transaction menu..");
            return; 
        }
        else 
        {
            Console.WriteLine("Ensure typing 'yes' or 'no' ");
        }
    }

    // Funkar bra och testad 
    public static async Task CallingCurrentBalance() 
    {
        if (Program.LoggedInUser == null) 
        {
            Console.WriteLine("No user is logged in..");
            return;
        }

        try 
        {
            var CurrentBalanceObject = new GetTransaction();
            var currentTransaction = new Transaction
            {
                ClientId = Program.LoggedInUser.Id // Use the logged-in user's ClientId
            };

            decimal currentBalance = await CurrentBalanceObject.GetCurrentBalance(currentTransaction);
            Console.WriteLine($"Your current balance is: {currentBalance:C}");
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"An error occurred while retrieving the balance: {ex.Message}");
        }
    }

    // Updated method to view spent money with automatic date calculations
    public static async Task CallingMoneySpent()
    {
        if (Program.LoggedInUser == null)
        {
            Console.WriteLine("No user is logged in...");
            return;
        }

        DisplayMenu.DisplayMoneySpentMenu();

        string choice = Console.ReadLine()?.ToLower();

        // Get the current year, month, week, and date for convenience
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        int currentWeek = GetWeekOfYear(DateTime.Now); // A helper method to get the current week number
        DateTime currentDay = DateTime.Now.Date; // Current day (ignoring time part)

        switch (choice)
        {
            case "1":
                // Show spending for the current year
                decimal totalSpentYearly = await MoneySpentCalculator.MoneySpentPeriods(new Transaction { ClientId = Program.LoggedInUser.Id },
                 "year", year: currentYear);
                Console.WriteLine($"Total money spent in {currentYear}: {totalSpentYearly:C}");
                break;

            case "2":
                // Show spending for the current month
                decimal totalSpentMonthly = await MoneySpentCalculator.MoneySpentPeriods(new Transaction { ClientId = Program.LoggedInUser.Id }, 
                "month", year: currentYear, month: currentMonth);
                Console.WriteLine($"Total money spent in {currentYear}-{currentMonth:D2}: {totalSpentMonthly:C}");
                break;

            case "3":
                // Show spending for the current week
                decimal totalSpentWeekly = await MoneySpentCalculator.MoneySpentPeriods(new Transaction { ClientId = Program.LoggedInUser.Id }, 
                "week", year: currentYear, week: currentWeek);
                Console.WriteLine($"Total money spent in week {currentWeek}, {currentYear}: {totalSpentWeekly:C}");
                break;

            case "4":
                // Show spending for the current day
                decimal totalSpentDaily = await MoneySpentCalculator.MoneySpentPeriods(new Transaction { ClientId = Program.LoggedInUser.Id }, 
                "day", day: currentDay);
                Console.WriteLine($"Total money spent on {currentDay.ToShortDateString()}: {totalSpentDaily:C}");
                break;

            case "5":
                Console.WriteLine("Exiting spending options.");
                return; // Exiting the menu and going back to the previous menu

            default:
                Console.WriteLine("Invalid option. Please choose a valid option (1-5).");
                break;
        }
    }

    // Helper method to get the current week of the year
    private static int GetWeekOfYear(DateTime date)
    {
        var calendar = System.Globalization.CultureInfo.InvariantCulture.Calendar;
        return calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    // Implemented method to view income for different periods
    public static async Task CallingIncomePeriods() 
    {
        if (Program.LoggedInUser == null) 
        {
            Console.WriteLine("No user is logged in..");
            return;
        }

        DisplayMenu.DisplayIncomeMenu();

        string choice = Console.ReadLine()?.ToLower();

        // Get the current year, month, week, and date for convenience
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        int currentWeek = GetWeekOfYear(DateTime.Now); // A helper method to get the current week number
        DateTime currentDay = DateTime.Now.Date; // Current day (ignoring time part)

        switch (choice)
        {
            case "1":
                // Show income for the current year
                var (startDateYear, endDateYear) = IncomeTransaction.GetPeriods("year");
                decimal totalIncomeYearly = await IncomeTransaction.GetIncomePeriods(Program.LoggedInUser.Id, startDateYear, endDateYear);
                Console.WriteLine($"Total income in {currentYear}: {totalIncomeYearly:C}");
                break;

            case "2":
                // Show income for the current month
                var (startDateMonth, endDateMonth) = IncomeTransaction.GetPeriods("month");
                decimal totalIncomeMonthly = await IncomeTransaction.GetIncomePeriods(Program.LoggedInUser.Id, startDateMonth, endDateMonth);
                Console.WriteLine($"Total income in {currentYear}-{currentMonth:D2}: {totalIncomeMonthly:C}");
                break;

            case "3":
                // Show income for the current week
                var (startDateWeek, endDateWeek) = IncomeTransaction.GetPeriods("week");
                decimal totalIncomeWeekly = await IncomeTransaction.GetIncomePeriods(Program.LoggedInUser.Id, startDateWeek, endDateWeek);
                Console.WriteLine($"Total income in week {currentWeek}, {currentYear}: {totalIncomeWeekly:C}");
                break;

            case "4":
                // Show income for the current day
                var (startDateDay, endDateDay) = IncomeTransaction.GetPeriods("day");
                decimal totalIncomeDaily = await IncomeTransaction.GetIncomePeriods(Program.LoggedInUser.Id, startDateDay, endDateDay);
                Console.WriteLine($"Total income on {currentDay.ToShortDateString()}: {totalIncomeDaily:C}");
                break;

            case "5":
                Console.WriteLine("Exiting income options.");
                return; // Exiting the menu and going back to the previous menu

            default:
                Console.WriteLine("Invalid option. Please choose a valid option (1-5).");
                break;
        }
    }

    // Example method to log in a user (to set LoggedInUser)
    public static void LoginUser(Clients user)
    {
        Program.LoggedInUser = user;
        Console.WriteLine($"Logged in as {Program.LoggedInUser.username}");
    }
}
