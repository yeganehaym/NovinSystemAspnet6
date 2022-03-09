namespace WebApplication2;

public static class Utils
{
    private static Random random = new Random();
    public static string RandomString(RandomType type, int length)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        if (type == RandomType.Letters)
        {
            chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }
        else if(type==RandomType.Numbers)
        {
            chars = "0123456789";
        }
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    public enum RandomType
    {
        All,
        Letters,
        Numbers
    }
}