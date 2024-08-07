using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                .Cast<DescriptionAttribute>()
                                                .FirstOrDefault();
        return attribute != null ? attribute.Description : value.ToString();
    }
}
