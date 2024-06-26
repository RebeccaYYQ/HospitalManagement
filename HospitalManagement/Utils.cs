﻿using System.Net;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace HospitalManagement
{
    internal class Utils
    {
        //The main header seen on all screens
        public static void MenuHeader(string subheaderText)
        {
            //calculate spacing so that the subheader is centered
            int headerWidth = 44;
            int leftPadding = (headerWidth - subheaderText.Length) / 2;
            int remainder = headerWidth - leftPadding;

            string formatting = "{0," + leftPadding + "}{1,-" + remainder + "}";
            string subheading = string.Format(formatting, "", subheaderText);

            Console.Clear();
            Console.WriteLine("┌────────────────────────────────────────────┐");
            Console.WriteLine("│     DOTNET Hospital Management System      │");
            Console.WriteLine("├────────────────────────────────────────────┤");
            Console.WriteLine("|" + subheading + "|");
            Console.WriteLine("└────────────────────────────────────────────┘");
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
            string[] fileContent = CheckUserExists(userId, "any");

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

        //To check if a user exists, and matches a wanted type. If there is, return the user's file contents. If not, retun an empty string[]
        //userType is to check for a specific user type, eg doctor or patient or any
        public static string[] CheckUserExists(string userId, string userType)
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

            //based on userType, check if that ID starts with a 2 (doctor) or 3 (patient). If not, exit without reading the file
            if (userType == "doctor" && userId[0] != '2')
            {
                Console.WriteLine("Invalid ID, please try again.\n");
                return fileContent;
            }
            if (userType == "patient" && userId[0] != '3')
            {
                Console.WriteLine("Invalid ID, please try again.\n");
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
            string[] lines, files, fileContent;

            //all doctors have an ID starting with 2, so find files in the debug/net6.0 directory starting with 2.
            files = Directory.GetFiles(@".", "2*.txt");

            PrintDoctorHeader();

            //print out the contents of that file
            foreach (string filePath in files)
            {
                lines = File.ReadAllLines(filePath);
                fileContent = lines[0].Split(',');
                PrintDoctorDetails(fileContent);
            }
        }

        //List patients, either all, or only specific ones based on doctorID
        public static void ListPatients(string searchCriteria)
        {
            string[] lines, files, fileContent;

            //all patients have an ID starting with 3, so find files in the debug/net6.0 directory starting with 3.
            files = Directory.GetFiles(@".", "3*.txt");

            PrintPatientHeader();

            //print out the contents of patient files, based on searchCriteria. If it doesn't match the 2 If statements, don't print that patient
            foreach (string filePath in files)
            {
                lines = File.ReadAllLines(filePath);
                fileContent = lines[0].Split(',');
                //If the user wants all patients (admin)
                if (searchCriteria == "all")
                {
                    PrintPatientDetails(fileContent);
                }
                //check if that patient has the same doctorID has the searchCriteria
                else if (fileContent[3] == searchCriteria)
                {
                    PrintPatientDetails(fileContent);

                }
            }
        }

        //Prints out appointments based on a criteria.It filters based on doctorId, patientId, or both if both arguments have values
        public static void ListAppointments(string doctorId, string patientId)
        {
            PrintAppointmentHeader();

            string[] allAppts = File.ReadAllLines("AllAppointments.txt");
            string[] apptArray;

            //for every item in the file, except the last line (because the last line is empty)
            for (int i = 0; i < allAppts.Length; i++)
            {
                apptArray = allAppts[i].Split(",");

                //if there is a doctorId argument and no patient arg, filter all appointments based on doctor
                if (apptArray[0] == doctorId && patientId == "")
                {
                    PrintAppointmentDetails(apptArray);
                }
                //else if both args have a value, and match the row
                else if (apptArray[0] == doctorId && apptArray[1] == patientId)
                {
                    PrintAppointmentDetails(apptArray);
                }
                //if there is a patientId argument, filter appts based on it
                else if (apptArray[1] == patientId && doctorId == "")
                {
                    PrintAppointmentDetails(apptArray);
                }
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
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                fileContent = CheckUserExists(userInput, "doctor");

            } while (fileContent.Length == 0);

            //when the user inputs a proper ID, print the doctor's file
            PrintDoctorHeader();
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
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                fileContent = CheckUserExists(userInput, "patient");

            } while (fileContent.Length == 0);

            //when the user inputs a proper ID, print the patient's file
            PrintPatientHeader();
            PrintPatientDetails(fileContent);
        }

        //The header for all doctor information screens
        public static void PrintDoctorHeader()
        {
            Console.WriteLine(String.Format("{0,-6} | {1,-20} | {2, -25} | {3,-20} | {4, -30}", "ID", "Name", "Email Address", "Phone", "Address"));
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
        }

        //Print out a doctor's details. Accepts the user file, not the userID
        public static void PrintDoctorDetails(string[] fileContent)
        {
            //Print out the ID, full name, email, phone, and address
            Console.WriteLine(String.Format("{0,-6} | {1,-20} | {2, -25} | {3,-20} | {4, -30}", fileContent[0], fileContent[2], fileContent[3], fileContent[4], fileContent[5]));
        }

        //The header for all patient information screens
        public static void PrintPatientHeader()
        {
            Console.WriteLine(String.Format("{0,-6} | {1,-20} | {2,-20} | {3, -25} | {4,-20} | {5, -30}", "ID", "Doctor", "Name", "Email Address", "Phone", "Address"));
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
        }

        //Print out a patient's details. Accepts the user file, not the userID
        public static void PrintPatientDetails(string[] fileContent)
        {
            string doctorName = "null";
            string[] doctorFile;

            //In the file the doctor is stored as an ID. Get the doctor's name to print
            if (fileContent[3] != "null")
            {
                doctorName = GetName(fileContent[3]);
            }
            //print out the ID, full name, doctor name, email, phone, and address
            Console.WriteLine(String.Format("{0,-6} | {1,-20} | {2,-20} | {3, -25} | {4,-20} | {5, -30}", fileContent[0], fileContent[2], doctorName, fileContent[4], fileContent[5], fileContent[6]));
        }

        //The header for all appointment screens
        public static void PrintAppointmentHeader()
        {
            Console.WriteLine(String.Format("{0,-20} | {1,-20} | {2,-20}", "Doctor", "Patient", "Description"));
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
        }

        //Prints out the appointment details. Accepts a stringArray
        public static void PrintAppointmentDetails(string[] fileContent)
        {
            string doctorName = GetName(fileContent[0]);
            string patientName = GetName(fileContent[1]);
            //Print out the doctor's name, patient name and appointment details
            Console.WriteLine(String.Format("{0,-20} | {1,-20} | {2,-20}", doctorName, patientName, fileContent[2]));
        }

        //Add a user. Create a file at the end
        public static void AddUser(string userType)
        {
            string userData;
            int userId;

            //Doctor Ids start with a 2, and patient Ids start with a 3. Set the benchmark here.
            if (userType == "doctor")
            {
                userId = 200001;
            }
            //else default to patient
            else
            {
                userId = 300001;
            }
            

            //loop through userIds until it finds a file (and hence ID) that doesn't exist. 
            while (File.Exists(string.Format("{0}.txt", userId))) {
                userId++;
            }

            //get all the data to create a user
            string userTypeCapitalised = char.ToUpper(userType[0]) + userType[1..];
            MenuHeader("Add " + userTypeCapitalised);
            Console.WriteLine("Register a new {0}.\n", userType);

            Console.WriteLine("The ID of the created user will be {0}\n", userId);

            Console.Write("Full name: ");
            userData = Console.ReadLine();

            //if they are a patient, add a null for the doctor field
            if (userType == "patient") {
                userData += ",null";
            }

            Console.Write("Email: ");
            userData = userData + "," + Console.ReadLine();
            Console.Write("Phone number: ");
            userData = userData + "," + Console.ReadLine();
            Console.Write("Address: ");
            userData = userData + "," + Console.ReadLine();
            Console.Write("Password: ");
            userData = Console.ReadLine() + ',' + userData;

            //Add ID to the start of the string
            userData = Convert.ToString(userId) + ',' + userData;

            //Create the file
            string filePath = userId + ".txt";
            File.WriteAllText(filePath, userData);

            Console.WriteLine();
            Console.WriteLine("{0} created.", userTypeCapitalised);
        }

        //Update the appointment file with a new appointment
        public static void AddAppointment(string doctorId, int userId, string apptDescription)
        {
            string filePath = "AllAppointments.txt";
            File.AppendAllText(filePath, Convert.ToString(doctorId + ","+userId+","+apptDescription+"\n"));
        }

        //Returns the name of the user by reading the file and retieving their name
        public static string GetName(string userId)
        {
            string[] userFile = ReadFile(userId);
            return userFile[2];
        }
    }
}

