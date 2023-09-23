using System.Net;

namespace HospitalManagement
{
    public abstract class User
    {
        public int userId;
        public string fullName, email, phoneNo, address;

        public User(int userId, string fullName, string email, string phoneNo, string address)
        {
            this.userId = userId;
            this.fullName = fullName;
            this.email = email;
            this.phoneNo = phoneNo;
            this.address = address;
        }
    }

    public interface IMenu
    {
        //The main user menu should go in this method
        public void UserMenu();
        //The content for the menu should go in this method
        public Boolean LoadMenu();
    }

    class Admin : User, IMenu
    {
        //Constructor
        public Admin(int userId, string fullName, string email, string phoneNo, string address) : base(userId, fullName, email, phoneNo, address)
        {
            this.userId = userId;
            this.fullName = fullName;
            this.email = email;
            this.phoneNo = phoneNo;
            this.address = address;
        }

        //the main user menu. Repeat this until the user logs out or exits the system
        public void UserMenu()
        {
            Boolean logOut = false;
            while (!logOut)
            {
                logOut = LoadMenu();
            }
        }

        //The user menu, with all the options. Run a function for each option, unless the user logs out, exits, or enters invalid input.
        public Boolean LoadMenu()
        {
            Utils.MenuHeader("Administrator Menu");
            Console.WriteLine("Welcome to the DOTNET Hospital Management System, {0}", fullName);
            Console.WriteLine();
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1: List all doctors");
            Console.WriteLine("2: Check doctor details");
            Console.WriteLine("3: List all patients");
            Console.WriteLine("4: Check patient details");
            Console.WriteLine("5: Add doctor");
            Console.WriteLine("6: Add patient");
            Console.WriteLine("7: Exit to login");
            Console.WriteLine("8: Exit system\n");
            Console.Write("Selection: ");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    Utils.ListAllDoctors();
                    Console.ReadKey();
                    break;
                case "2":
                    Utils.CheckDoctorDetails();
                    Console.ReadKey();
                    break;
                case "3":
                    Utils.MenuHeader("All Patients");
                    Console.WriteLine("All patients registered in the DOTNET Hospital System");
                    Console.WriteLine();
                    Utils.ListPatients("all");
                    Console.ReadKey();
                    break;
                case "4":
                    Utils.CheckPatientDetails();
                    Console.ReadKey();
                    break;
                case "5":
                    Console.ReadKey();
                    break;
                case "6":
                    Console.ReadKey();
                    break;
                case "7":
                    Console.WriteLine("Logging out...");
                    Console.ReadKey();
                    return true;
                case "8":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please enter a valid selection.");
                    Console.ReadKey();
                    break;
            }
            return false;
        }

    }

    class Doctor : User, IMenu
    {
        //Constructor
        public Doctor(int userId, string fullName, string email, string phoneNo, string address) : base(userId, fullName, email, phoneNo, address)
        {
            this.userId = userId;
            this.fullName = fullName;
            this.email = email;
            this.phoneNo = phoneNo;
            this.address = address;
        }

        //the main user menu. Repeat this until the user logs out or exits the system
        public void UserMenu()
        {
            Boolean logOut = false;
            while (!logOut)
            {
                logOut = LoadMenu();
            }
        }

        //The user menu, with all the options. Run a function for each option, unless the user logs out, exits, or enters invalid input.
        public Boolean LoadMenu()
        {
            Utils.MenuHeader("Doctor Menu");
            Console.WriteLine("Welcome to the DOTNET Hospital Management System, {0}", fullName);
            Console.WriteLine();
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1: List doctor details");
            Console.WriteLine("2: List patients");
            Console.WriteLine("3: List appointments");
            Console.WriteLine("4: Check particular patient");
            Console.WriteLine("5: List appointments with patient");
            Console.WriteLine("6: Exit to login");
            Console.WriteLine("7: Exit system\n");
            Console.Write("Selection: ");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    Utils.MenuHeader("My Details");
                    string[] fileContent = Utils.ReadFile(Convert.ToString(userId));
                    Utils.PrintDoctorHeader();
                    Utils.PrintDoctorDetails(fileContent);
                    Console.ReadKey();
                    break;
                case "2":
                    Utils.MenuHeader("My Patients");
                    Utils.ListPatients(Convert.ToString(userId));
                    Console.ReadKey();
                    break;
                case "3":
                    Console.ReadKey();
                    break;
                case "4":
                    Utils.CheckPatientDetails();
                    Console.ReadKey();
                    break;
                case "5":
                    Console.ReadKey();
                    break;
                case "6":
                    Console.WriteLine("Logging out...");
                    Console.ReadKey();
                    return true;
                case "7":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please enter a valid selection.");
                    Console.ReadKey();
                    break;
            }
            return false;
        }
    }

    class Patient : User, IMenu
    {
        string doctor;

        //Constructor
        public Patient(int userId, string fullName, string doctor, string email, string phoneNo, string address) : base(userId, fullName, email, phoneNo, address)
        {
            this.userId = userId;
            this.fullName = fullName;
            this.doctor = doctor;
            this.email = email;
            this.phoneNo = phoneNo;
            this.address = address;
        }
        //the main user menu. Repeat this until the user logs out or exits the system
        public void UserMenu()
        {
            Boolean logOut = false;
            while (!logOut)
            {
                logOut = LoadMenu();
            }
        }

        //The user menu, with all the options. Run a function for each option, unless the user logs out, exits, or enters invalid input.
        public Boolean LoadMenu()
        {
            Utils.MenuHeader("Patient Menu");
            Console.WriteLine("Welcome to the DOTNET Hospital Management System, {0}", fullName);
            Console.WriteLine();
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1: List patient details");
            Console.WriteLine("2: List my doctor details");
            Console.WriteLine("3: List all appointments");
            Console.WriteLine("4: Book appointment");
            Console.WriteLine("5: Exit to login");
            Console.WriteLine("6: Exit system\n");
            Console.Write("Selection: ");
            string userInput = Console.ReadLine();

            string[] fileContent;

            switch (userInput)
            {
                case "1":
                    Utils.MenuHeader("My Details");
                    fileContent = Utils.ReadFile(Convert.ToString(userId));
                    Utils.PrintPatientHeader();
                    Utils.PrintPatientDetails(fileContent);
                    Console.ReadKey();
                    break;
                
                //if the user doesn't have a doctor, put an information box.
                case "2":
                    Utils.MenuHeader("My Doctor");
                    if (doctor == "null")
                    {
                        Console.WriteLine("You do not have a doctor yet. Select one in the Book appointment menu.");
                    } else
                    {
                        Utils.PrintDoctorHeader();
                        fileContent = Utils.ReadFile(doctor);
                        Utils.PrintDoctorDetails(fileContent);
                    }
                    Console.ReadKey();
                    break;

                case "3":
                    Console.ReadKey();
                    break;
                case "4":
                    Console.ReadKey();
                    break;
                case "5":
                    Console.WriteLine("Logging out...");
                    Console.ReadKey();
                    return true;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please enter a valid selection.");
                    Console.ReadKey();
                    break;
            }
            return false;
        }
    }
}