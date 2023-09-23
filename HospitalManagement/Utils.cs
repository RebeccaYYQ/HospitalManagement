using System.Net;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

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
            string userId, password;
            Console.WriteLine("Welcome to the DOTNET Hospital Management System.\nType 'Exit' in the userID field to exit the system.\n");

            //loops until there is a valid login, or the user enters 'Exit'
            do
            {
                Console.Write("User ID: ");
                userId = Console.ReadLine();
                //check the user did not type 'Exit'
                if (userId == "Exit" || userId == "exit")
                {
                    Environment.Exit(0);
                }

                //get the user password. As they enter it, hide their input with *
                password = GetAndHidePassword();
                Console.WriteLine();
            } while (!CheckLogin(userId, password));

            Console.WriteLine("Login successful");
            Console.ReadKey();

            return Convert.ToInt32(userId);
        }

        //returns true when there is a valid credential.
        static Boolean CheckLogin(string userId, string password)
        {
            string[] fileContent = CheckUserExists(userId);

            //Check if the user exists. If not (i.e. the array returned is empty), fail the check immediately.
            if (fileContent.Length == 0)
            {
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

        //To check if a user exists. If there is, return the user's file contents. If not, retun an empty string[]
        public static string[] CheckUserExists(string userId)
        {
            string[] fileContent = new string[0];

            //check if userID is an integer. If not, fail the check
            try
            {
                Convert.ToInt32(userId);
            }
            catch (FormatException)
            {
                Console.WriteLine("Please input numbers for the user ID.\n");
                return fileContent;
            }

            //try to retrieve a file with that userId. If the file doesn't exist, fail the check.
            //If a file can be retrieved, then that user exists. Return true.
            try
            {
                fileContent = ReadFile(Convert.ToString(userId));
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Invalid ID, please try again.\n");
            }
            return fileContent;
        }

        //Password masking
        public static string GetAndHidePassword()
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
                Admin currentUser = new Admin(Convert.ToInt32(fileContent[0]), fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
                currentUser.UserMenu();
            }
            //else if userId starts with a 2, create a doctor 
            else if (userId < 300000) {
                Doctor currentUser = new Doctor(Convert.ToInt32(fileContent[0]), fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
                currentUser.UserMenu();
            }
            //else if their id starts with a 3, create a patient
            else if (userId < 400000) {
                Patient currentUser = new Patient(Convert.ToInt32(fileContent[0]), fileContent[2], fileContent[3], fileContent[4], fileContent[5], fileContent[6]);
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

        //List all the doctors
        public static void ListAllDoctors()
        {
            string[] lines;

            //all doctors have an ID starting with 2, so find files in the debug/net6.0 directory starting with 2.
            string[] files = Directory.GetFiles(@".", "2*.txt");

            //print the header
            Utils.MenuHeader("All Doctors");
            Console.WriteLine("All doctors registered in the DOTNET Hospital System");
            Console.WriteLine();
            Console.WriteLine("ID       | Name             | Email Address         | Phone      | Address                                   ");
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────");

            //print out the contents of that file
            foreach (string filePath in files)
            {
                lines = File.ReadAllLines(filePath);
                string[] fileContent = lines[0].Split(',');
                PrintDoctorDetails(fileContent);
            }
        }

        //List all the patients
        public static void ListAllPatients()
        {
            string[] lines;

            //all patients have an ID starting with 3, so find files in the debug/net6.0 directory starting with 3.
            string[] files = Directory.GetFiles(@".", "3*.txt");

            //print the header
            Utils.MenuHeader("All Patients");
            Console.WriteLine("All patients registered in the DOTNET Hospital System");
            Console.WriteLine();
            Console.WriteLine("ID       | Name             | Doctor             | Email Address         | Phone      | Address                                   ");
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────");

            //print out the contents of that file
            foreach (string filePath in files)
            {
                lines = File.ReadAllLines(filePath);
                string[] fileContent = lines[0].Split(',');
                PrintPatientDetails(fileContent);
            }
        }

        //method to check a specific doctor, using ID.
        public static void CheckDoctorDetails()
        {
            string userInput;
            string[] fileContent;

            MenuHeader("Doctor Details");
            Console.WriteLine("Please enter the ID of the doctor who's details you are checking.");
            Console.WriteLine("Enter 'Exit' to return to the menu.\n");

            //Repeat until the user exits, or the user enters a valid doctor's ID
            do
            {
                Console.Write("ID: ");
                userInput = Console.ReadLine();

                //if the user entered 'exit', end the method
                if (userInput == "Exit" || userInput == "exit")
                {
                    return;
                }
                //if the inputted ID starts with a 2, the inputted ID may be a doctor. Check if they exist.
                else if (userInput[0] == '2')
                {
                    fileContent = CheckUserExists(userInput);
                }
                //else give an error
                else
                {
                    Console.WriteLine("Invalid ID, please try again.\n");
                    fileContent = new string[0];
                }
            } while (fileContent.Length == 0);

            //when the user inputs a proper ID, print the doctor's file
            Console.WriteLine("ID       | Name             | Email Address         | Phone      | Address                                   ");
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
            PrintDoctorDetails(fileContent);
        }

        //method to check a specific doctor, using ID.
        public static void CheckPatientDetails()
        {
            string userInput;
            string[] fileContent;

            MenuHeader("Patient Details");
            Console.WriteLine("Please enter the ID of the patient who's details you are checking.");
            Console.WriteLine("Enter 'Exit' to return to the menu.\n");

            //Repeat until the user exits, or the user enters a valid patients's ID
            do
            {
                Console.Write("ID: ");
                userInput = Console.ReadLine();

                //if the user entered 'exit', end the method
                if (userInput == "Exit" || userInput == "exit")
                {
                    return;
                }
                //if the inputted ID starts with a 3, the inputted ID may be a patient. Check if they exist.
                else if (userInput[0] == '3')
                {
                    fileContent = CheckUserExists(userInput);
                }
                //else give an error
                else
                {
                    Console.WriteLine("Invalid ID, please try again.\n");
                    fileContent = new string[0];
                }
            } while (fileContent.Length == 0);

            //when the user inputs a proper ID, print the patient's file
            Console.WriteLine("ID       | Name             | Doctor             | Email Address         | Phone      | Address                                   ");
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
            PrintPatientDetails(fileContent);
        }

        //Print out a doctor's details. Accepts the user file, not the userID
        public static void PrintDoctorDetails(string[] fileContent)
        {
            //Print out the ID, full name, email, phone, and address
            Console.WriteLine("{0}      |{1}       | {2}       | {3} | {4}", fileContent[0], fileContent[2], fileContent[3], fileContent[4], fileContent[5]);
        }

        //Print out a doctor's details. Accepts the user file, not the userID
        public static void PrintPatientDetails(string[] fileContent)
        {
            Console.WriteLine("{0}      |{1}       |{2}       | {3}       | {4} | {5}", fileContent[0], fileContent[2], fileContent[3], fileContent[4], fileContent[5], fileContent[6]);
        }
    }
}

