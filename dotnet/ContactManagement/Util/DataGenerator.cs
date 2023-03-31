using ContactManagement.Model;
using Newtonsoft.Json;

namespace ContactManagement.Util
{
    /**
     * This class is used to generate the simulated data for the POC.
     * See Program.cs and the main method.
     * 
     * It writes *.json files to the data/ directory, and are then used in the
     * Cosmos DB bulk loading process.
     *
     * Chris Joakim, Microsoft, 2023
     */ 
    
    public class DataGenerator
    {
        // Parameters: Companies
        private static int smallBusinessCount = 100;  // 1000
        private static int corporationCount   =  10;  // 100
        private static int contactSeq = 0;
        private static int companySeq = 0;

        // Parameters: Contacts
        private static int maxContactsPerSmallBusiness = 3;
        private static int maxContactsPerCorporation   = 10;

        // Generated Data Objects
        private static Dictionary<string, Company> companyDict = new Dictionary<string, Company>();
        private static Dictionary<string, Contact> contactDict = new Dictionary<string, Contact>();

        // Unique keys of the Generated Data Objects
        private static Dictionary<string, string> uniqueCompanyDict = new Dictionary<string, string>();
        private static Dictionary<string, string> uniqueContactDict = new Dictionary<string, string>();

        public static void generate(string format)
        {
            generateCompanies(format);
            generateContacts(format);
            generateContactMethods(format);

            Console.WriteLine($"companyNameDict count: {uniqueCompanyDict.Count}");
            FileUtil.writeObjectAsJson(uniqueCompanyDict, "data/companyNameDict.json");
        }

