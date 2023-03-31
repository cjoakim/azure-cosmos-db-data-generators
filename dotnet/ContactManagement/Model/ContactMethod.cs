namespace ContactManagement.Model
{
    /**
     * This model class represents one way that a Contact can be communicated with,
     * such as phone, fax, mail, etc.  See the related Contact class.
     *
     * Chris Joakim, Microsoft, 2023
     */
    
    public class ContactMethod
    {
        // Original attributes
        public string id { get; set; }
        public string pk { get; set; }
        public string doctype { get; set; }
        public string company_id { get; set; }
        public string contact_id { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string unique_key { get; set; }
        public string memo { get; set; }
        
        // Additional attributes
        
        public DateTime created_on { get; set; }
        public string   created_by { get; set; }
        public DateTime modified_on { get; set; }
        public string   modified_by { get; set; }
        public DateTime expiration_date { get; set; }
        public bool     is_deleted { get; set; }
        
        public ContactMethod()
        {
            this.doctype = AppConstants.DOCTYPE_CONTACT_METHOD;
        }

        public ContactMethod(Contact contact)
        {
            this.contact_id = contact.id;
            this.doctype = AppConstants.DOCTYPE_CONTACT_METHOD;
        }

        public string calculateUniqueKey()
        {
            unique_key = $"{pk}|{contact_id}|{type}|{value}".Trim().ToLower();
            return unique_key;
        }
    }
}