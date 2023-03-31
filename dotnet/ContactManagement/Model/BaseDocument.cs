namespace ContactManagement.Model
{
    /**
     * This is the superclass of the Model classes in this namespace; these correspond
     * to different (schemaless) document types in the application.
     *
     * In this multi-tenant app, all documents contain these attributes: id, pk, doctype, companyId.
     * pk stands for "partition key", while doctype is the type of the document (company, contact, etc)
     *
     * Chris Joakim, Microsoft, 2023
     */
    
    public class BaseDocument
    {



        public BaseDocument()
        {
        }
    } 
}


