using EmailOTP.Models;
using EmailOTP.Enums;

namespace EmailOTP.Services;

// this is a mock for database storage

public interface IUserService
{
    bool Add(string email);
    User? Get(string email);
    bool AddOTP(string email, string otpCode);
    OTPStatusEnum VerifyOTP(string email, string otpCode);
}

public class UserService : IUserService
{
    private readonly IList<User> _users;
    private readonly int _maxOTPAttempt = 10;

    public UserService()
    {
        _users = new List<User>();
    }

    public bool Add(string email)
    {
        try
        {
            var user = new User(email, false, null);
            _users.Add(user);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public User? Get(string email)
    {
        return _users.FirstOrDefault(x => x.Email == email);
    }

    public bool AddOTP(string email, string otpCode)
    {
        // check if the user exist
        var user = Get(email);
        if (user == null)
        {
            return false;
        }

        // update otp code
        user.OTP = new OTP(otpCode, DateTime.UtcNow);
        user.OTPAttemptCount = 0;
        user.IsVerified = false;

        return true;
    }

    public OTPStatusEnum VerifyOTP(string email, string otpCode)
    {
        // check if the user exist
        var user = Get(email);
        if (user == null)
        {
            return OTPStatusEnum.NOTCHECKED;
        }

        if (user.OTP == null)
        {
            return OTPStatusEnum.NOTCHECKED;
        }

        // check if the otp code is expired
        if (user.OTP.OTPExpiryTime < DateTime.UtcNow)
        {
            return OTPStatusEnum.EXPIRED;
        }

        // check if the otp code is correct
        if (user.OTP.Code == otpCode)
        {
            // update user
            user.IsVerified = true;
            user.OTP = null;
            user.OTPAttemptCount = 0;
            return OTPStatusEnum.OK;
        }

        user.OTPAttemptCount++;
        if (user.OTPAttemptCount >= _maxOTPAttempt)
        {
            // update user
            user.IsVerified = false;
            user.OTP = null;
            user.OTPAttemptCount = 0;
            return OTPStatusEnum.MAXATTEMPT;
        }

        return OTPStatusEnum.WRONG;
    }
}