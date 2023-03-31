namespace ContactManagement.Model
{
    /**
     * This model class represents a Contact - a person in a Company (or tenant).
     * See the separate, but related, ContactMethod class.
     *
     * Chris Joakim, Microsoft, 2023
     */
    
    public class Contact : BaseDocument
    {
        public string contactId { get; set; }   // enables easy joins to ContactMethod documents
        public string name { get; set; }
        public string preferredContactMethod { get; set; }
        public string uniqueKey { get; set; }
        public List<string> roles { get; set; }
        public List<string> notificationPreferences { get; set; }

        // ContactMethods are separate documents in the same container

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
            uniqueKey = $"{pk}|{name}".Trim().ToLower();
            return uniqueKey;
        }
    }
}