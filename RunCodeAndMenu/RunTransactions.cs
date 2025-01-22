using System;
using Npgsql;
using Client;
using CoreofApplication;
using System.Linq.Expressions;

// Denna fil hanterar endast transaction 
// Integreringen med användaren då main blev för mycket 
// Och det blev tydligare så

// integreringens Class 
class RunTransactions { 

    // Deposit metod som hanterar integreringen för Deposit 
    public static async Task CallingDeposit() { 

        Console.Clear();

        // Kontrollerar om användaren är inloggad 
        if (Program.LoggedInUser == null) {

            Console.WriteLine("No user is logged in. Please log in first. ");
            return; // Om ingen användare är inloggad, returnera från metoden
        }

        // Depositionen 
        Console.WriteLine("Enter the amount you want to deposit: ");

        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount)) {

            Console.Clear();

            // Källa för depositionen
            Console.WriteLine("Enter a description for the deposit:");
            string description = Console.ReadLine() ?? "No description provided"; 

            // Skapar ett transaktionsobjekt
            var DepostTransaction = new Transaction {

                // Använder den inloggade användarens ID
                ClientId = Program.LoggedInUser.Id,  
                amount = depositAmount,
                source = description
            };

            Console.Clear();

            // Anropar metoden för att sätta in pengarna
            bool success = await DepositClass.DepositTransactions(DepostTransaction);

            // Om depositionen lyckades 
            if (success) {

                Console.WriteLine("Deposit was successful! Press any key to continue..");
                Console.ReadKey();
            }
            else {
                // Om den misslyckas
                Console.WriteLine("Failed to process the deposit.");
                return;
            }
        }
        else {
            // Om användaren skriver annat än nummer
            Console.WriteLine("Invalid amount entered. Please enter a valid decimal number.");
            return;
        }
    }

    // Delete metod som hanterar integreringen för Delete  
    public static async Task CallingDelete() {

        Console.Clear();

        // Kollar om användaren är inloggad 
        if (Program.LoggedInUser == null)  {

            Console.WriteLine("No user is logged in. Please log in first.");
            return; // Om ingen användare är inloggad, returnera från metoden
        }

        Console.WriteLine("Are you sure you want to delete all your transactions? (yes/no)");
        string confirmation = Console.ReadLine()!.ToLower();

        // Om användaren vill radera sina transaktioner
        if (confirmation == "yes") {

            Console.Clear();

            // Skapar ett transaktionsobjekt med den inloggade användarens ClientId
            var DeleteObject = new Transaction
            {
                ClientId = Program.LoggedInUser.Id // Använder den inloggade användarens ClientId
            };

            // Anropar DeleteTransactions metoden
            await DeleteTransaction.DeleteTransactions(DeleteObject); // Raderar transaktionerna

            Console.Clear();

            Console.WriteLine("All transactions for your account have been deleted.");
        }
        
        // Om användaren inte vill radera transaktionerna
        else if (confirmation == "no") {

            Console.WriteLine("Going back to transaction menu..");
            return; // Skickas användaren tillbaka
        }
        else {

            // Felmeddelande om man skriver något annat än 'yes' eller 'no'
            Console.WriteLine("Ensure typing 'yes' or 'no' ");
        }
    }

    // CurrentBalance metod som hanterar integreringen för Balance 
    public static async Task CallingCurrentBalance() {

        Console.Clear();

        // Kontrollerar om användaren är inloggad
        if (Program.LoggedInUser == null) {

            Console.WriteLine("No user is logged in..");
            return;
        }

        try {

            var CurrentBalanceObject = new GetTransaction();

            var currentTransaction = new Transaction
            {
                // Använder den inloggade användarens ClientId
                ClientId = Program.LoggedInUser.Id 
            };

            Console.Clear();

            // Hämtar metoden CurrentBalance
            decimal currentBalance = await CurrentBalanceObject.GetCurrentBalance(currentTransaction);

            Console.WriteLine($"Your current balance is: {currentBalance:C}");
        }

        catch (Exception ex) {

            // Om det misslykas
            Console.WriteLine($"An error occurred while retrieving the balance: {ex.Message}");
        }
    }

    // Pengar spenderat metod som hanterar integreringen för Spenderade pengar 
    public static async Task CallingMoneySpent() {

        Console.Clear();
         
        // Kollar om användaren är inloggad
        if (Program.LoggedInUser == null) {

            Console.WriteLine("No user is logged in...");
            return;
        }

        // Meny för perioderna, år, månad, vecka och dag
        DisplayMenu.DisplayMoneySpentMenu();

        string choice = Console.ReadLine()!.ToLower();

        // Hämtar nuvarande år, månad, vecka och datum för bekvämlighet
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        int currentWeek = GetWeekOfYear(DateTime.Now); // En funktion för att få nuvarande vecka
        DateTime currentDay = DateTime.Now.Date; // Nuvarande dag 

        Console.Clear();

        switch (choice) {

            case "1":
                // Visa spenderade pengar för nuvarande år
                decimal totalSpentYearly = await MoneySpentCalculator.MoneySpentPeriods
                (new Transaction { ClientId = Program.LoggedInUser.Id },
                 "year", year: currentYear);
                Console.WriteLine($"Total money spent in {currentYear}: {totalSpentYearly:C}");
                break;

            case "2":
                // Visa spenderade pengar för nuvarande månad
                decimal totalSpentMonthly = await MoneySpentCalculator.MoneySpentPeriods
                (new Transaction { ClientId = Program.LoggedInUser.Id }, 
                "month", year: currentYear, month: currentMonth);
                Console.WriteLine($"Total money spent in {currentYear}-{currentMonth:D2}: {totalSpentMonthly:C}");
                break;

            case "3":
                // Visa spenderade pengar för nuvarande vecka
                decimal totalSpentWeekly = await MoneySpentCalculator.MoneySpentPeriods
                (new Transaction { ClientId = Program.LoggedInUser.Id }, 
                "week", year: currentYear, week: currentWeek);
                Console.WriteLine($"Total money spent in week {currentWeek}, {currentYear}: {totalSpentWeekly:C}");
                break;

            case "4":
                // Visa spenderade pengar för nuvarande dag
                decimal totalSpentDaily = await MoneySpentCalculator.MoneySpentPeriods
                (new Transaction { ClientId = Program.LoggedInUser.Id }, 
                "day", day: currentDay);
                Console.WriteLine($"Total money spent on {currentDay.ToShortDateString()}: {totalSpentDaily:C}");
                break;

            case "5":
                Console.WriteLine("Exiting spending options.");
                return; // Avslutar menyn och går tillbaka till transaktion meny

            default:
                Console.WriteLine("Invalid option. Please choose a valid option (1-5).");
                break;
        }
    }

    // Funktion för att få nuvarande vecka på året
    private static int GetWeekOfYear(DateTime date) {

        var calendar = System.Globalization.CultureInfo.InvariantCulture.Calendar;
        return calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    // Income metod som hanterar integreringen för income perioderna 
    public static async Task CallingIncomePeriods() {

        Console.Clear();

        // Kontrollerar om användaren är inloggad
        if (Program.LoggedInUser == null) {

            Console.WriteLine("No user is logged in..");
            return;
        }

        // Meny för perioderna
        DisplayMenu.DisplayIncomeMenu();

        string choice =  Console.ReadLine()!.ToLower();

        // Hämtar nuvarande år, månad, vecka och datum för bekvämlighet
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        int currentWeek = GetWeekOfYear(DateTime.Now); // En funktion för att få nuvarande vecka
        DateTime currentDay = DateTime.Now.Date; // Nuvarande dag 

        Console.Clear();

        switch (choice) {

            case "1":
                // Visa inkomst för nuvarande år
                var (startDateYear, endDateYear) = IncomeTransaction.GetPeriods("year");
                decimal totalIncomeYearly = await IncomeTransaction.GetIncomePeriods(Program.LoggedInUser.Id, startDateYear, endDateYear);
                Console.WriteLine($"Total income in {currentYear}: {totalIncomeYearly:C}");
                break;

            case "2":
                // Visa inkomst för nuvarande månad
                var (startDateMonth, endDateMonth) = IncomeTransaction.GetPeriods("month");
                decimal totalIncomeMonthly = await IncomeTransaction.GetIncomePeriods(Program.LoggedInUser.Id, startDateMonth, endDateMonth);
                Console.WriteLine($"Total income in {currentYear}-{currentMonth:D2}: {totalIncomeMonthly:C}");
                break;

            case "3":
                // Visa inkomst för nuvarande vecka
                var (startDateWeek, endDateWeek) = IncomeTransaction.GetPeriods("week");
                decimal totalIncomeWeekly = await IncomeTransaction.GetIncomePeriods(Program.LoggedInUser.Id, startDateWeek, endDateWeek);
                Console.WriteLine($"Total income in week {currentWeek}, {currentYear}: {totalIncomeWeekly:C}");
                break;

            case "4":
                // Visa inkomst för nuvarande dag
                var (startDateDay, endDateDay) = IncomeTransaction.GetPeriods("day");
                decimal totalIncomeDaily = await IncomeTransaction.GetIncomePeriods(Program.LoggedInUser.Id, startDateDay, endDateDay);
                Console.WriteLine($"Total income on {currentDay.ToShortDateString()}: {totalIncomeDaily:C}");
                break;

            case "5":
                Console.WriteLine("Exiting income options.");
                return; // Avslutar menyn och går tillbaka till transaktion meny

            default:
                Console.WriteLine("Invalid option. Please choose a valid option (1-5).");
                break;
        }
    } 
}
