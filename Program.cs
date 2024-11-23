using System;
using System.Collections.Generic;
using SecureSweBank; // Use the namespace for access to Transaction class

namespace SecureSweBank // Namespace puts together codes.
{
    class Program {
        public static List<UserTransaction> TransactionList = new List<UserTransaction>(); 
        
        public static void Main(string[] args){

            bool stillrunning = true; 
            int usersmenuoptions;

            while (stillrunning){ 

                DisplayMenu.DisplayWelcomeTheme(); 

                DisplayMenu.DisplayMainMenu(); 
                
                string getUserInput = Console.ReadLine()!.ToLower(); // Reading user input and converts to lowercased
 
                if (int.TryParse(getUserInput, out usersmenuoptions)){ 

                    switch (usersmenuoptions){ 
                        case 1:
                        Console.WriteLine(""); // User logs in a account
                            break; 

                        case 2:
                             UserTransaction.UsersMoneyDeposit(TransactionList); 
                            break;    

                        case 3:
                            UserTransaction.UsersDeleteTransaction(TransactionList); 
                             // Fix the delete so the user can delete as much as he wants
                            break; // And the lefts overs are left, so that he can see the money spent. because it is not working

                        case 4:
                            UserTransaction.UsersCurrentBalance(TransactionList); 
                            break;

                        case 5: 
                            UserTransaction.UsersMoneySpent(TransactionList);
                            break;// ending the case 

                        case 6:
                            UserTransaction.UsersMoneyIncome(TransactionList); 
                            break;

                        case 7:
                            UserTransaction.UserNeedHelp();
                            break;

                        case 8:
                            Console.WriteLine("Exiting SecureSwe Bank..."); 
                            stillrunning = false; 
                            break; 

                        default: 
                            Console.WriteLine("ensure entering a choice between 1-7!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please, ensure you are choosing between 1-7!"); 
                }

                Console.WriteLine("\nPress any key to continue to the menu!..."); 

                Console.ReadKey(); 
            }   
        }
    }
}