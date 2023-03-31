namespace ContactManagement.Model
{
    /**
     * This model class represents a Contact - a person in a Company (or tenant).
     * See the separate, but related, ContactMethod class.
     *
     * Chris Joakim, Microsoft, 2023
     */
    
    public class Contact
    {
        // Original attributes
        public string id { get; set; }
        public string pk { get; set; }
        public string doctype { get; set; }
        public string company_id { get; set; }
        public string contact_id { get; set; }
        public string name { get; set; }
        public string preferred_contact_method { get; set; }
        public string unique_key { get; set; }
        public List<string> roles { get; set; }
        public List<string> notificationPreferences { get; set; }

        // Additional attributes
        
        public DateTime  created_on { get; set; }
        public string    created_by { get; set; }
        public DateTime  modified_on { get; set; }
        public string    modified_by { get; set; }
        public DateTime? expiration_date { get; set; }
        public bool      is_deleted { get; set; }

        public Contact()
        {
            this.doctype = AppConstants.DOCTYPE_CONTACT;
            this.roles = new List<string>();
            this.notificationPreferences = new List<string>();
        }

        public string asCsv()
        {
            List<string> values = new List<string>();
            values.Add(id);
            values.Add(IdFactory.lookup(company_id));
            values.Add(IdFactory.lookup(contact_id));
            values.Add(quoted(name));
            values.Add(quoted(preferred_contact_method));
            values.Add(quoted(string.Join(",", roles)));
            values.Add(quoted(string.Join(",", notificationPreferences)));
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
            return "id,company_id,contact_id,name,preferred_contact_method,roles,notification_references,created_on,created_by,modified_on,modified_by,expiration_date,is_deleted";
        }

        private string quoted(string s)
        {
            return s= "\"" + s+ "\"";
        }
        
        public void addRole(string role)
        {
            if (roles == null)
            {
                roles = new List<string>();
            }

            if (role != null)
            {
                roles.Add(role);
            }
        }

        public void addNotificationPreference(string np)
        {
            if (notificationPreferences == null)
            {
                notificationPreferences = new List<string>();
            }

            if (np != null)
            {
                notificationPreferences.Add(np);
            }
        }

        public string calculateUniqueKey()
        {
            // Assume, for now, that names are unique within a Company.
            // email address would be a better uniqueness component.
            unique_key = $"{pk}|{name}".Trim().ToLower();
            return unique_key;
        }
    }
}