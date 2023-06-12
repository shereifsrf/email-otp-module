using System.ComponentModel;

namespace EmailOTP.Enums;

// generic enum description extension
public static class EnumExtensions
{
    public static string Description(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
}