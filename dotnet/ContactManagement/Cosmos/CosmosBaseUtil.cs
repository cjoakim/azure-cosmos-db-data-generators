using Microsoft.Azure.Cosmos;

namespace ContactManagement.Cosmos
{
    /**
     * This is the abstract superclass of the several Cosmos DB Util classes in this namespace.
     *
     * Chris Joakim, Microsoft, 2023
     */
    public abstract class CosmosBaseUtil
    {
        protected CosmosClient client = null;
        protected bool verbose = false;
        protected Database currentDatabase = null;
        protected Container currentContainer = null;

        protected CosmosBaseUtil()
        {
        }

        public async Task<Database> SetCurrentDatabase(string name)
        {
            try
            {
                this.currentDatabase = client.GetDatabase(name);
                return await currentDatabase.ReadAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"SetCurrentDatabase {name} -> Exception {e}");
                return null;
            }
        }

        public async Task<Container> SetCurrentContainer(string name)
        {
            try
            {
                this.currentContainer = this.currentDatabase.GetContainer(name);
                return await currentContainer.ReadContainerAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"SetCurrentContainer {name} -> Exception {e}");
                return null;
            }
        }
    }
}