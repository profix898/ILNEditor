using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using ILNEditor.TypeConverters;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(AxisConverter))]
    public class AxisWrapper : GroupWrapper
    {
        private readonly LinesWrapper gridMajor;
        private readonly LinesWrapper gridMinor;
        private readonly LabelWrapper label;
        private readonly LabelWrapper scaleLabel;
        private readonly Axis source;
        private readonly TickCollectionWrapper ticks;

        public AxisWrapper(Axis source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? source.AxisName.ToString().Replace(" ", String.Empty) : name,
                   String.IsNullOrEmpty(label) ? source.AxisName.ToString() : label)
        {
            this.source = source;

            this.label = new LabelWrapper(source.Label, editor, Path, Axis.LabelTag, "AxisLabel");
            scaleLabel = new LabelWrapper(source.ScaleLabel, editor, Path, Axis.ScaleLabelTag, "ScaleLabel");
            ticks = new TickCollectionWrapper(source.Ticks, editor, Path, "TicksCollection");
            gridMajor = new LinesWrapper(source.GridMajor, editor, Path, Axis.GridMajorLinesTag, "GridMajor");
            gridMinor = new LinesWrapper(source.GridMinor, editor, Path, Axis.GridMinorLinesTag, "GridMinor");
        }

        #region Axis

        [Category("Label")]
        [TypeConverter(typeof(PointFConverter))]
        public PointF? LabelAnchor
        {
            get { return source.LabelAnchor; }
            set { source.LabelAnchor = value; }
        }

        [Category("Label")]
        [TypeConverter(typeof(PointFConverter))]
        public PointF? ScaleLabelAnchor
        {
            get { return source.ScaleLabelAnchor; }
            set { source.ScaleLabelAnchor = value; }
        }

        [Category("Label")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3? LabelPosition
        {
            get { return source.LabelPosition; }
            set { source.LabelPosition = value; }
        }

        [Category("Label")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3? ScaleLabelPosition
        {
            get { return source.ScaleLabelPosition; }
            set { source.ScaleLabelPosition = value; }
        }

        [Category("Axis")]
        public AxisNames AxisName
        {
            get { return source.AxisName; }
            set { source.AxisName = value; }
        }

        [Category("Axis")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3? Position
        {
            get { return source.Position; }
            set { source.Position = value; }
        }

        [Category("Axis")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Direction
        {
            get { return source.Direction; }
            set { source.Direction = value; }
        }

        [Category("Axis")]
        public float? Min
        {
            get { return source.Min; }
            set { source.Min = value; }
        }

        [Category("Axis")]
        public float? Max
        {
            get { return source.Max; }
            set { source.Max = value; }
        }

        [Category("Label")]
        public new LabelWrapper Label
        {
            get { return label; }
        }

        [Category("Label")]
        public LabelWrapper ScaleLabel
        {
            get { return scaleLabel; }
        }

        [Category("Axis")]
        public TickCollectionWrapper Ticks
        {
            get { return ticks; }
        }

        [Category("Grid")]
        public LinesWrapper GridMajor
        {
            get { return gridMajor; }
        }

        [Category("Grid")]
        public LinesWrapper GridMinor
        {
            get { return gridMinor; }
        }

        #endregion

        #region Overrides of GroupWrapper

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new Node[] { source.Label, source.ScaleLabel, source.Ticks, source.GridMajor, source.GridMinor }));
        }

        #endregion

        #region Nested type: AxisConverter

        private class AxisConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is AxisWrapper)
                {
                    var axis = (AxisWrapper) value;

                    return $"Axis ({axis.AxisName})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
