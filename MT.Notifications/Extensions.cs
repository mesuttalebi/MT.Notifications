using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MT.Notifications
{
    internal static class Extensions
    {
        public static string GetDescription(this Enum enumObj)
        {
            var description = enumObj.GetType()
                .GetMember(enumObj.ToString())
                .First()
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description;

            if (string.IsNullOrEmpty(description))
                return enumObj.ToString(); 


            return description;
        }
    }
}
