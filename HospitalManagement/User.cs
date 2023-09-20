using System.Net;

namespace HospitalManagement
{
    public abstract class User
    {
        public int userId;
        public string password, fullName, email, phoneNo, address;

        public User(int userId, string password, string fullName, string email, string phoneNo, string address)
        {
            this.userId = userId;
            this.password = password;
            this.fullName = fullName;
            this.email = email;
            this.phoneNo = phoneNo;
            this.address = address;
        }
    }

    class Admin : User
    {
        //Constructor
        public Admin(int userId, string password, string fullName, string email, string phoneNo, string address) : base(userId, password, fullName, email, phoneNo, address)
        {
            this.userId = userId;
            this.password = password;
            this.fullName = fullName;
            this.email = email;
            this.phoneNo = phoneNo;
            this.address = address;
        }
        public void UserMenu()
        {
            Utils.MenuHeader("Administrator Menu");
            Console.WriteLine("Welcome to the DOTNET Hospital Management System, {0}", fullName);
            Console.WriteLine();
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("7: Log out");
            Console.WriteLine("8: Exit\n");
            Console.Write("Selection: ");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "7":
                    Console.WriteLine("Logging out");
                    Console.ReadLine();
                    break;
                case "8":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
        }
    }

    class Doctor : User
    {
        //Constructor
        public Doctor(int userId, string password, string fullName, string email, string phoneNo, string address) : base(userId, password, fullName, email, phoneNo, address)
        {
            this.userId = userId;
            this.password = password;
            this.fullName = fullName;
            this.email = email;
            this.phoneNo = phoneNo;
            this.address = address;
        }

        public void UserMenu()
        {
            Utils.MenuHeader("Doctor Menu");
            Console.WriteLine("Welcome to the DOTNET Hospital Management System, " + fullName);
            Console.ReadLine();
        }
    }

    class Patient : User
    {
        //Constructor
        public Patient(int userId, string password, string fullName, string email, string phoneNo, string address) : base(userId, password, fullName, email, phoneNo, address)
        {
            this.userId = userId;
            this.password = password;
            this.fullName = fullName;
            this.email = email;
            this.phoneNo = phoneNo;
            this.address = address;
        }
        public void UserMenu()
        {
            Utils.MenuHeader("Patient Menu");
            Console.WriteLine("Welcome to the DOTNET Hospital Management System, " + fullName);
            Console.ReadLine();
        }
    }
}