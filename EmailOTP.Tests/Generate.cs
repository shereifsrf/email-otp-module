namespace EmailOTP.Tests;

public class GenerateTests
{
    [Fact]
    public void OTPGenerateTest()
    {
        // generate 10000 tests
        for (int i = 0; i < 1000000; i++)
        {
            var otpCode = Generate.OTPCode();
            Assert.True(otpCode.Length == 6);
            
            // should be between 1 and 999999
            var otpCodeInt = int.Parse(otpCode);
            Assert.True(otpCodeInt >= 1 && otpCodeInt <= 999999);
        }
    }
}