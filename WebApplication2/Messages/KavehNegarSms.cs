namespace WebApplication2.Messages;

public class KavehNegarSms:ISmsManager
{
    public Task<string> SendSmsAsync(string mobile, string message)
    {
        return Task.FromResult("1456");
    }

    public Task<string> SendOtpMessage(string mobile, string template, string[] param)
    {
        return Task.FromResult("50000");
    }
}