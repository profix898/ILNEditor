using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILMarkerConverter))]
    internal class ILMarkerWrapper : ILGroupWrapper
    {
        private readonly ILLinesWrapper border;
        private readonly ILTrianglesWrapper fill;
        private readonly ILMarker source;

        public ILMarkerWrapper(ILMarker source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Marker"), label)
        {
            this.source = source;

            fill = new ILTrianglesWrapper(source.Fill, editor, Path, ILMarker.DefaultFillTag, "Fill");
            border = new ILLinesWrapper(source.Border, editor, Path, ILMarker.DefaultBorderTag, "Border");
        }

        #region ILMarker

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
        public ILTrianglesWrapper Fill
        {
            get { return fill; }
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

        #endregion

        #region Overrides of ILGroupWrapper

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new ILNode[] { source.Fill, source.Border }));
        }

        #endregion

        #region Nested type: ILMarkerConverter

        private class ILMarkerConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILMarkerWrapper)
                {
                    var marker = (ILMarkerWrapper) value;

                    return String.Format("{0} ({1}, {2})", marker.Label, marker.Style, marker.Size);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
