using API_Demo.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace API_Demo.Services
{
    public class SMSService : ISMSService
    {
        private readonly TwilioSettings _twilio;

        public SMSService(IOptions<TwilioSettings> twilio)
        {
            _twilio = twilio.Value;
        }

        public async Task<MessageResource> SendSMSAsync(string phoneNumber, string body)
        {
            TwilioClient.Init(_twilio.AccountSID, _twilio.AuthToken);

            var result = await MessageResource.CreateAsync(
                body: body,
                from: new PhoneNumber(_twilio.TwilioPhoneNumber),
                to: phoneNumber
            );
            return result;
        }
    }
}
