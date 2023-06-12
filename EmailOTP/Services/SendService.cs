using EmailOTP.Enums;
using EmailOTP.Models;

namespace EmailOTP.Services;

public interface ISendService
{
    Task<EmailStatusEnum> SendOTP(string email);
}

public class SendService : ISendService
{
    private readonly IUserService _userService;

    public SendService(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<EmailStatusEnum> SendOTP(string email)
    {
        // generate OTP code
        // var otp = new OTP(Generate.OTPCode(), DateTime.UtcNow);
        // user check
        var user = _userService.Get(email);
        if (user == null)
        {
            var ok = _userService.Add(email);
            if (!ok)
            {
                return EmailStatusEnum.INVALID;
            }
        }

        var otp = Generate.OTPCode();

        // logging otp for testing purpose, in prod, it should receive in email
        Console.WriteLine($"Your OTP code is {otp}. This code is valid for 1 minute");

        var body = $"Your OTP code is {otp}. This code is valid for 1 minute";
        var emailStatus = await SendEmail(email, body);
        if (emailStatus == EmailStatusEnum.OK)
        {
            // find user and update the OTP code
            _userService.AddOTP(email, otp);
        }

        return emailStatus;
    }

    // mocking this function since its not needed to implement as per the requirement
    private static async Task<EmailStatusEnum> SendEmail(string email, string body)
    {
        return await Task.FromResult(EmailStatusEnum.OK);
    }
}