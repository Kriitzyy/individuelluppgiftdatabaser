using System;

namespace CoreofApplication // Namespace puts together codes.
{
    public static class DisplayMenu
    {
        public static void DisplayLoginOptions() {
            
            Console.WriteLine("Welcome to SecureSweBank -");
            Console.WriteLine("And it's new features!");

            Console.WriteLine("\nChoose an option: ");
            Console.WriteLine("[1] - Register new account!");
            Console.WriteLine("[2] - Already have a account? Login");
            Console.WriteLine("[3] - log out, and change user!");
            Console.WriteLine("[4] - Exit");
        }
        

        public static void DisplayMainMenu() {

            Console.WriteLine("SecureSwe Bank - Start Managing Your Finances!");
            
            Console.WriteLine("\nChoose between 1-7!");
            Console.WriteLine("[1] - Make a deposit!");
            Console.WriteLine("[2] - Delete transactions!");
            Console.WriteLine("[3] - View your current account balance!");
            Console.WriteLine("[4] - See money spent yearly, monthly, weekly, and daily!");
            Console.WriteLine("[5] - See income by year, monthly, weekly, and daily!");
            Console.WriteLine("[6] - Not understanding the program? Get help!");
            Console.WriteLine("[7] - Exit the program!");
        }

        public static void DisplayIncomeMenu() {

            Console.WriteLine("Choose a period of time to view your income:");

            Console.WriteLine("\n[1] - Yearly income");
            Console.WriteLine("[2] - Monthly income");
            Console.WriteLine("[3] - Weekly income");
            Console.WriteLine("[4] - Daily income");
            Console.WriteLine("[5] - Exit income view");
        }

        public static void DisplayMoneySpentMenu() {
            
            Console.WriteLine("Choose a period to see your spent money:");

            Console.WriteLine("\n[1] - View yearly spending");
            Console.WriteLine("[2] - View monthly spending");
            Console.WriteLine("[3] - View weekly spending");
            Console.WriteLine("[4] - View daily spending");
            Console.WriteLine("[5] - Exit spending option!");
        }

        public static void DisplayUserNeedHelp() {

            Console.WriteLine("Welcome to SecureSweBank");
            
            Console.WriteLine("\nWhen you are using SecureSwe Bank!");
            Console.WriteLine("Ensure you are using the program step by step!");
            Console.WriteLine("Start with number 1 and end with number 7!");
            Console.WriteLine("Hope the information helps!");
            Console.WriteLine("WARNING!! My program is case-sensitive, so please write correctly");

            Console.WriteLine("Write your feedback so we can improve our bank!");

            Console.WriteLine("\nRegards, SecureSwe Bank!");
        }
          
    }
}