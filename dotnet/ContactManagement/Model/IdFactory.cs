namespace ContactManagement.Model;

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
