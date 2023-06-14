namespace EmailOTP.Tests;

public class GenerateTests
{
    [Fact]
    public void OTPGenerateTest()
    {
        // run parellel 1 million times
        Parallel.For
        (
            0,
            1000000,
            i =>
            {
                var otpCode = Generate.OTPCode();
                Assert.True(otpCode.Length == 6);
                
                // should be between 1 and 999999
                var otpCodeInt = int.Parse(otpCode);
                Assert.True(otpCodeInt >= 1 && otpCodeInt <= 999999);
            }
        );
    }
}