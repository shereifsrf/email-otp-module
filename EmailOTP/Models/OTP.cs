namespace EmailOTP.Models;

public class OTP
{
    public string Code { get; set; }
    public DateTime OTPGeneratedTime { get; set; }
    public DateTime OTPExpiryTime { get; set; }

    public OTP(string otpCode, DateTime otpGeneratedTime)
    {
        // otp should be 6 digits
        if (otpCode.Length != 6)
        {
            throw new ArgumentException("OTP code should be 6 digits");
        }

        Code = otpCode;
        OTPGeneratedTime = otpGeneratedTime;

        // expires after 1 minute
        OTPExpiryTime = otpGeneratedTime.AddMinutes(1);
    }
}