namespace ContactManagement.Model;

/**
* This class is used to generate both UUID and id/sequence values
* in the data generation process.  It also contains lookup logic
* so that either Cosmos DB NoSQL JSON documents can be generated
* with UUID value, while Cosmos DB PostgreSQL CSV rows can be generated
* with id/sequence values.  This allows the various objects or rows
* to be correlated via these identifiers.
*
* Chris Joakim, Microsoft, 2023
*/

public class IdFactory
{
    private static long sequence = 1000000;
    private static Dictionary<string, string> lookupDictionary = new Dictionary<string, string>();

    public static long NextInt()
    {
        sequence++;
        string seqStr = "" + sequence;
        string uuid = Guid.NewGuid().ToString();
        lookupDictionary.Add(seqStr, uuid);
        lookupDictionary.Add(uuid, seqStr);
        return sequence;
    }
    
    public static string NextUuid()
    {
        sequence++;
        string seqStr = "" + sequence;
        string uuid = Guid.NewGuid().ToString();
        lookupDictionary.Add(seqStr, uuid);
        lookupDictionary.Add(uuid, seqStr);
        return uuid;
    }

    public static string lookup(string key)
    {
        if (lookupDictionary.ContainsKey(key))
        {
            return lookupDictionary[key];
        }
        else
        {
            return "?";
        }
    }
}
