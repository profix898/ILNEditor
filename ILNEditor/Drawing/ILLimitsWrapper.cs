using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILLimitsConverter))]
    internal class ILLimitsWrapper : ILWrapperBase
    {
        private readonly ILLimits source;

        public ILLimitsWrapper(ILLimits source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Limits" : name, label)
        {
            this.source = source;
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

                    return String.Format("{0} (X:{1:F}/{2:F}, Y:{3:F}/{4:F}, Z:{5:F}/{6:F})",
                                         limits.Label, limits.XMin, limits.XMax, limits.YMin, limits.YMax, limits.ZMin, limits.ZMax);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
