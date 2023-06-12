using System.ComponentModel;

namespace EmailOTP.Enums;

public enum OTPStatusEnum
{
    [Description("OTP is valid and checked")]
    OK,
    [Description("OTP is wrong after 10 tries")]
    MAXATTEMPT,
    [Description("OTP is wrong")]
    WRONG,
    [Description("Timout after 1 min")]
    TIMEOUT,
    [Description("OTP is expired")]
    EXPIRED,
    [Description("OTP is not checked")]
    NOTCHECKED
}