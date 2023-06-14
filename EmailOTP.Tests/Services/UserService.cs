using EmailOTP.Enums;
using EmailOTP.Services;
using EmailOTP.Models;
using Moq;

namespace EmailOTP.Tests.Services;

public class UserServiceTests : IClassFixture<TestFixture>
{
    TestFixture _fixture;
    const string existingUserEmail = "tester1@dso.org.sg";

    public UserServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("t@f.com", false)]
    [InlineData("test@dso.org.sg", true)]
    public void Add_ShouldReturnCorrectResponse(string email, bool expected)
    {
        // Arrange
        IUserService userService = new UserService();

        // Act
        var actual = userService.Add(email);

        // Assert
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object?[]> GetUserData()
    {
        yield return new object?[] { "t@f.com", null };
        yield return new object?[] { "test@dso.org.com", null };
        yield return new object?[] { existingUserEmail, new User(existingUserEmail, false, null) };
    }

    [Theory]
    [MemberData(nameof(GetUserData))]
    public void Get_ShouldReturnCorrectResponse(string email, User? expected)
    {
        // Arrange
        var mockUserService = _fixture._mockUserService;

        // Act
        var actual = mockUserService.Get(email);

        // Assert object values
        if (expected == null)
        {
            Assert.Null(actual);
        }
        else
        {
            Assert.Equal(expected.Email, actual?.Email);
            Assert.Equal(expected.IsVerified, actual?.IsVerified);
            Assert.Equal(expected.OTPAttemptCount, actual?.OTPAttemptCount);
        }
    }

    [Theory]
    [InlineData("t@f.com", "123456", false)]
    [InlineData("test@dso.org.sg", "123456", false)]
    [InlineData(existingUserEmail, "123456", true)]
    public void AddOTP_ShouldReturnCorrectResponse(string email, string otpCode, bool expected)
    {
        // Arrange
        var mockUserService = _fixture._mockUserService;

        // Act
        var actual = mockUserService.AddOTP(email, otpCode);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AddOTP_ShouldHaveOTP()
    {
        // Arrange
        var mockUserService = _fixture._mockUserService;
        var email = existingUserEmail;

        // Act
        mockUserService.AddOTP(email, "123456");
        var actual = mockUserService.Get(email);

        // Assert
        Assert.NotNull(actual?.OTP);
        // check otp values
        Assert.Equal("123456", actual?.OTP?.Code);
        Assert.Equal(0, actual?.OTPAttemptCount);
        Assert.False(actual?.IsVerified);
    }

    [Fact]
    public void AddOTP_ShouldNotHaveOTP()
    {
        // Arrange
        var mockUserService = _fixture._mockUserService;
        var email = "t@f.com";

        // Act
        mockUserService.AddOTP(email, "123456");
        var actual = mockUserService.Get(email);

        // Assert
        Assert.Null(actual?.OTP);
    }

    [Fact]
    public void VerifyOTP_ShouldNotCheck_UserNotExist()
    {
        // Arrange
        var mockUserService = _fixture._mockUserService;
        var email = "t@f.com";

        // Act
        var actual = mockUserService.VerifyOTP(email, "123456");

        // Assert
        Assert.Equal(OTPStatusEnum.NOTCHECKED, actual);
    }

    [Fact]
    public void VerifyOTP_ShouldNotCheck_OTPNotExist()
    {
        // Arrange
        var mockUserService = _fixture._mockUserService;
        var email = existingUserEmail;

        // Act
        var actual = mockUserService.VerifyOTP(email, "123456");

        // Assert
        Assert.Equal(OTPStatusEnum.NOTCHECKED, actual);
    }

    [Fact]
    public void VerifyOTP_ShouldBeExpired()
    {
        // Arrange
        var mockUserService = _fixture._mockUserService;
        var user = mockUserService.Get(existingUserEmail);
        user!.OTP = new("123456", DateTime.Now);
        user.OTP.OTPExpiryTime = DateTime.Now.AddMinutes(-1);

        // Act
        var actual = mockUserService.VerifyOTP(existingUserEmail, "123456");

        // Assert
        Assert.Equal(OTPStatusEnum.EXPIRED, actual);
    }

    [Fact]
    public void VerifyOTP_ShouldBeWrong()
    {
        // Arrange
        var mockUserService = _fixture._mockUserService;
        var user = mockUserService.Get(existingUserEmail);
        user!.OTP = new("123456", DateTime.Now);

        // Act
        var actual = mockUserService.VerifyOTP(existingUserEmail, "654321");

        // Assert
        Assert.Equal(OTPStatusEnum.WRONG, actual);
    }

    [Fact]
    public void VerifyOTP_ShouldMaxOTPAttempt()
    {
        var mockUserService = _fixture._mockUserService;
        var user = mockUserService.Get(existingUserEmail);
        user!.OTP = new("123456", DateTime.Now);

        // Act
        // run 10
        var actual = OTPStatusEnum.NOTCHECKED;
        for (int i = 0; i < 10; i++)
        {
            actual = mockUserService.VerifyOTP(existingUserEmail, "654321");
        }

        // Assert
        Assert.Equal(OTPStatusEnum.MAXATTEMPT, actual);
    }

    [Fact]
    public void VerifyOTP_ShouldBeOK()
    {
        var mockUserService = _fixture._mockUserService;
        var user = mockUserService.Get(existingUserEmail);
        user!.OTP = new("123456", DateTime.Now);

        // Act
        var actual = mockUserService.VerifyOTP(existingUserEmail, "123456");

        // Assert
        Assert.Equal(OTPStatusEnum.OK, actual);
    }
}