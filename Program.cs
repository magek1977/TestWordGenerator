using System;
using System.Threading;

namespace TestWordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating password and calculating hash");
            ThreadManager manager = new ThreadManager();
            manager.run();
        }
    }
}
