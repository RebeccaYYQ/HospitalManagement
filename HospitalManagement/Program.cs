namespace HospitalManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int userId;

            //this loop enables logging in and logging out of different user accounts.
            while (true)
            {
                Utils.MenuHeader("Login");
                userId = Utils.Login();

                //load and run the user that matches the login credentials
                Utils.LoadAndRunUser(userId);
            }
            
        }
    }
}