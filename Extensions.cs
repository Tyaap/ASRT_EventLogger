namespace EventLogger
{
    using System;
    using System.ComponentModel;

    public static class Extensions
    {
        public static string GetDescription(this object enumerationValue)
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            }

            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }

        public static float ToFloat(this int hex)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(hex), 0);
        }

        public static int ToHex(this float f)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(f), 0);
        }
    }
}
