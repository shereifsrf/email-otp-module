using System.Text.RegularExpressions;

namespace EmailOTP.Models;

public class User
{
    public string Email { get; set; }
    public bool IsVerified { get; set; }
    public OTP? OTP { get; set; }
    public int OTPAttemptCount { get; set; }

    public User(string email, bool isVerified, OTP? otp)
    {
        // validate email regex
        var emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        if (!emailRegex.IsMatch(email))
        {
            throw new ArgumentException("Email is not valid");
        }

        // accepts email with only @dso.org.sg
        // for eg: abc@dso.org.sg is valid
        // abc@gmail.com is not valid
        if (!email.EndsWith("@dso.org.sg"))
        {
            throw new ArgumentException("Email should end with @dso.org.sg");
        }


        Email = email;
        IsVerified = isVerified;
        OTP = otp;
    }
}