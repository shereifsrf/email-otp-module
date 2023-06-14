using EmailOTP.Models;
using EmailOTP.Services;
using Moq;

namespace EmailOTP.Tests;

public class TestFixture
{
    public readonly IUserService _mockUserService;

    public TestFixture()
    {
        _mockUserService = TestDataHelper.MockUserService();
    }
}

public class TestDataHelper
{
    // users
    public static IUserService MockUserService()
    {
        var userService = new UserService();

        userService.Add("tester1@dso.org.sg");
        userService.Add("tester2@dso.org.sg");
        userService.Add("tester3@dso.org.sg");

        return userService;
    }
}