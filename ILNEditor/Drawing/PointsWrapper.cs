using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(PointsConverter))]
    public class PointsWrapper : ShapeWrapper
    {
        private readonly Points source;

        public PointsWrapper(Points source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Points"), label)
        {
            this.source = source;
        }

        #region Points

        [Category("Points")]
        public float Size
        {
            get { return source.Size; }
            set { source.Size = value; }
        }

        #endregion

        #region Nested type: PointsConverter

        private class PointsConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is PointsWrapper)
                {
                    var points = (PointsWrapper) value;
                    string specularColor = points.SpecularColor.IsKnownColor ? points.SpecularColor.ToKnownColor().ToString() : points.SpecularColor.ToString();
                    string emissionColor = points.SpecularColor.IsKnownColor ? points.SpecularColor.ToKnownColor().ToString() : points.SpecularColor.ToString();

                    return $"{points.Label} ({specularColor}, {emissionColor}, {points.Size})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
