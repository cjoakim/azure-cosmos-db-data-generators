namespace ContactManagement.Model;

public class ApplicationConfig
{
    /**
     * This class defines application configuration values, including UI config.
     *
     * Chris Joakim, Microsoft, 2023
     */
    
    public List<string> definedRoles = new List<string>();
    public List<string> definedContactMethods = new List<string>();
    public List<string> definedNotificationPreferences = new List<string>();

    public ApplicationConfig()
    {
        definedRoles.Add("Accounting");
        definedRoles.Add("Marketing");
        definedRoles.Add("Procurement");
        definedRoles.Add("Shipping");
        definedRoles.Add("Warehouse");
        definedRoles.Add("Owner");
        definedRoles.Add("Management");

        definedContactMethods.Add(AppConstants.CONTACT_METHOD_EMAIL);
        definedContactMethods.Add(AppConstants.CONTACT_METHOD_PHONE);
        definedContactMethods.Add(AppConstants.CONTACT_METHOD_FAX);
        definedContactMethods.Add(AppConstants.CONTACT_METHOD_MAIL);

        definedNotificationPreferences.Add(AppConstants.NOTIFICATION_PREF_MARKETING_UPDATES);
        definedNotificationPreferences.Add(AppConstants.NOTIFICATION_PREF_INVOICES);
        definedNotificationPreferences.Add(AppConstants.NOTIFICATION_PREF_JOB_REMINDERS);
    }

    public Dictionary<string, List<string>> uiConfig()
    {
        Dictionary<string, List<string>> d = new Dictionary<string, List<string>>();
        d.Add("roles", definedRoles);
        d.Add("contactMethods", definedContactMethods);
        d.Add("notificationPreferences", definedNotificationPreferences);
        return d;
    }
}