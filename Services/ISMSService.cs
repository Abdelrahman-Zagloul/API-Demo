using Twilio.Rest.Api.V2010.Account;

namespace API_Demo.Services
{
    public interface ISMSService
    {
        Task<MessageResource> SendSMSAsync(string phoneNumber, string body);
    }

}
