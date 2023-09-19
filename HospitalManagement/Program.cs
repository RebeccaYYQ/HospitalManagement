namespace HospitalManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int userId;
            
            Utils.MenuHeader("Login");
            userId = Utils.Login();

            //after a successful login, create the user
            Admin currentUser = new Admin();

            currentUser.UserMenu();
            //Maybe Login returns a user, and you login with that user.
            //user.LoadMenu();
            Console.ReadKey();
        }
    }
}