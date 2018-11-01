using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNEditor.TypeExpanders;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(PlotCubeScaleGroupConverter))]
    public class PlotCubeScaleGroupWrapper : GroupWrapper
    {
        private readonly AxisCollectionWrapper axes;
        private readonly LinesWrapper lines;
        private readonly PlotCubeScaleGroup source;

        public PlotCubeScaleGroupWrapper(PlotCubeScaleGroup source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, PlotCubeScaleGroup.DefaultTag),
                   String.IsNullOrEmpty(label) ? GetPlotCubeScaleGroupLabel(source, editor.Panel) : label)
        {
            this.source = source;

            axes = new AxisCollectionWrapper(source.First<AxisCollection>(PlotCubeScaleGroup.AxesTag), editor, Path, PlotCubeScaleGroup.AxesTag);
            lines = new LinesWrapper(source.First<Lines>(PlotCubeScaleGroup.LinesTag), editor, Path, PlotCubeScaleGroup.LinesTag);
        }

        #region PlotCubeScaleGroup

        [Category("Format")]
        public AxisCollectionWrapper Axes
        {
            get { return axes; }
        }

        [Category("Format")]
        public LinesWrapper Lines
        {
            get { return lines; }
        }

        [Category("Format")]
        public Matrix4Expander Rotation
        {
            get { return new Matrix4Expander(source, "Rotation"); }
        }

        [Browsable(false)]
        public Matrix4 RotationMatrix
        {
            get { return GetSyncNode(source).Rotation; }
            set { source.Rotation = value; }
        }

        #endregion

        #region Overrides of GroupWrapper

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new[] { (Node) axes.Source, (Node) lines.Source }));
        }

        #endregion

        #region Helpers

        private static string GetPlotCubeScaleGroupLabel(PlotCubeScaleGroup source, Panel panel)
        {
            if (panel.Scene.Find<PlotCubeScaleGroup>().Count() == 1)
                return "ScaleGroup";

            return BuildDefaultName(panel, source, "ScaleGroup");
        }

        #endregion

        #region Nested type: PlotCubeScaleGroupConverter

        private class PlotCubeScaleGroupConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is PlotCubeScaleGroupWrapper)
                    return ((PlotCubeScaleGroupWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
