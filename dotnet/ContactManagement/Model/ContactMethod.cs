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

        public string asCsv()
        {
            List<string> values = new List<string>();
            values.Add(id);
            values.Add(IdFactory.lookup(company_id));
            values.Add(IdFactory.lookup(contact_id));
            values.Add(quoted(type));
            values.Add(quoted(value));
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
            return "id,company_id,contact_id,type,value,memo,created_on,created_by,modified_on,modified_by,expiration_date,is_deleted";
        }

        private string quoted(string s)
        {
            return s= "\"" + s+ "\"";
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