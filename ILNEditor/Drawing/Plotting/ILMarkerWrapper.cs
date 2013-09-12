using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILMarkerConverter))]
    internal class ILMarkerWrapper : ILGroupWrapper
    {
        private readonly ILLinesWrapper border;
        private readonly ILMarker source;

        public ILMarkerWrapper(ILMarker source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Marker" : name)
        {
            this.source = source;

            border = new ILLinesWrapper(source.Border, editor, path);
        }

        [Category("Marker")]
        public int Size
        {
            get { return source.Size; }
            set { source.Size = value; }
        }

        [Category("Marker")]
        public Vector3? Position
        {
            get { return source.Position; }
            set { source.Position = value; }
        }

        [Category("Marker")]
        public ILLinesWrapper Border
        {
            get { return border; }
        }

        [Category("Marker")]
        public MarkerStyle Style
        {
            get { return source.Style; }
            set { source.Style = value; }
        }

        #region Nested type: ILMarkerConverter

        private class ILMarkerConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILMarkerWrapper)
                {
                    var marker = (ILMarkerWrapper) value;

                    return String.Format("{0} ({1}, {2})", marker.Name, marker.Style, marker.Size);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
