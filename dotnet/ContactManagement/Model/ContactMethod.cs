namespace ContactManagement.Model
{
    /**
     * This model class represents one way that a Contact can be communicated with,
     * such as phone, fax, mail, etc.  See the related Contact class.
     *
     * Chris Joakim, Microsoft, 2023
     */
    
    public class ContactMethod : BaseDocument
    {
        public string contact_id { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string uniqueKey { get; set; }
        public string memo { get; set; }
        
        public ContactMethod()
        {
            this.doctype = AppConstants.DOCTYPE_CONTACT_METHOD;
        }

        public ContactMethod(Contact contact)
        {
            this.id = IdSequence.Next();
            this.pk = contact.pk;
            this.contact_id = contact_id;
            this.doctype = AppConstants.DOCTYPE_CONTACT_METHOD;
        }

        public string calculateUniqueKey()
        {
            uniqueKey = $"{pk}|{contact_id}|{type}|{value}".Trim().ToLower();
            return uniqueKey;
        }
    }
}