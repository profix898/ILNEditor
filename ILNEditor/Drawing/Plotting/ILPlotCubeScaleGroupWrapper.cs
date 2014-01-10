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
    [TypeConverter(typeof(ILPlotCubeScaleGroupConverter))]
    public class ILPlotCubeScaleGroupWrapper : ILGroupWrapper
    {
        private readonly ILAxisCollectionWrapper axes;
        private readonly ILLinesWrapper lines;
        private readonly ILPlotCubeScaleGroup source;

        public ILPlotCubeScaleGroupWrapper(ILPlotCubeScaleGroup source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILPlotCubeScaleGroup.DefaultTag),
                   String.IsNullOrEmpty(label) ? GetPlotCubeScaleGroupLabel(source, editor.Panel) : label)
        {
            this.source = source;

            axes = new ILAxisCollectionWrapper(source.First<ILAxisCollection>(ILPlotCubeScaleGroup.AxesTag), editor, Path, ILPlotCubeScaleGroup.AxesTag);
            lines = new ILLinesWrapper(source.First<ILLines>(ILPlotCubeScaleGroup.LinesTag), editor, Path, ILPlotCubeScaleGroup.LinesTag);
        }

        #region ILPlotCubeScaleGroup

        [Category("Format")]
        public ILAxisCollectionWrapper Axes
        {
            get { return axes; }
        }

        [Category("Format")]
        public ILLinesWrapper Lines
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

        #region Overrides of ILGroupWrapper

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new[] { (ILNode) axes.Source, (ILNode) lines.Source }));
        }

        #endregion

        #region Helpers

        private static string GetPlotCubeScaleGroupLabel(ILPlotCubeScaleGroup source, ILPanel panel)
        {
            if (panel.Scene.Find<ILPlotCubeScaleGroup>().Count() == 1)
                return "ScaleGroup";

            return BuildDefaultName(panel, source, "ScaleGroup");
        }

        #endregion

        #region Nested type: ILPlotCubeScaleGroupConverter

        private class ILPlotCubeScaleGroupConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILPlotCubeScaleGroupWrapper)
                    return ((ILPlotCubeScaleGroupWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
