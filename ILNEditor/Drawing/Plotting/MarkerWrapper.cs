using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(MarkerConverter))]
    public class MarkerWrapper : GroupWrapper
    {
        private readonly LinesWrapper border;
        private readonly TrianglesWrapper fill;
        private readonly Marker source;

        public MarkerWrapper(Marker source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Marker"), label)
        {
            this.source = source;

            fill = new TrianglesWrapper(source.Fill, editor, Path, Marker.DefaultFillTag, "Fill");
            border = new LinesWrapper(source.Border, editor, Path, Marker.DefaultBorderTag, "Border");
        }

        #region Marker

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
        public TrianglesWrapper Fill
        {
            get { return fill; }
        }

        [Category("Marker")]
        public LinesWrapper Border
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

        #region Overrides of GroupWrapper

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new Node[] { source.Fill, source.Border }));
        }

        #endregion

        #region Nested type: MarkerConverter

        private class MarkerConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is MarkerWrapper)
                {
                    var marker = (MarkerWrapper) value;

                    return $"{marker.Label} ({marker.Style}, {marker.Size})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
