using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ribbons.Utils
{
    public static class ExtendedConvert
    {
        public static Vector2 ToVector2(string value)
        {
            value = value.Replace('(', ' ');
            value = value.Replace(')', ' ');
            value = value.Trim();
            string[] components = value.Split(',');
            if (components.Length < 2)
                throw new ArgumentException();
            return new Vector2(Convert.ToSingle(components[0]), Convert.ToSingle(components[1]));
        }

        public static Vector3 ToVector3(string value)
        {
            value = value.Replace('(', ' ');
            value = value.Replace(')', ' ');
            value = value.Trim();
            string[] components = value.Split(',');
            if (components.Length < 3)
                throw new ArgumentException();
            return new Vector3(Convert.ToSingle(components[0]), Convert.ToSingle(components[1]), Convert.ToSingle(components[2]));
        }

        public static Vector4 ToVector4(string value)
        {
            value = value.Replace('(', ' ');
            value = value.Replace(')', ' ');
            value = value.Trim();
            string[] components = value.Split(',');
            if (components.Length < 4)
                throw new ArgumentException();
            return new Vector4(Convert.ToSingle(components[0]), Convert.ToSingle(components[1]), Convert.ToSingle(components[2]), Convert.ToSingle(components[3]));
        }

        public static Color ToColor(string value)
        {
            // This is awful and needs to be fixed
            try { return new Color(ToVector3(value)); }
            catch { return new Color(ToVector4(value)); }
        }

        public static TEnum ToEnum<TEnum>(string value) where TEnum : struct
        {
            TEnum ret;
            bool success = Enum.TryParse<TEnum>(value, out ret);
#if DEBUG
            if (!success)
                Console.WriteLine("ExtendedConvert WARNING: {0} is not a valid enum value for this enumeration.", value);
#endif
            return ret;
        }
    }
}
