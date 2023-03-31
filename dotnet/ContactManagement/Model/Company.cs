namespace ContactManagement.Model
{
    /**
     * This model class represents a Company, or tenant, in the application.
     * It may be a small business or a corporation.
     *
     * Chris Joakim, Microsoft, 2023
     */
    
    public class Company
    {
        // Original attributes
        public string id { get; set; }
        public string pk { get; set; }
        public string doctype { get; set; }
        public string company_id { get; set; }
        public string name { get; set; }
        public string org_type { get; set; }
        public string hq_state { get; set; }
        
        public string memo { get; set; }
        
        public bool jumbo { get; set; }
        
        // Additional attributes
        
        public DateTime created_on { get; set; }
        public string   created_by { get; set; }
        public DateTime modified_on { get; set; }
        public string   modified_by { get; set; }
        public DateTime expiration_date { get; set; }
        public bool     is_deleted { get; set; }

        public Company()
        {
            this.doctype = AppConstants.DOCTYPE_COMPANY;
            this.jumbo = false;
        }

        public bool isSmallBusiness()
        {
            return AppConstants.ORG_TYPE_SMALL_BUSINESS.Equals(org_type);
        }

        public bool isCorporation()
        {
            return AppConstants.ORG_TYPE_CORPORATION.Equals(org_type);
        }
    }
}