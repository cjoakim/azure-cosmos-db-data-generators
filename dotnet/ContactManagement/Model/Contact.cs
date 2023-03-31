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
        
        public DateTime created_on { get; set; }
        public string   created_by { get; set; }
        public DateTime modified_on { get; set; }
        public string   modified_by { get; set; }
        public DateTime expiration_date { get; set; }
        public bool     is_deleted { get; set; }

        public Contact()
        {
            this.doctype = AppConstants.DOCTYPE_CONTACT;
            this.roles = new List<string>();
            this.notificationPreferences = new List<string>();
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