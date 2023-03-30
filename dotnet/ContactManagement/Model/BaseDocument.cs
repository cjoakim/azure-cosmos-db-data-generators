namespace ContactManagement.Model
{
    /**
     * This is the superclass of the Model classes in this namespace.
     *
     * Chris Joakim, Microsoft, 2023
     */
    
    public class BaseDocument
    {
        public string id { get; set; }
        public string pk { get; set; }
        public string doctype { get; set; }
        public string companyId { get; set; }

        public string _etag { get; set; }
        
        public DateTime created_on { get; set; }   // PostgreSQL timestamp
        
        public DateTime modified_on { get; set; }  // PostgreSQL timestamp
        
        public long _ts { get; set; }
        public BaseDocument()
        {
        }
    } 
}
