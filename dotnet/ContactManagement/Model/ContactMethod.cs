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
        public string contactId { get; set; }
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
            this.contactId = contact.id;
            this.doctype = AppConstants.DOCTYPE_CONTACT_METHOD;
        }

        public string calculateUniqueKey()
        {
            uniqueKey = $"{pk}|{contactId}|{type}|{value}".Trim().ToLower();
            return uniqueKey;
        }
    }
}