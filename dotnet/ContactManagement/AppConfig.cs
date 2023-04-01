using Newtonsoft.Json;

namespace ContactManagement
{
    /**
     * This class is the source of all configuration values for this application -  
     * including environment variables and command-line arguments.
     *
     * Chris Joakim, Microsoft, 2023
     */
    public class AppConfig
    {
        // Constants; environment variable names:
        public const string AZURE_COSMOSDB_NOSQL_CONN_STRING1 = "AZURE_COSMOSDB_NOSQL_CONN_STRING1";
        public const string AZURE_COSMOSDB_NOSQL_RW_KEY1      = "AZURE_COSMOSDB_NOSQL_RW_KEY1";
        public const string AZURE_COSMOSDB_NOSQL_URI          = "AZURE_COSMOSDB_NOSQL_URI";
        public const string AZURE_COSMOSDB_NOSQL_DB           = "AZURE_COSMOSDB_NOSQL_DB";
        public const string AZURE_COSMOSDB_NOSQL_PREF_REGIONS = "AZURE_COSMOSDB_NOSQL_PREF_REGIONS";

        public const int AZURE_COSMOSDB_DEFAULT_BULK_BATCH_SIZE = 1000;

        public const string FORMAT_CSV  = "csv";
        public const string FORMAT_JSON = "json";

        // Constants; command-line and keywords:
        public const string VERBOSE_FLAG = "--verbose";

        // Class variables:
        private static AppConfig singleton;

        // Instance variables:
        private string[] cliArgs = { };

        public static AppConfig Singleton(string[] args)
        {
            // called by Program.cs Main()
            if (singleton == null)
            {
                singleton = new AppConfig(args);
            }
            return singleton;
        }

        public static AppConfig Singleton()
        {
            return singleton;
        }

        private AppConfig(string[] args)
        {
            cliArgs = args; // dotnet run xxx yyy -> args:["xxx","yyy"]
        }

        public bool IsValid()
        {
            Console.WriteLine("Config#IsValid args: " + JsonConvert.SerializeObject(cliArgs));
            if (cliArgs.Length < 2)
            {
                Console.WriteLine("ERROR: empty command-line args");
                return false;
            }
            return true;
        }

        public string[] GetCliArgs()
        {
            return cliArgs;
        }

        public string GetCosmosConnString()
        {
            return GetEnvVar(AZURE_COSMOSDB_NOSQL_CONN_STRING1, null);
        }

        public string GetCosmosUri()
        {
            return GetEnvVar(AZURE_COSMOSDB_NOSQL_URI, null);
        }

        public string GetCosmosKey()
        {
            return GetEnvVar(AZURE_COSMOSDB_NOSQL_RW_KEY1, null);
        }

        public string[] GetCosmosPreferredRegions()
        {
            string delimList = GetEnvVar(AZURE_COSMOSDB_NOSQL_PREF_REGIONS, null);
            if (delimList == null)
            {
                return new string[] { };
            }
            else
            {
                return delimList.Split(',');
            }
        }

        public string GetEnvVar(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

        public string GetEnvVar(string name, string defaultValue = null)
        {
            string value = Environment.GetEnvironmentVariable(name);
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        public string GetCliKeywordArg(string keyword, string defaultValue = null)
        {
            try
            {
                for (int i = 0; i < cliArgs.Length; i++)
                {
                    if (keyword == cliArgs[i])
                    {
                        return cliArgs[i + 1];
                    }
                }

                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public bool HasCliFlagArg(string flag)
        {
            for (int i = 0; i < cliArgs.Length; i++)
            {
                if (cliArgs[i].Equals(flag))
                {
                    return true;
                }
            }

            return false;
        }

        public int BulkBatchSize()
        {
            return AZURE_COSMOSDB_DEFAULT_BULK_BATCH_SIZE;
        }

        public bool IsVerbose()
        {
            for (int i = 0; i < cliArgs.Length; i++)
            {
                if (cliArgs[i] == VERBOSE_FLAG)
                {
                    return true;
                }
            }
            return false;
        }

        public void Display()
        {
            Console.WriteLine($"Config, args: {JsonConvert.SerializeObject(GetCliArgs())}");
        }
    }
}