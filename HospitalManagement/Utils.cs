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
            string[] fileContent;

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
                fileContent = ReadFile(Convert.ToString(userId));
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Incorrect credentials, please try again.\n");
                return false;
            }

            //if there is a user with that ID, check the password
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
                //if the user hits backspace, delete a character from UI and from the string
                if (keyInput.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[0..^1];
                } else
                {
                    //Add the character inputted into the password string, and write a * to show a character was typed
                    password += keyInput.KeyChar;
                    Console.Write("*");
                }
                keyInput = Console.ReadKey(true);
            }

            return password;
        }

        //load the specified user, and open their user menu
        public static void LoadAndRunUser(int userId)
        {
            string[] fileContent;

            fileContent = ReadFile(Convert.ToString(userId));

            //if the userId starts with a 1, create an admin
            if (userId < 200000)
            {
                Admin currentUser = new Admin(Convert.ToInt32(fileContent[0]), fileContent[1], fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
                currentUser.UserMenu();
            }
            //else if userId starts with a 2, create a doctor 
            else if (userId < 300000) {
                Doctor currentUser = new Doctor(Convert.ToInt32(fileContent[0]), fileContent[1], fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
                currentUser.UserMenu();
            }
            //else if their id starts with a 3, create a patient
            else if (userId < 400000) {
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

        //Use to list all doctors or patients
        public static void ListAll(string userType)
        {
            int wantedID;
            // get all the txt files in the debug/net6.0 directory
            //string[] files = Directory.GetFiles(@".", "2*.txt");

            //if the user wanted all doctors, find files that start with a 2. Else if they want patients, which start with a 3. Default to admins if nothing else.
            if (userType == "doctors")
            {
                wantedID = 2;
            }
            else if (userType == "patients")
            {
                wantedID = 3;
            }
            else
            {
                wantedID = 1;
            }

            string searchPattern = string.Format("{0}*.txt", wantedID);
            string[] files = Directory.GetFiles(@".", searchPattern); 

            foreach (string filePath in files)
            {
                //check if that file starts with the wanted userType code.
                Console.WriteLine(filePath);
            }
        }
    }
}