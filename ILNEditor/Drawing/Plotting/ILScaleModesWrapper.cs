using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILScaleModesConverter))]
    internal class ILScaleModesWrapper
    {
        private readonly ILPanelEditor editor;
        private readonly ILScaleModes source;

        public ILScaleModesWrapper(ILScaleModes source, ILPanelEditor editor)
        {
            this.source = source;
            this.editor = editor;
        }

        #region ILScaleModes

        [Category("Format")]
        public AxisScale XAxisScale
        {
            get { return source.XAxisScale; }
            set { source.XAxisScale = value; }
        }

        [Category("Format")]
        public AxisScale YAxisScale
        {
            get { return source.YAxisScale; }
            set { source.YAxisScale = value; }
        }

        [Category("Format")]
        public AxisScale ZAxisScale
        {
            get { return source.ZAxisScale; }
            set { source.ZAxisScale = value; }
        }

        #endregion

        #region Nested type: ILScaleModesConverter

        private class ILScaleModesConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILScaleModesWrapper)
                {
                    var scaleModes = (ILScaleModesWrapper) value;

                    return String.Format("ScaleModes (X:{0}, Y:{1}, Z:{2})", scaleModes.XAxisScale, scaleModes.YAxisScale, scaleModes.ZAxisScale);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
