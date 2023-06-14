using EmailOTP.Enums;
using EmailOTP.Services;
using EmailOTP;
using Moq;

namespace EmailOTP.Tests.Services;

public class SendServiceTests : IClassFixture<TestFixture>
{
    TestFixture _fixture;

    public SendServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("test@f.com", EmailStatusEnum.INVALID)]
    [InlineData("test@dso.org.sg", EmailStatusEnum.OK)]
    public async Task SendOTP_ShouldReturnEmailStatusEnum(string email, EmailStatusEnum expected)
    {
        // Arrange
        var sendService = new SendService(_fixture._mockUserService);

        // Act
        var actual = await sendService.SendOTP(email);

        // Assert
        Assert.Equal(expected, actual);
    }
}