namespace HospitalManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int userId;

            while (true)
            {
                Utils.MenuHeader("Login");
                userId = Utils.Login();

                //after a successful login, create the user and open their menu
                //if userId starts with a 0, they are an admin. Create an admin object
                if (userId == 0)
                {
                    Admin currentUser = new Admin();
                    currentUser.UserMenu();
                }
                //else if userId starts with a 1, they are a doctor
                else if (userId ==1)
                {
                    Doctor currentUser = new Doctor();
                    currentUser.UserMenu();
                }
                //else their id must start with a 2 or higher, they are a patient
                else
                {
                    Patient currentUser = new Patient();
                    currentUser.UserMenu();
                }
            }
            
        }
    }
}