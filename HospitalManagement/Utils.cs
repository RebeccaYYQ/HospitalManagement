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
            string userId, password;
            Console.WriteLine("Welcome to the DOTNET Hospital Management System.\nType 'Exit' in the userID field to exit the system.\n");

            //loops until there is a valid ID, or the user enters 'Exit'
            do
            {
                Console.Write("User ID: ");
                userId = Console.ReadLine();
                //check the user did not type 'Exit'
                if (userId == "Exit" || userId == "exit")
                {
                    Environment.Exit(0);
                }
                password = HidePassword();
                Console.WriteLine();

            } while (!LoginCheck(userId, password));

            Console.WriteLine("Login successful");
            Console.ReadKey();

            return Convert.ToInt32(userId);
        }

        //returns true when there is a valid credential.
        static Boolean LoginCheck(string userId, string password)
        {
            string[] lines;

            //check if userID is an integer. If not, fail the case
            try
            {
                Convert.ToInt32(userId);
            }
            catch (FormatException)
            {
                Console.WriteLine("Please input numbers for the user ID.\n");
                return false;
            }

            //try to retrieve a file with that userId. If not, fail the case.
            try
            {
                //read the file 
                string fileName = string.Format("{0}.txt", userId);
                lines = File.ReadAllLines(fileName);
            }
            catch (FileNotFoundException)
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
        }

        //Password masking
        public static string HidePassword()
        {
            string password = "";
            Console.Write("Password: ");

            //gets a single key input from the user. Does not display it
            ConsoleKeyInfo keyInput = Console.ReadKey(true);

            //While the user does not hit the 'Enter' key
            while (keyInput.Key != ConsoleKey.Enter)
            {
                //Add the character inputted into the password string, and write a * to show a character was typed
                password += keyInput.KeyChar;
                Console.Write("*");

                keyInput = Console.ReadKey(true);
            }

            return password;
        }

        //load the specified user, and open their user menu
        public static void LoadAndRunUser(int userId)
        {
            string password, fullName, email, phoneNo, address;
            string[] fileContent;

            fileContent = ReadFile(Convert.ToString(userId));

            //if the userId starts with a 0, create an admin
            if (userId == 0)
            {
                Admin currentUser = new Admin(Convert.ToInt32(fileContent[0]), fileContent[1], fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
                currentUser.UserMenu();
            }
            //else if userId starts with a 1, create a doctor 
            else if (userId == 1) {
                Doctor currentUser = new Doctor(Convert.ToInt32(fileContent[0]), fileContent[1], fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
                currentUser.UserMenu();
            }
            //else if their id starts with a 2, create a patient
            else if (userId == 2) {
                Patient currentUser = new Patient(Convert.ToInt32(fileContent[0]), fileContent[1], fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
                currentUser.UserMenu();
            }
        }

        //Reads a file and splits the data into a string array
        public static string[] ReadFile(string fileName)
        {
            //read the file
            string[] lines = File.ReadAllLines(string.Format("{0}.txt", fileName));

            //split the contents into another array.
            string[] fileContent = lines[0].Split(',');

            return fileContent;
        }
    }
}