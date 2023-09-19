namespace HospitalManagement
{
    abstract class User
    {
        int userId;
        string fullName;

        void exitApp()
        {
            Environment.Exit(0);
        }
    }

    class Admin : User
    {
        string fullName = "admin";

        //Constructor
        public Admin() 
        {
            
        }
        public void UserMenu()
        {
            Utils.MenuHeader("Administrator Menu");
            Console.WriteLine("Welcome to the DOTNET Hospital Management System, {0}", fullName);
            Console.WriteLine();
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("7: Log out \n8: Exit\n");

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
        string fullName = "doctor";

        public void UserMenu()
        {
            Utils.MenuHeader("Doctor Menu");
            Console.WriteLine("Welcome to the DOTNET Hospital Management System, " + fullName);
            Console.ReadLine();
        }
    }

    class Patient : User
    {
        string fullName = "patient";
        public void UserMenu()
        {
            Utils.MenuHeader("Patient Menu");
            Console.WriteLine("Welcome to the DOTNET Hospital Management System, " + fullName);
            Console.ReadLine();
        }
    }
}