namespace ContactManagement.Model;

public class IdSequence
{
    private static long _value = 1000000;
    
    public static long Next()
    {
        return _value++;
    }
}