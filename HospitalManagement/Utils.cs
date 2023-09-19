using System.Net.NetworkInformation;

namespace HospitalManagement
{
    internal class Utils
    {
        //The main header seen on all screens
        public static void MenuHeader(string subHeader)
        {
            Console.Clear();
            Console.WriteLine("┌───────────────────────────────────────────┐");
            Console.WriteLine("│     DOTNET Hospital Management System     │");    
            Console.WriteLine("├───────────────────────────────────────────┤");
            Console.WriteLine("│                  {0}                      │", subHeader);
            Console.WriteLine("└───────────────────────────────────────────┘");
            Console.WriteLine();
        }

        //the login functionality. Repeats until user enters valid credentials. When valid, return the ID.
        public static int Login()
        {
            int userId;
            string password;

            do
            {
                Console.Write("User ID: ");
                userId = Convert.ToInt32(Console.ReadLine());
                Console.Write("Password: ");
                password = Console.ReadLine();

                //Console.WriteLine("Please input numbers for the user ID\n");

            } while (!LoginCheck(userId, password));

            Console.WriteLine("Login successful");
            Console.ReadKey();

            return userId;
        }

        //returns true when there is a valid credential.
        static Boolean LoginCheck(int userId, string password)
        {
            //Retrieves all files
            //finds ID
            //If ID is incorrect
            //Console.WriteLine("Invalid ID. Please try again.")

            if (userId == 0 && password == "admin")
            {
                return true;
            } 
            else if (userId == 1 && password == "doctor") {
                return true;
            }
            else
            {
                Console.WriteLine("Incorrect credentials, please try again.\n");
                return false;
            }
            
        }
    }
}