using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILPlotCubeDataGroupConverter))]
    internal class ILPlotCubeDataGroupWrapper : ILGroupWrapper
    {
        private readonly ILLimitsWrapper limits;
        private readonly ILScaleModesWrapper scaleModes;
        private readonly ILPlotCubeDataGroup source;

        public ILPlotCubeDataGroupWrapper(ILPlotCubeDataGroup source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILPlotCubeScaleGroup.PlotsTag),
                   String.IsNullOrEmpty(label) ? GetPlotCubeDataGroupLabel(source, editor.Panel) : label)
        {
            this.source = source;

            scaleModes = new ILScaleModesWrapper(source.ScaleModes, Editor, Path);
            limits = new ILLimitsWrapper(source.Limits, Editor, Path);
        }

        #region ILPlotCubeDataGroup

        [Category("Format")]
        public ILScaleModesWrapper ScaleModes
        {
            get { return scaleModes; }
        }

        [Category("Format")]
        public ILLimitsWrapper Limits
        {
            get { return limits; }
        }

        #endregion

        #region Helper

        private static string GetPlotCubeDataGroupLabel(ILPlotCubeDataGroup source, ILPanel panel)
        {
            if (panel.Scene.Find<ILPlotCubeDataGroup>().Count() == 1)
                return "DataGroup";

            return BuildDefaultName(panel, source, "DataGroup");
        }

        #endregion

        #region Nested type: ILPlotCubeDataGroupConverter

        private class ILPlotCubeDataGroupConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILPlotCubeDataGroupWrapper)
                {
                    var dataGroup = (ILPlotCubeDataGroupWrapper) value;

                    return String.Format("{0} (N = {1})", dataGroup.Label, dataGroup.source.Children.Count);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
