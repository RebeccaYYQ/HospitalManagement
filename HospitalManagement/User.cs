namespace HospitalManagement
{
    abstract class User
    {
        int userId;
        string fullName;
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
            Console.WriteLine("Welcome to the DOTNET Hospital Management System, " + fullName);
        }
    }

    class Doctor : User
    {
        string fullName = "doctor";
    }
}