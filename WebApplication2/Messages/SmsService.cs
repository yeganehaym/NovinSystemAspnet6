namespace WebApplication2.Messages;

public class SmsService
{
    private IConfiguration _configuration;

    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public  ISmsManager GetSmsManger(int provider)
    {
        switch (provider)
        {
            case 0:
                return new GhsedakSmsService(_configuration);
            case 1:
                return new KavehNegarSms();
        }

        return null;
    }
}