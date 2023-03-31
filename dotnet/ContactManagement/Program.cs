using Azure.Core.GeoJson;
using ContactManagement.Model;
using ContactManagement.Util;
using Newtonsoft.Json;

namespace ContactManagement
{
    /**
     * This is the entry-point for the application, with a Main() method.
     *
     * Chris Joakim, Microsoft, 2023
     */
    class Program
    {
        private static string[]  cliArgs = null;
        private static string    cliFunction = null;
        private static AppConfig config = null;

        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                PrintCliExamples(null);
                await Task.Delay(0);
                return;
            }
            cliArgs = args;
            config = AppConfig.Singleton(cliArgs);
            cliFunction = args[0];

            try
            {
                switch (cliFunction)
                {
                    case "generate":
                        await Generate(args[1].ToLower());
                        break;
                    default:
                        PrintCliExamples($"invalid cliFunction: {cliFunction}");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Exception in Main() - ", e.Message);
                Console.WriteLine(e.StackTrace);
            }
            await Task.Delay(0);
        }

        private static void PrintCliExamples(string msg)
        {
            if (msg != null)
            {
                Console.WriteLine($"Error: {msg}");
            }
            Console.WriteLine("");
            Console.WriteLine("Command-Line Examples:");
            Console.WriteLine("dotnet run generate json");
            Console.WriteLine("dotnet run generate csv");
            Console.WriteLine("");
        }

        private static async Task Generate(string format)
        {
            DataGenerator.generate(format);
            await Task.CompletedTask;
        }
    }
}