namespace ContactManagement.Model;

public class IdFactory
{
    private static long sequence = 1000000;
    private static Dictionary<long, string> idDictionary = new Dictionary<long, string>();
    private static Dictionary<string, long> uuidDictionary = new Dictionary<string, long>();
    
    public static long NextInt()
    {
        sequence++;
        string uuid = Guid.NewGuid().ToString();
        idDictionary.Add(sequence, uuid);
        uuidDictionary.Add(uuid, sequence);
        return sequence;

    }
    
    public static string NextUuid()
    {
        sequence++;
        string uuid = Guid.NewGuid().ToString();
        idDictionary.Add(sequence, uuid);
        uuidDictionary.Add(uuid, sequence);
        return uuid;
    }

    public static long lookupUuid(string uuid)
    {
        if (uuidDictionary.ContainsKey(uuid))
        {
            return uuidDictionary[uuid];
        }
        else
        {
            return -1;
        }
    }
    
    public static string lookupSequence(int seq)
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