        private static void generateCompanies(string format)
        {
            Console.WriteLine("generateCompanies...");
            List<Company> companyList = new List<Company>();

            // first create "small businesses" which will have fewer contacts
            for (int i = 0; i < smallBusinessCount; i++)
            {
                string name = newUniqueCompanyName(false);
                if (name != null)
                {
                    try
                    {
                        Company c = createCompany(false, i);
                        companyDict.Add(c.pk, c);
                        uniqueCompanyDict.Add(c.name, "");
                        companyList.Add(c);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            // next create "corporations" with many more contacts than small businesses
            for (int i = 0; i < corporationCount; i++)
            {
                var faker = new Bogus.Faker();
                string name = newUniqueCompanyName(true);
                if (name != null)
                {
                    Company c = createCompany(true, i + 10000);
                    try
                    {
                        companyDict.Add(c.pk, c);
                        if (i < 10)
                        {
                            // some of the corporations will be "jumbo" with many many contacts
                            c.jumbo = true;
                        }
                        uniqueCompanyDict.Add(c.name, "");
                        companyList.Add(c);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            switch (format)
            {
                case (AppConfig.FORMAT_CSV):
                    List<string> lines = new List<string>();
                    for (int i = 0; i < companyList.Count; i++)
                    {
                        Company c = companyList[i];
                        if (i == 0)
                        {
                            lines.Add(c.csvHeader());
                        }
                        lines.Add(c.asCsv());
                    }
                    Console.WriteLine($"companies count: {companyList.Count}");
                    FileUtil.writeLines(lines, "data/companies.csv");
                    break;

                default:
                    // default to JSON
                    Console.WriteLine($"companies count: {companyList.Count}");
                    FileUtil.writeObjectAsJson(companyList, "data/companies.json");
                    break;
            }

            Thread.Sleep(1000);
        }

        private static string newUniqueCompanyName(bool corporation)
        {
            bool continueToIterate = true;
            int iterations = 0;

            while (continueToIterate)
            {
                iterations++;
                string name = null;

                if (corporation)
                {
                    name = new Bogus.Faker().Person.LastName + ", " + new Bogus.Faker().Person.LastName + " & " +
                           capitalize(new Bogus.Faker().Commerce.Color()) + " Corp";
                }
                else
                {
                    name = new Bogus.Faker().Company.CompanyName();
                    if (!name.ToLower().Contains("llc"))
                    {
                        name = name + " LLC";
                    }
                }

                if (uniqueCompanyDict.ContainsKey(name))
                {
                    // try again, the name is already used
                    // Console.WriteLine($"newUniqueCompanyName duplicate: {name}, iter: {iterations}");
                }
                else
                {
                    return name;
                }

                if (iterations > 20)
                {
                    Console.WriteLine($"newUniqueCompanyName max iterations reached: {iterations}");
                    break; // we've run out of values in Bogus
                }
            }
            return null;
        }

        private static void generateContacts(string format)
        {
            Console.WriteLine("generateContacts...");
            List<Contact> contactList = new List<Contact>();

            foreach (var pk in companyDict.Keys)
            {
                Company company = companyDict[pk];
                int targetCount = randomContactCount(company);
                int createdCount = 0;
                string json = JsonConvert.SerializeObject(company);

                if (company.jumbo == true)
                {
                    targetCount = (targetCount + 5) * 1000; // generate lots of data for "jumbo" tenants
                    Console.WriteLine($"jumbo company {company.name} {targetCount}");
                }

                while (createdCount < targetCount)
                {
                    Contact contact = createContact(company);
                    string uniqueKey = contact.calculateUniqueKey();
                    if (uniqueContactDict.ContainsKey(uniqueKey))
                    {
                        // try again
                    }
                    else
                    {
                        createdCount++;
                        contactDict.Add(uniqueKey, contact);
                        contactList.Add(contact);
                        json = JsonConvert.SerializeObject(contact);
                        //Console.WriteLine($"contact: {json}");
                    }
                }
            }
            switch (format)
            {
                case (AppConfig.FORMAT_CSV):
                    List<string> lines = new List<string>();
                    for (int i = 0; i < contactList.Count; i++)
                    {
                        Contact c = contactList[i];
                        if (i == 0)
                        {
                            lines.Add(c.csvHeader());
                        }
                        lines.Add(c.asCsv());
                    }
                    Console.WriteLine($"contacts count: {contactList.Count}");
                    FileUtil.writeLines(lines, "data/contacts.csv");
                    break;

                default:
                    // default to JSON
                    Console.WriteLine($"contacts count: {contactList.Count}");
                    FileUtil.writeObjectAsJson(contactList, "data/contacts.json");
                    break;
            }
            Thread.Sleep(1000);
        }

        public static Company createCompany(bool isCorp, int seq)
        {
            companySeq++;
            var faker = new Bogus.Faker();
            Company c = new Company();
            c.id = IdFactory.NextUuid();
            c.company_id = IdFactory.NextUuid();
            c.pk = c.company_id;
            c.name = newUniqueCompanyName(isCorp);
            if (isCorp)
            {
                c.org_type = AppConstants.ORG_TYPE_CORPORATION;  
            }
            else
            {
                c.org_type = AppConstants.ORG_TYPE_SMALL_BUSINESS;
            }
            c.hq_state = faker.Address.StateAbbr();
            c.created_on = DateTime.Now;
            c.created_by = "migration";
            c.modified_on = DateTime.Now;
            c.modified_by = "migration";
            c.expiration_date = null;
            c.is_deleted = false;
            return c;
        }
        
        public static Contact createContact(Company company)
        {
            contactSeq++;
            Contact c = new Contact();
            c.id = IdFactory.NextUuid();
            c.pk = company.pk;
            c.doctype = AppConstants.DOCTYPE_CONTACT;
            c.company_id = company.company_id;
            c.contact_id = c.id;
            c.name = (new Bogus.Faker().Person.FullName) + " " + contactSeq;  // contactSeq is a hack to work around the Faker limited namespace
            c.preferred_contact_method = randomContactMethod();
            c.addRole(randomRole());
            c.addNotificationPreference(randomNotificationPreference());
            c.calculateUniqueKey();
            c.created_on = DateTime.Now;
            c.created_by = "migration";
            c.modified_on = DateTime.Now;
            c.modified_by = "migration";
            c.expiration_date = null;
            c.is_deleted = false;
            return c;
        }

        public static string capitalize(string s)
        {
            if (s.Length > 1)
            {
                return char.ToUpper(s[0]) + s.Substring(1);
            }
            else
            {
                return s.ToUpper();
            }
        }

        public static int randomContactCount(Company company)
        {
            if (company.isCorporation())
            {
                return new Random().Next(1, maxContactsPerCorporation + 1);
            }
            else
            {
                return new Random().Next(1, maxContactsPerSmallBusiness + 1);
            }
        }

        public static string randomContactMethod()
        {
            List<string> values = new ApplicationConfig().definedContactMethods;
            int index = new Random().Next(0, values.Count);
            //Console.WriteLine($"randomContactMethod index: {index}");
            return values[index];
        }

        public static string randomRole()
        {
            List<string> values = new ApplicationConfig().definedRoles;
            int index = new Random().Next(0, values.Count);
            //Console.WriteLine($"randomRole index: {index}");
            return values[index];
        }

        public static string randomNotificationPreference()
        {
            List<string> values = new ApplicationConfig().definedNotificationPreferences;
            int index = new Random().Next(0, values.Count);
            //Console.WriteLine($"randomRole index: {index}");
            return values[index];
        }

        private static void generateContactMethods(string format)
        {
            Console.WriteLine("generateContactMethods...");
            List<ContactMethod> contactMethodsList = new List<ContactMethod>();

            foreach (var pk in contactDict.Keys)
            {
                Contact contact = contactDict[pk];
                ContactMethod cm = createContactMethod(contact);
                string json = JsonConvert.SerializeObject(cm);
                //Console.WriteLine($"contact method: {json}");
                contactMethodsList.Add(cm);
            }

            switch (format)
            {
                case (AppConfig.FORMAT_CSV):
                    List<string> lines = new List<string>();
                    for (int i = 0; i < contactMethodsList.Count; i++)
                    {
                        ContactMethod c = contactMethodsList[i];
                        if (i == 0)
                        {
                            lines.Add(c.csvHeader());
                        }
                        lines.Add(c.asCsv());
                    }
                    Console.WriteLine($"contact_methods count: {contactMethodsList.Count}");
                    FileUtil.writeLines(lines, "data/contact_methods.csv");
                    
                    break;

                default:
                    // default to JSON
                    Console.WriteLine($"contact_methods count: {contactMethodsList.Count}");
                    FileUtil.writeObjectAsJson(contactMethodsList, "data/contact_methods.json");
                    break;
            }
            Thread.Sleep(1000);
        }

        public static ContactMethod createContactMethod(Contact contact)
        {
            ContactMethod cm = new ContactMethod(contact);
            cm.id = IdFactory.NextUuid();
            cm.company_id = contact.company_id;
            cm.type = contact.preferred_contact_method;

            switch (contact.preferred_contact_method)
            {
                case AppConstants.CONTACT_METHOD_EMAIL:
                    cm.value = new Bogus.Faker().Internet.Email();
                    break;
                case AppConstants.CONTACT_METHOD_PHONE:
                    cm.value = new Bogus.Faker().Phone.PhoneNumber();
                    break;
                case AppConstants.CONTACT_METHOD_FAX:
                    cm.value = new Bogus.Faker().Phone.PhoneNumber();
                    break;
                case AppConstants.CONTACT_METHOD_MAIL:
                    cm.value = new Bogus.Faker().Address.FullAddress();
                    break;
            }
            cm.memo = "As of " + DateTime.Now.ToString("d");
            cm.created_on = DateTime.Now;
            cm.created_by = "migration";
            cm.modified_on = DateTime.Now;
            cm.modified_by = "migration";
            cm.expiration_date = null;
            cm.is_deleted = false;
            return cm;
        }
        
        public static long epoch()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}