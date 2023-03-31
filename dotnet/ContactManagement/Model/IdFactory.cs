namespace ContactManagement.Model;

public class IdFactory
{
    private static long sequence = 1000000;
    private static Dictionary<string, string> idDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> uuidDictionary = new Dictionary<string, string>();
    
    public static long NextInt()
    {
        sequence++;
        string seqStr = "" + sequence;
        string uuid = Guid.NewGuid().ToString();
        idDictionary.Add(seqStr, uuid);
        uuidDictionary.Add(uuid, seqStr);
        return sequence;

    }
    
    public static string NextUuid()
    {
        sequence++;
        string seqStr = "" + sequence;
        string uuid = Guid.NewGuid().ToString();
        idDictionary.Add(seqStr, uuid);
        uuidDictionary.Add(uuid, seqStr);
        return uuid;
    }

    public static long lookupUuid(string uuid)
    {
        if (uuidDictionary.ContainsKey(uuid))
        {
            return long.Parse(uuidDictionary[uuid]);
        }
        else
        {
            return -1;
        }
    }
    
    public static string lookupSequence(string seq)
    {
        if (idDictionary.ContainsKey(seq))
        {
            return idDictionary[seq];
        }
        else
        {
            return "";
        }
    }
}
