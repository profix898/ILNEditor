using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using ILNumerics.Drawing;

namespace ILNEditor.TypeConverters
{
    public class Vector3Converter : PointConverter
    {
        #region Overrides of PointConverter

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    Match match = new Regex("X=([\\d.,-]+); Y=([\\d.,-]+); Z=([\\d.,-]+)").Match(((string) value).Trim());
                    if (match.Success)
                        return new Vector3(Single.Parse(match.Groups[1].Value), Single.Parse(match.Groups[2].Value), Single.Parse(match.Groups[3].Value));
                }
                catch
                {
                    throw new ArgumentException($"Can not convert '{value}' to type Vector3.");
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is Vector3)
            {
                var vector = (Vector3) value;

                return $"X={vector.X:F}; Y={vector.Y:F}; Z={vector.Z:F}";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
                throw new ArgumentNullException(nameof(propertyValues));

            object x = propertyValues["X"];
            object y = propertyValues["Y"];
            object z = propertyValues["Z"];

            if (x != null && y != null && z != null && x is float && y is float && z is float)
                return new Vector3((float) x, (float) y, (float) z);

            throw new Exception("PropertyValue Invalid Entry");
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Vector3), attributes).Sort(new[] { "X", "Y", "Z" });
            properties.Remove(properties.Find("Length", true));
            properties.Remove(properties.Find("LengthFast", true));
            properties.Remove(properties.Find("LengthSquared", true));

            return properties;
        }

        #endregion
    }
}
