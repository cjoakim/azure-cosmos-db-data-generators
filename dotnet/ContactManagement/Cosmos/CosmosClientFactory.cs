using Microsoft.Azure.Cosmos;

namespace ContactManagement.Cosmos
{
    /**
     * This factory class is used to create and return CosmosClient objects.
     *
     * Chris Joakim, Microsoft, 2023
     */
    public class CosmosClientFactory
    {
        private CosmosClientFactory()
        {
            // do not use a constructor; use the static methods instead
        }

        public static CosmosClient RegularClient()
        {
            string uri = AppConfig.Singleton().GetCosmosUri();
            string key = AppConfig.Singleton().GetCosmosKey();
            Console.WriteLine($"uri: {uri}");
            //Console.WriteLine($"key: {key.Substring(0, 6)}...");

            IReadOnlyList<string> prefRegionsList = AppConfig.Singleton().GetCosmosPreferredRegions();

            CosmosClientOptions options = new CosmosClientOptions
            {
                ApplicationPreferredRegions = prefRegionsList
            };
            return new CosmosClient(uri, key, options);
        }

        public static CosmosClient BulkLoadingClient()
        {
            string uri = AppConfig.Singleton().GetCosmosUri();
            string key = AppConfig.Singleton().GetCosmosKey();
            IReadOnlyList<string> prefRegionsList = AppConfig.Singleton().GetCosmosPreferredRegions();

            Console.WriteLine($"uri: {uri}");
            //Console.WriteLine($"key: {key.Substring(0, 6)}...");

            CosmosClientOptions options = new CosmosClientOptions
            {
                ApplicationPreferredRegions = prefRegionsList,
                ApplicationName = "CosmosDbPlayground",
                AllowBulkExecution = true,
                ConnectionMode = ConnectionMode.Direct,
                MaxRetryAttemptsOnRateLimitedRequests = 12
            };
            return new CosmosClient(uri, key, options);
        }
    }
}