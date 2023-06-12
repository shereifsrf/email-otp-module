namespace EmailOTP;

public static class Generate
{
    public static string OTPCode()
    {
        var random = new Random();
        var otpCode = random.Next(1, 999999).ToString().PadLeft(6, '0');
        return otpCode;
    }
}