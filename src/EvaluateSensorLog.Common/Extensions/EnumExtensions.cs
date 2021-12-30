using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EvaluateSensorLog.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the enum description attribute or the enum name if no description is present
        /// </summary>
        /// <param name="value">The Enum</param>
        /// <returns>The string description of the enum or the enum name if no description is present</returns>
        public static string GetDescription(this Enum value)
        {
            Type genericEnumType = value.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(value.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] _Attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (_Attribs != null && _Attribs.Length > 0)
                {
                    return ((DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }

            return value.ToString();
        }
    }
}
