using Azure.Core.GeoJson;
using ContactManagement.Cosmos;
using ContactManagement.Model;
using ContactManagement.Util;
using Microsoft.Azure.Cosmos;
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
        private static CosmosClient cosmosClient = null;

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
                    case "generate_json_files":
                        await GenerateJsonFiles();
                        break;
                    case "generate_csv_files":
                        await GenerateCsvFiles();
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
            finally
            {
                if (cosmosClient != null)
                {
                    cosmosClient.Dispose();
                }
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
            Console.WriteLine("dotnet run generate_json_files");
            Console.WriteLine("dotnet run generate_csv_files");
            Console.WriteLine("");
        }

        private static async Task GenerateJsonFiles()
        {
            DataGenerator.generateJsonFiles();
            await Task.CompletedTask;
        }

        private static async Task GenerateCsvFiles()
        {
            DataGenerator.generateCsvFiles();
            await Task.CompletedTask;
        }

        private static List<BaseDocument> ReadJsonDocumentsFile(string infile, string doctype)
        {
            List<BaseDocument> baseDocs = new List<BaseDocument>();
            switch (doctype)
            {
                case "company":
                    List<Company> companies = FileUtil.readCompanyDocumentsList(infile);
                    for (int i = 0; i < companies.Count; i++)
                    {
                        baseDocs.Add((BaseDocument)companies[i]);
                    }

                    break;
                case "contact":
                    List<Contact> contacts = FileUtil.readContactDocumentsList(infile);
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        baseDocs.Add((BaseDocument)contacts[i]);
                    }

                    break;
                case "contact_method":
                    List<ContactMethod> contactMethods = FileUtil.readContactMethodsDocumentsList(infile);
                    for (int i = 0; i < contactMethods.Count; i++)
                    {
                        baseDocs.Add((BaseDocument)contactMethods[i]);
                    }

                    break;
            }
            return baseDocs;
        }
    }
}