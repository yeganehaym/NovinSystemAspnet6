using ElmahCore;

namespace WebApplication2.Elmah;

public class ElmahNotifier : IErrorNotifier
{
    private string apiKey;
    private string mobile;
    public ElmahNotifier(string apiKey, string mobile)
    {
        this.apiKey = apiKey;
        this.mobile = mobile;
    }
    public void Notify(Error error)
    {
        var ghasedak = new Ghasedak.Core.Api(apiKey);
        ghasedak.SendSMSAsync(error.Message,new string[]{mobile});
    }

    public string Name { get; }
}