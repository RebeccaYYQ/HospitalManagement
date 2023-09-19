namespace HospitalManagement
{
    internal class Utils
    {
        public static void MenuHeader(string subHeader)
        {
            Console.WriteLine("┌───────────────────────────────────────────┐");
            Console.WriteLine("│     DOTNET Hospital Management System     │");    
            Console.WriteLine("├───────────────────────────────────────────┤");
            Console.WriteLine("│                  {0}                      │", subHeader);
            Console.WriteLine("└───────────────────────────────────────────┘");
            Console.WriteLine();
        }
    }
}