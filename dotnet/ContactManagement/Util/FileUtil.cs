using System.Dynamic;
using ContactManagement.Model;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ContactManagement.Util
{
    /**
     * This class implements local File IO operations for this application.
     * 
     * Chris Joakim, Microsoft, 2023
     */
    class FileUtil
    {
        public static bool writeObjectAsJson(object obj, string outfile)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(outfile, json);
            Console.WriteLine("File written: {0}", outfile);
            return true;
        }

        public static List<GenericDocument> readGenericDocuments(string infile)
        {
            try
            {
                string json = File.ReadAllText(infile);
                List<GenericDocument> docs = JsonSerializer.Deserialize<List<GenericDocument>>(json);
                return docs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<Company> readCompanyDocumentsList(string infile)
        {
            try
            {
                string json = File.ReadAllText(infile);
                List<Company> docs = JsonSerializer.Deserialize<List<Company>>(json);
                return docs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<Contact> readContactDocumentsList(string infile)
        {
            try
            {
                string json = File.ReadAllText(infile);
                List<Contact> docs = JsonSerializer.Deserialize<List<Contact>>(json);
                return docs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<ContactMethod> readContactMethodsDocumentsList(string infile)
        {
            try
            {
                string json = File.ReadAllText(infile);
                List<ContactMethod> docs = JsonSerializer.Deserialize<List<ContactMethod>>(json);
                return docs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<ExpandoObject> readJsonList(string infile)
        {
            using FileStream openStream = File.OpenRead(infile);
            List<ExpandoObject> objects = JsonSerializer.Deserialize<List<ExpandoObject>>(openStream);
            if (objects == null)
            {
                objects = new List<ExpandoObject>();
            }

            return objects;
        }

        public static void writeLines(List<string> lines, string outfile)
        {
            string text = string.Join('\n', lines);
            File.WriteAllText(outfile, text);
            Console.WriteLine("File written: {0}", outfile);
        }
    }
}