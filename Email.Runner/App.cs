using EmailOTP.Services;
using EmailOTP.Enums;
using System.IO;

namespace Email.Runner;

public class App
{
    private readonly ISendService _sendService;
    private readonly IUserService _userService;

    public App(ISendService sendService, IUserService userService)
    {
        _sendService = sendService;
        _userService = userService;
    }

    public async Task Run()
    {
        do
        {
            Console.WriteLine("Welcome to Email OTP");
            var email = await sendEmail();

            verifyOTP(email, TimeSpan.FromSeconds(10));
        } while (true);
    }

    // reads user input for email from user
    private async Task<string> sendEmail()
    {
        var email = "";
        var emailStatus = EmailStatusEnum.FAIL;

        do
        {
            Console.Write("Please enter your email address: ");
            email = Console.ReadLine();
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email address is required");
                continue;
            }
            emailStatus = await _sendService.SendOTP(email);
            if (emailStatus != EmailStatusEnum.OK)
            {
                Console.WriteLine(emailStatus.Description());
            }
        } while (string.IsNullOrEmpty(email) || emailStatus != EmailStatusEnum.OK);

        Console.WriteLine(emailStatus.Description());
        return email;
    }

    private void verifyOTP(string email, TimeSpan readLineTimeout)
    {
        var otp = "";
        var otpStatus = OTPStatusEnum.WRONG;

        var timeoutTask = Task.Delay(readLineTimeout);

        var inputTask = Task.Run(() =>
        {
            do
            {
                Console.Write("Please enter your OTP code: ");
                otp = Console.ReadLine();
                if (string.IsNullOrEmpty(otp))
                {
                    Console.WriteLine("OTP code is required");
                    continue;
                }

                otpStatus = _userService.VerifyOTP(email, otp);
                Console.WriteLine(otpStatus.Description());
                if (otpStatus == OTPStatusEnum.OK || otpStatus == OTPStatusEnum.MAXATTEMPT || otpStatus == OTPStatusEnum.EXPIRED)
                {
                    return;
                }
            } while (string.IsNullOrEmpty(otp) || otpStatus != OTPStatusEnum.OK);
        });

        Task completedTask = Task.WhenAny(timeoutTask, inputTask).GetAwaiter().GetResult();

        if (completedTask == timeoutTask)
        {
            Console.WriteLine(OTPStatusEnum.TIMEOUT.Description());
            return;
        }
    }
}

