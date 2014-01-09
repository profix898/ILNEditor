using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILAxisCollectionConverter))]
    public class ILAxisCollectionWrapper : ILGroupWrapper
    {
        private readonly ILAxisCollection source;
        private readonly ILAxisWrapper xAxis;
        private readonly ILAxisWrapper yAxis;
        private readonly ILAxisWrapper zAxis;

        public ILAxisCollectionWrapper(ILAxisCollection source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILAxisCollection.DefaultTag), label)
        {
            this.source = source;

            xAxis = new ILAxisWrapper(source.XAxis, editor, Path);
            yAxis = new ILAxisWrapper(source.YAxis, editor, Path);
            zAxis = new ILAxisWrapper(source.ZAxis, editor, Path);
        }

        #region ILAxisCollection

        [Category("Axis")]
        public ILAxisWrapper XAxis
        {
            get { return xAxis; }
        }

        [Category("Axis")]
        public ILAxisWrapper YAxis
        {
            get { return yAxis; }
        }

        [Category("Axis")]
        public ILAxisWrapper ZAxis
        {
            get { return zAxis; }
        }

        #endregion

        #region Nested type: ILAxisCollectionConverter

        private class ILAxisCollectionConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILAxisCollectionWrapper)
                    return ((ILAxisCollectionWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
