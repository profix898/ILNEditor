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
    [TypeConverter(typeof(ILAxisConverter))]
    public class ILAxisWrapper : ILGroupWrapper
    {
        private readonly ILLinesWrapper gridMajor;
        private readonly ILLinesWrapper gridMinor;
        private readonly ILLabelWrapper label;
        private readonly ILLabelWrapper scaleLabel;
        private readonly ILAxis source;
        private readonly ILTickCollectionWrapper ticks;

        public ILAxisWrapper(ILAxis source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? source.AxisName.ToString().Replace(" ", String.Empty) : name,
                   String.IsNullOrEmpty(label) ? source.AxisName.ToString() : label)
        {
            this.source = source;

            this.label = new ILLabelWrapper(source.Label, editor, Path, ILAxis.LabelTag, "AxisLabel");
            scaleLabel = new ILLabelWrapper(source.ScaleLabel, editor, Path, ILAxis.ScaleLabelTag, "ScaleLabel");
            ticks = new ILTickCollectionWrapper(source.Ticks, editor, Path, "TicksCollection");
            gridMajor = new ILLinesWrapper(source.GridMajor, editor, Path, ILAxis.GridMajorLinesTag, "GridMajor");
            gridMinor = new ILLinesWrapper(source.GridMinor, editor, Path, ILAxis.GridMinorLinesTag, "GridMinor");
        }

        #region ILAxis

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
        public ILLabelWrapper Label
        {
            get { return label; }
        }

        [Category("Label")]
        public ILLabelWrapper ScaleLabel
        {
            get { return scaleLabel; }
        }

        [Category("Axis")]
        public ILTickCollectionWrapper Ticks
        {
            get { return ticks; }
        }

        [Category("Grid")]
        public ILLinesWrapper GridMajor
        {
            get { return gridMajor; }
        }

        [Category("Grid")]
        public ILLinesWrapper GridMinor
        {
            get { return gridMinor; }
        }

        #endregion

        #region Overrides of ILGroupWrapper

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new ILNode[] { source.Label, source.ScaleLabel, source.Ticks, source.GridMajor, source.GridMinor }));
        }

        #endregion

        #region Nested type: ILAxisConverter

        private class ILAxisConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILAxisWrapper)
                {
                    var axis = (ILAxisWrapper) value;

                    return $"Axis ({axis.AxisName})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
