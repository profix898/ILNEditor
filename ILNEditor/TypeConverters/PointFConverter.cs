using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ILNEditor.TypeConverters
{
    public class PointFConverter : PointConverter
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
                    Match match = new Regex("X=([\\d.,-]+); Y=([\\d.,-]+)").Match(((string) value).Trim());
                    if (match.Success)
                        return new PointF(Single.Parse(match.Groups[1].Value), Single.Parse(match.Groups[2].Value));
                }
                catch
                {
                    throw new ArgumentException(String.Format("Can not convert '{0}' to type PointF.", value));
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is PointF)
            {
                var point = (PointF) value;

                return String.Format("X={0:F}; Y={1:F}", point.X, point.Y);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
                throw new ArgumentNullException("propertyValues");

            object x = propertyValues["X"];
            object y = propertyValues["Y"];

            if (x != null && y != null && x is float && y is float)
                return new PointF((float) x, (float) y);

            throw new Exception("PropertyValue Invalid Entry");
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(PointF), attributes).Sort(new[] { "X", "Y" });
        }

        #endregion
    }
}
