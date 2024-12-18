﻿using System;
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
                             UserTransactionMethods.UsersMoneyDeposit(TransactionList); 
                            break;    

                        case 3:
                            UserTransactionMethods.UsersDeleteTransaction(TransactionList); 
                            break;

                        case 4:
                            UserTransactionMethods.UsersCurrentBalance(TransactionList); 
                            break;

                        case 5: 
                            UserTransactionMethods.UsersMoneySpent(TransactionList);
                            break;// ending the case 

                        case 6:
                            UserTransactionMethods.UsersMoneyIncome(TransactionList); 
                            break;

                        case 7:
                            DisplayMenu.UserNeedHelp();
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