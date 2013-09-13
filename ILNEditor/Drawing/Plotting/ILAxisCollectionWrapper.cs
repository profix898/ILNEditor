using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILAxisCollectionConverter))]
    internal class ILAxisCollectionWrapper : ILGroupWrapper
    {
        private readonly ILAxisCollection source;
        private readonly ILAxisWrapper xAxis;
        private readonly ILAxisWrapper yAxis;
        private readonly ILAxisWrapper zAxis;

        public ILAxisCollectionWrapper(ILAxisCollection source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? ILAxisCollection.DefaultTag : name)
        {
            this.source = source;

            xAxis = new ILAxisWrapper(source.XAxis, editor, FullName);
            yAxis = new ILAxisWrapper(source.YAxis, editor, FullName);
            zAxis = new ILAxisWrapper(source.ZAxis, editor, FullName);
        }

        #region ILPlotCube

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
                    return ((ILAxisCollectionWrapper) value).Name;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
