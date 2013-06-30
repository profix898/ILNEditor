using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILAxisCollectionConverter))]
    internal class ILAxisCollectionWrapper : ILGroupWrapper
    {
        private readonly ILPanelEditor editor;
        private readonly ILAxisCollection source;
        private readonly ILAxisWrapper xAxis;
        private readonly ILAxisWrapper yAxis;
        private readonly ILAxisWrapper zAxis;

        public ILAxisCollectionWrapper(ILAxisCollection source, ILPanelEditor editor)
            : base(source, editor)
        {
            this.source = source;
            this.editor = editor;

            xAxis = new ILAxisWrapper(source.XAxis, editor);
            yAxis = new ILAxisWrapper(source.YAxis, editor);
            zAxis = new ILAxisWrapper(source.ZAxis, editor);
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
                    return "Axis Collection";

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
