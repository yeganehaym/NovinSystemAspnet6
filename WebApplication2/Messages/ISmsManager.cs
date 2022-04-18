namespace WebApplication2.Messages;

public interface ISmsManager
{
    Task<string> SendSmsAsync(string mobile, string message);
    Task<string> SendOtpMessage(string mobile, string template, string[] param);
}