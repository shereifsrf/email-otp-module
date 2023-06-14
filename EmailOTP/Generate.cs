namespace EmailOTP;

public static class Generate
{
    public static string OTPCode()
    {
        var random = new Random();
        // from 000001 to 999999
        var otpCode = random.Next(1, 1000000).ToString("000000");
        return otpCode;
    }
}