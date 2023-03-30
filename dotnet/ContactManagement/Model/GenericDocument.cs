// Instances of this class represent a generic and minimal document in CosmosDB,
// containing 'id' and 'pk' attributes.
//
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace ContactManagement.Model
{
    /**
     * This is a legacy class, not actively used in this POC.
     *
     * Chris Joakim, Microsoft, 2023
     */
    
    public class GenericDocument
    {
        public string id { get; set; }
        public string pk { get; set; }

        public GenericDocument()
        {
        }

        public GenericDocument(string id, string pk)
        {
            this.id = id;
            this.pk = pk;
        }

        public PartitionKey GetPartitionKey()
        {
            return new PartitionKey(pk);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}