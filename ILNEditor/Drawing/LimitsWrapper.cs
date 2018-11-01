using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(LimitsConverter))]
    public class LimitsWrapper : WrapperBase
    {
        private readonly Limits source;
        private readonly Limits sourceSync;

        public LimitsWrapper(Limits source, Limits sourceSync, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Limits" : name, label)
        {
            this.source = source;
            this.sourceSync = sourceSync;
        }

        #region Limits

        [Category("Limits")]
        public float XMin
        {
            get { return sourceSync.XMin; }
            set { source.XMin = value; }
        }

        [Category("Limits")]
        public float YMin
        {
            get { return sourceSync.YMin; }
            set { source.YMin = value; }
        }

        [Category("Limits")]
        public float ZMin
        {
            get { return sourceSync.ZMin; }
            set { source.ZMin = value; }
        }

        [Category("Limits")]
        public float XMax
        {
            get { return sourceSync.XMax; }
            set { source.XMax = value; }
        }

        [Category("Limits")]
        public float YMax
        {
            get { return sourceSync.YMax; }
            set { source.YMax = value; }
        }

        [Category("Limits")]
        public float ZMax
        {
            get { return sourceSync.ZMax; }
            set { source.ZMax = value; }
        }

        [Category("Behavior")]
        public bool AllowZeroVolume
        {
            get { return source.AllowZeroVolume; }
            set { source.AllowZeroVolume = value; }
        }

        #endregion

        #region Nested type: LimitsConverter

        private class LimitsConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is LimitsWrapper)
                {
                    var limits = (LimitsWrapper) value;

                    return $"{limits.Label} (X:{limits.XMin:F}/{limits.XMax:F}, Y:{limits.YMin:F}/{limits.YMax:F}, Z:{limits.ZMin:F}/{limits.ZMax:F})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
