// Chris Joakim, Microsoft

using ContactManagement.Model;
using Microsoft.Azure.Cosmos;

namespace ContactManagement.Cosmos
{
    /**
     * This is a DAO (Data Access Object) class that implements CRUD and Query operations.
     *
     * Links:
     * - https://github.com/Azure/azure-cosmos-dotnet-v3/blob/master/Microsoft.Azure.Cosmos.Samples/Usage/ItemManagement/Program.cs
     * 
     * Chris Joakim, Microsoft, 2023
     */
    public class CosmosDAO : CosmosBaseUtil
    {
        public CosmosDAO(CosmosClient client, bool verbose = false)
        {
            this.client = client;
            this.verbose = verbose;
        }

        public async Task<QueryResponse> CountDocuments(string predicate, bool verbose = false)
        {
            QueryResponse respObj = new QueryResponse();
            string sql = null;

            try
            {
                sql = $"SELECT COUNT(1) FROM c {predicate.Trim()}";
                respObj.sql = sql;
                if (verbose)
                {
                    Console.WriteLine($"countDocuments - sql: {sql}");
                }

                QueryDefinition queryDefinition = new QueryDefinition(sql);
                QueryRequestOptions requestOptions = new QueryRequestOptions();
                FeedIterator<dynamic> queryResultSetIterator =
                    this.currentContainer.GetItemQueryIterator<dynamic>(
                        queryDefinition, requestOptions: requestOptions);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<dynamic> feedResponse =
                        await queryResultSetIterator.ReadNextAsync();
                    respObj.statusCode = feedResponse.StatusCode;
                    respObj.elapsedMs = feedResponse.Diagnostics.GetClientElapsedTime().TotalMilliseconds;
                    foreach (var item in feedResponse)
                    {
                        // Item is an instance of Newtonsoft.Json.Linq.JObject
                        string value = item.Property("$1").First.ToString();
                        respObj.AddItem(int.Parse(value));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"CountDocuments {sql} -> Exception {e}");
            }

            return respObj;
        }

        public async Task<QueryResponse> ExecuteQuery(string sql, bool verbose = false)
        {
            QueryResponse respObj = new QueryResponse();
            try
            {
                if (verbose)
                {
                    Console.WriteLine($"ExecuteQuery - sql: {sql}");
                }

                QueryDefinition qDef = new QueryDefinition(sql);
                QueryRequestOptions qOpts = new QueryRequestOptions();
                FeedIterator<dynamic> qIter =
                    this.currentContainer.GetItemQueryIterator<dynamic>(
                        qDef, requestOptions: qOpts);

                while (qIter.HasMoreResults)
                {
                    FeedResponse<dynamic> feedResponse = await qIter.ReadNextAsync();
                    respObj.statusCode = feedResponse.StatusCode;
                    respObj.elapsedMs = feedResponse.Diagnostics.GetClientElapsedTime().TotalMilliseconds;
                    respObj.IncrementRequestCharge(feedResponse.RequestCharge);
                    foreach (var item in feedResponse)
                    {
                        respObj.AddItem(item);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"CountDocuments {sql} -> Exception {e}");
            }

            return respObj;
        }
        
        public async Task<ItemResponse<Company>> CreateCompany(Company doc)
        {
            ItemResponse<Company> response = await this.currentContainer.CreateItemAsync(
                doc, new PartitionKey(doc.pk));
            return response;
        }
        
        public async Task<ItemResponse<Company>> ReadCompany(string id, string pk)
        {
            ItemResponse<Company> response = await this.currentContainer.ReadItemAsync<Company>(
                partitionKey: new PartitionKey(pk), id: id);
            return response;
        }
        
        public async Task<ItemResponse<Company>> UpdateCompany(Company doc)
        {
            ItemResponse<Company> response = await this.currentContainer.UpsertItemAsync(
                doc, new PartitionKey(doc.pk));
            return response;
        }
        
        /**
         * This method can apply to the subclasses of BaseDocument - Contact, ContactMethod, Company.
         */
        public async Task<ItemResponse<BaseDocument>> DeleteDocument(BaseDocument doc)
        {
            ItemResponse<BaseDocument> response =
                await this.currentContainer.DeleteItemAsync<BaseDocument>(
                    doc.id, new PartitionKey(doc.pk));
            return response;
        }
    }
}