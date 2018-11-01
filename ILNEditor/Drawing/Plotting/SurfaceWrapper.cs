using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(SurfaceConverter))]
    public class SurfaceWrapper : GroupWrapper
    {
        private readonly TrianglesWrapper fill;
        private readonly ReadOnlyCollection<float> positions;
        private readonly Surface source;
        private readonly LinesWrapper wireframe;

        public SurfaceWrapper(Surface source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, LinePlot.LinePlotTag),
                   String.IsNullOrEmpty(label) ? GetSurfaceLabelFromLegend(source, editor.Panel) : label)
        {
            this.source = source;

            fill = new TrianglesWrapper(source.Fill, editor, Path, Surface.FillTag, "Fill");
            wireframe = new LinesWrapper(source.Wireframe, editor, Path, Surface.WireframeTag, "Wireframe");
            positions = new ReadOnlyCollection<float>(source.Positions.ToList());
        }

        #region Surface

        [Category("Format")]
        public Surface.ColorModes ColorMode
        {
            get { return source.ColorMode; }
            set { source.ColorMode = value; }
        }

        [Category("Format")]
        public Colormaps Colormap
        {
            get { return source.Colormap.Type; }
            set { source.Colormap = new Colormap(value); }
        }

        [Category("Format")]
        public TrianglesWrapper Fill
        {
            get { return fill; }
        }

        [Category("Format")]
        public LinesWrapper Wireframe
        {
            get { return wireframe; }
        }

        [Category("Rendering")]
        public bool? UseLighting
        {
            get { return source.UseLighting; }
            set { source.UseLighting = value; }
        }

        [Category("Positions")]
        [TypeConverter(typeof(PositionsConverter))]
        public ReadOnlyCollection<float> Positions
        {
            // TODO: Make this editable
            get { return positions; }
        }

        #endregion

        #region Helpers

        private static string GetSurfaceLabelFromLegend(Surface source, Panel panel)
        {
            int index = GetNodeIndex(panel, source);
            var legend = panel.Scene.First<Legend>();
            if (legend != null)
            {
                // Get text from LegendItem at the index
                if (legend.Items.Children.Count() > index)
                    return $"{Surface.SurfaceDefaultTag} (\"{legend.Items.Find<LegendItem>().ElementAt(index).Text}\")";
            }

            return null;
        }

        #endregion

        #region Overrides of GroupWrapper

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new Node[] { source.Fill, source.Wireframe }));
        }

        #endregion

        #region Nested type: SurfaceConverter

        private class SurfaceConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is SurfaceWrapper)
                    return ((SurfaceWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #region Nested type: PositionsConverter

        private class PositionsConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ReadOnlyCollection<float>)
                {
                    var positions = (ReadOnlyCollection<float>) value;

                    return $"Positions (N = {positions.Count})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
