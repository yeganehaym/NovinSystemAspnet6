namespace WebApplication2.Messages;

public class GhsedakSmsService:ISmsManager
{
    private IConfiguration _configuration;

    public GhsedakSmsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> SendSmsAsync(string mobile, string message)
    {
        var apiKey = _configuration["sms:ghasedak:apikey"].ToString();
        var smsManager =new  Ghasedak.Core.Api(apiKey);
        var result=await smsManager.SendSMSAsync(message, mobile);
        if (result.Result.Code == 200)
            return result.Items[0].ToString();
        
        return null;
    }

    public async Task<string> SendOtpMessage(string mobile, string template, string[] param)
    {
        var apiKey = _configuration["sms:ghasedak:apikey"].ToString();
        var smsManager =new  Ghasedak.Core.Api(apiKey);
        var result=await smsManager.VerifyAsync(1,template, new []{mobile},param[0]);
        if (result.Result.Code == 200)
            return result.Items[0].ToString();
        
        return null;
    }

    public int GetPrice()
    {
        return 40000;
    }
}