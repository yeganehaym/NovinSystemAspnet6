using System.Security.Claims;

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

    public static int GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (claim == null)
            return 0;
        var claimValue = claim.Value;
        var userId = int.Parse(claimValue);
        return userId;
    }
    
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var claim = user.Claims.First(x => x.Type == ClaimTypes.Name);
        var claimValue = claim.Value;
        return claimValue;
    }
    public static string GetFullName(this ClaimsPrincipal user)
    {
        var claim = user.Claims.First(x => x.Type == ClaimTypes.GivenName);
        var claimValue = claim.Value;
        return claimValue;
    }

    public static string MyExtensionMethod(this string s,string v)
    {
        return s + v;
    }
}