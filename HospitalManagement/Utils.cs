using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace HospitalManagement
{
    internal class Utils
    {
        //The main header seen on all screens
        public static void MenuHeader(string subHeader)
        {
            //Console.Clear();
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
            string[] lines;
            
            //try to retrieve a file with that userId. If not, fail the case.
            try
            {
                //read the file 
                string fileName = string.Format("{0}.txt", userId);
                lines = File.ReadAllLines(fileName);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Incorrect credentials, please try again.\n");
                return false;
            }

            //if there is a user with that ID, check the password
            string[] fileContent = lines[0].Split(',');

            if (password == fileContent[1])
            {
                return true;
            } else
            {
                Console.WriteLine("Incorrect credentials, please try again.\n");
                return false;
            }



            //Retrieves all files
            //finds ID
            //If ID is incorrect
            //Console.WriteLine("Invalid ID. Please try again.")

            //if (userId == 0 && password == "admin")
            //{
            //    return true;
            //} 
            //else if (userId == 1 && password == "doctor") {
            //    return true;
            //}
            //else
            //{
            //    Console.WriteLine("Incorrect credentials, please try again.\n");
            //    return false;
            //}

        }

        //load the specified user, and open their user menu
        public static void LoadAndRunUser(int userId)
        {
            string password, fullName, email, phoneNo, address;

            //read the file 
            string fileName = string.Format("{0}.txt", userId);
            string[] lines = File.ReadAllLines(fileName);

            //split the contents into another array.
            string[] fileContent = lines[0].Split(',');

            //if the userId starts with a 0, create an admin
            if (userId == 0)
            {
                Admin currentUser = new Admin(Convert.ToInt32(fileContent[0]), fileContent[1], fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
                currentUser.UserMenu();
            }
            if (userId == 1) {
                Doctor currentUser = new Doctor(Convert.ToInt32(fileContent[0]), fileContent[1], fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
                currentUser.UserMenu();
            }
            if (userId == 2) {
                Patient currentUser = new Patient(Convert.ToInt32(fileContent[0]), fileContent[1], fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
                currentUser.UserMenu();
            }
            

        }
    }
}