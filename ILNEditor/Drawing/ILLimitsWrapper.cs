using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILLimitsConverter))]
    public class ILLimitsWrapper
    {
        private readonly ILPanelEditor editor;
        private readonly ILLimits source;

        public ILLimitsWrapper(ILLimits source, ILPanelEditor editor)
        {
            this.source = source;
            this.editor = editor;
        }

        #region ILLimits

        [Category("Limits")]
        public float XMin
        {
            get { return source.XMin; }
            set { source.XMin = value; }
        }

        [Category("Limits")]
        public float YMin
        {
            get { return source.YMin; }
            set { source.YMin = value; }
        }

        [Category("Limits")]
        public float ZMin
        {
            get { return source.ZMin; }
            set { source.ZMin = value; }
        }

        [Category("Limits")]
        public float XMax
        {
            get { return source.XMax; }
            set { source.XMax = value; }
        }

        [Category("Limits")]
        public float YMax
        {
            get { return source.YMax; }
            set { source.YMax = value; }
        }

        [Category("Limits")]
        public float ZMax
        {
            get { return source.ZMax; }
            set { source.ZMax = value; }
        }

        [Category("Behavior")]
        public bool AllowZeroVolume
        {
            get { return source.AllowZeroVolume; }
            set { source.AllowZeroVolume = value; }
        }

        #endregion

        #region Nested type: ILLimitsConverter

        private class ILLimitsConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLimitsWrapper)
                {
                    var limits = (ILLimitsWrapper) value;

                    return String.Format("Limits (X:{0:F}/{1:F}, Y:{2:F}/{3:F}, Z:{4:F}/{5:F})", limits.XMin, limits.XMax, limits.YMin, limits.YMax, limits.ZMin, limits.ZMax);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
