using System.ComponentModel;

namespace EmailOTP.Enums;
public enum EmailStatusEnum
{
    [Description("Email containing OTP has been sent successfully")]
    OK,
    [Description("Email address does not exist or sending to the email has failed")]
    FAIL,
    [Description("Email address is invalid")]
    INVALID
}