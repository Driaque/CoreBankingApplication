using Processor.PeerConnection;
using System;

namespace Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Nippy's FEP!");

            Startup();

            while (true)
            {
                Console.WriteLine("FEP started, nodes Listening.......");
                Console.WriteLine("Press 1 to Shutdown");
                Console.WriteLine("Press 2 to Restart");
                Console.WriteLine("Press 3 to Exit");

                string response = Console.ReadLine();

                switch (response)
                {
                    case "1":
                        Shutdown();
                        break;

                    case "2":
                        Shutdown();
                        Startup();
                        break;
                    case "3":
                        Shutdown();
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Wrong input");
                        Startup();
                        break;
                }
            }
        }

        public static void Startup()
        {
            new Listener().StartListener();

        }

        public static void Shutdown()
        {

        }
    }
}
