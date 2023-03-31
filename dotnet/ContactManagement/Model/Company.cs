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
        public DateTime? expiration_date { get; set; }
        public bool     is_deleted { get; set; }

        public Company()
        {
            this.doctype = AppConstants.DOCTYPE_COMPANY;
            this.jumbo = false;
        }
        public string asCsv()
        {
            List<string> values = new List<string>();
            values.Add(id);
            values.Add(IdFactory.lookup(company_id));
            values.Add(quoted(name));
            values.Add(quoted(org_type));
            values.Add(quoted(hq_state));
            values.Add(quoted(memo));
            values.Add("" + created_on);
            values.Add(created_by);
            values.Add("" + modified_on);
            values.Add(quoted(modified_by));
            values.Add("" + expiration_date);
            values.Add(("" + is_deleted).ToLower());
            return string.Join(",", values);
        }
        
        public string csvHeader()
        {
            return "id,company_id,name,org_type,hq_state,memo,created_on,created_by,modified_on,modified_by,expiration_date,is_deleted";
        }

        private string quoted(string s)
        {
            return s= "\"" + s+ "\"";
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