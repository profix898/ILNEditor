using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(AxisCollectionConverter))]
    public class AxisCollectionWrapper : GroupWrapper
    {
        private readonly AxisCollection source;
        private readonly AxisWrapper xAxis;
        private readonly AxisWrapper yAxis;
        private readonly AxisWrapper zAxis;

        public AxisCollectionWrapper(AxisCollection source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, AxisCollection.DefaultTag), label)
        {
            this.source = source;

            xAxis = new AxisWrapper(source.XAxis, editor, Path);
            yAxis = new AxisWrapper(source.YAxis, editor, Path);
            zAxis = new AxisWrapper(source.ZAxis, editor, Path);
        }

        #region AxisCollection

        [Category("Axis")]
        public AxisWrapper XAxis
        {
            get { return xAxis; }
        }

        [Category("Axis")]
        public AxisWrapper YAxis
        {
            get { return yAxis; }
        }

        [Category("Axis")]
        public AxisWrapper ZAxis
        {
            get { return zAxis; }
        }

        #endregion

        #region Nested type: AxisCollectionConverter

        private class AxisCollectionConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is AxisCollectionWrapper)
                    return ((AxisCollectionWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
