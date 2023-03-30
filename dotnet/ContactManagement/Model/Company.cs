namespace ContactManagement.Model
{
    /**
     * This model class represents a Company, or tenant, in the application.
     * It may be a small business or a corporation.
     *
     * Chris Joakim, Microsoft, 2023
     */
    
    // CREATE TABLE cm.tenants (
    //     id UUID NOT NULL,
    //     created_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    //     modified_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    //     created_by VARCHAR(50) NOT NULL,
    //     last_modified_by VARCHAR(50) NOT NULL,
    //     name VARCHAR(50) NOT NULL,
    //     expiration_date date NULL,
    //     is_deleted boolean NOT NULL DEFAULT false,
    //     PRIMARY KEY(id)
    // );
    
    public class Company : BaseDocument
    {
        // Inherited: id, pk, doctype, companyId
        public string name { get; set; }
        public string orgType { get; set; }
        public string hqState { get; set; }

        public string memo { get; set; }
        
        public bool jumbo { get; set; }
        
        public Company()
        {
            this.doctype = AppConstants.DOCTYPE_COMPANY;
            this.jumbo = false;
        }

        public bool isSmallBusiness()
        {
            return AppConstants.ORG_TYPE_SMALL_BUSINESS.Equals(orgType);
        }

        public bool isCorporation()
        {
            return AppConstants.ORG_TYPE_CORPORATION.Equals(orgType);
        }
    }
}