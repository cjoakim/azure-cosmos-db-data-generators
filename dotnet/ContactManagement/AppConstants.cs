namespace ContactManagement
{
    /**
    * This interface defines constant values used in the application.
    *
    * Chris Joakim, Microsoft, 2023
    */
    public interface AppConstants
    {
        public const string DOCTYPE_COMPANY = "company";
        public const string DOCTYPE_CONTACT = "contact";
        public const string DOCTYPE_CONTACT_METHOD = "contact_method";

        public const string ORG_TYPE_SMALL_BUSINESS = "small_business";
        public const string ORG_TYPE_CORPORATION = "corporation";

        public const string CONTACT_METHOD_EMAIL = "email";
        public const string CONTACT_METHOD_PHONE = "phone";
        public const string CONTACT_METHOD_FAX = "fax";
        public const string CONTACT_METHOD_MAIL = "mail";

        public const string NOTIFICATION_PREF_MARKETING_UPDATES = "MarketingUpdates";
        public const string NOTIFICATION_PREF_INVOICES = "Invoices";
        public const string NOTIFICATION_PREF_JOB_REMINDERS = "JobReminders";

        public const long KB = 1024;
        public const long MB = KB * KB;
        public const long GB = KB * KB * KB;
        public const long TB = KB * KB * KB * KB;
    }
}