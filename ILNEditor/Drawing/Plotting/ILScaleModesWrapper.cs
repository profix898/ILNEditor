﻿using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILScaleModesConverter))]
    public class ILScaleModesWrapper : ILWrapperBase
    {
        private readonly ILScaleModes source;

        public ILScaleModesWrapper(ILScaleModes source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "ScaleModes" : name, label)
        {
            this.source = source;
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

                    return $"{scaleModes.Label} (X:{scaleModes.XAxisScale}, Y:{scaleModes.YAxisScale}, Z:{scaleModes.ZAxisScale})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
