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
    [TypeConverter(typeof(LinePlotConverter))]
    public class LinePlotWrapper : GroupWrapper
    {
        private readonly LinesWrapper line;
        private readonly MarkerWrapper marker;
        private readonly ReadOnlyCollection<float> positions;
        private readonly LinePlot source;

        public LinePlotWrapper(LinePlot source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, LinePlot.LinePlotTag),
                   String.IsNullOrEmpty(label) ? GetLinePlotLabelFromLegend(source, editor.Panel) : label)
        {
            this.source = source;

            line = new LinesWrapper(source.Line, editor, Path, LinePlot.LineTag, "Line");
            marker = new MarkerWrapper(source.Marker, editor, Path, LinePlot.MarkerTag, "Marker");
            positions = new ReadOnlyCollection<float>(source.Positions.ToList());
        }

        #region LinePlot

        [Category("Format")]
        public LinesWrapper Line
        {
            get { return line; }
        }

        [Category("Format")]
        public MarkerWrapper Marker
        {
            get { return marker; }
        }

        [Category("Positions")]
        [TypeConverter(typeof(PositionsConverter))]
        public ReadOnlyCollection<float> Positions
        {
            // TODO: Make this editable
            get { return positions; }
        }

        #endregion

        #region Overrides of GroupWrapper

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new Node[] { source.Line, source.Marker }));
        }

        #endregion

        #region Helpers

        private static string GetLinePlotLabelFromLegend(LinePlot source, Panel panel)
        {
            int index = GetNodeIndex(panel, source);
            var legend = panel.Scene.First<Legend>();
            if (legend != null)
            {
                // Get text from LegendItem at the index
                if (legend.Items.Children.Count() > index)
                    return $"{LinePlot.LinePlotTag} ('{legend.Items.Find<LegendItem>().ElementAt(index).Text}')";
            }

            return null;
        }

        #endregion

        #region Nested type: LinePlotConverter

        private class LinePlotConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is LinePlotWrapper)
                    return ((LinePlotWrapper) value).Label;

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
