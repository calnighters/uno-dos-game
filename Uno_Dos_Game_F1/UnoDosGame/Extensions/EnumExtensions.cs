using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnoDos.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescriptionFromEnum(this Enum enumValue)
        {
            FieldInfo _EnumField = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute[] _Descriptions = (DescriptionAttribute[])_EnumField.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return _Descriptions.Length > 0 ? _Descriptions[0].Description : enumValue.ToString();
        }
    }
}
