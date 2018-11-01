using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(PlotCubeDataGroupConverter))]
    public class PlotCubeDataGroupWrapper : GroupWrapper
    {
        private readonly LimitsWrapper limits;
        private readonly ScaleModesWrapper scaleModes;
        private readonly PlotCubeDataGroup source;

        public PlotCubeDataGroupWrapper(PlotCubeDataGroup source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, PlotCubeScaleGroup.PlotsTag),
                   String.IsNullOrEmpty(label) ? GetPlotCubeDataGroupLabel(source, editor.Panel) : label)
        {
            this.source = source;

            scaleModes = new ScaleModesWrapper(source.ScaleModes, editor, Path);
            limits = new LimitsWrapper(source.Limits, GetSyncNode(source).Limits, editor, Path);
        }

        #region PlotCubeDataGroup

        [Category("Format")]
        public ScaleModesWrapper ScaleModes
        {
            get { return scaleModes; }
        }

        [Category("Format")]
        public LimitsWrapper Limits
        {
            get { return limits; }
        }

        #endregion

        #region Helpers

        private static string GetPlotCubeDataGroupLabel(PlotCubeDataGroup source, Panel panel)
        {
            if (panel.Scene.Find<PlotCubeDataGroup>().Count() == 1)
                return "DataGroup";

            return BuildDefaultName(panel, source, "DataGroup");
        }

        #endregion

        #region Nested type: PlotCubeDataGroupConverter

        private class PlotCubeDataGroupConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is PlotCubeDataGroupWrapper)
                {
                    var dataGroup = (PlotCubeDataGroupWrapper) value;

                    return $"{dataGroup.Label} (N = {dataGroup.source.Children.Count})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
