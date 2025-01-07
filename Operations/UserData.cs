using System;
using System.IO;

namespace Client
{
    public class FileManager
    {
        // Filens sökväg där vi sparar användardata
        private const string FilePath = "users.txt"; 

        public static void SaveUserToFile(string username, string passwordhash, string email)
        {
            try
            {
                // Skapa en sträng som representerar användardata
                string userData = $"Username: {username}, PasswordHash: {passwordhash}, Email: {email}";

                // Skriv användardata till filen
                using (StreamWriter writer = new StreamWriter(FilePath, true))  // true = Append mode
                {
                    writer.WriteLine(userData);  // Skriv användardata till filen
                }

                Console.WriteLine("User data has been saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the user data: {ex.Message}");
            }
        }
    }
}
