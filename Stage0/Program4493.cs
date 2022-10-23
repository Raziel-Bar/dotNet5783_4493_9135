using System;
namespace Stage0

{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome4493();
            Console.ReadKey();
        }
        
        static partial void Welcome9135();
        private static void Welcome4493()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }

}