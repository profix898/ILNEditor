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
    [TypeConverter(typeof(ILLinePlotConverter))]
    public class ILLinePlotWrapper : ILGroupWrapper
    {
        private readonly ILLinesWrapper line;
        private readonly ILMarkerWrapper marker;
        private readonly ReadOnlyCollection<float> positions;
        private readonly ILLinePlot source;

        public ILLinePlotWrapper(ILLinePlot source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILLinePlot.LinePlotTag),
                   String.IsNullOrEmpty(label) ? GetLinePlotLabelFromLegend(source, editor.Panel) : label)
        {
            this.source = source;

            line = new ILLinesWrapper(source.Line, editor, Path, ILLinePlot.LineTag, "Line");
            marker = new ILMarkerWrapper(source.Marker, editor, Path, ILLinePlot.MarkerTag, "Marker");
            positions = new ReadOnlyCollection<float>(source.Positions.ToList());
        }

        #region ILLinePlot

        [Category("Format")]
        public ILLinesWrapper Line
        {
            get { return line; }
        }

        [Category("Format")]
        public ILMarkerWrapper Marker
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

        #region Overrides of ILGroupWrapper

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new ILNode[] { source.Line, source.Marker }));
        }

        #endregion

        #region Helpers

        private static string GetLinePlotLabelFromLegend(ILLinePlot source, ILPanel panel)
        {
            int index = GetNodeIndex(panel, source);
            var legend = panel.Scene.First<ILLegend>();
            if (legend != null)
            {
                // Get text from ILLegendItem at the index
                if (legend.Items.Children.Count() > index)
                    return $"{ILLinePlot.LinePlotTag} ('{legend.Items.Find<ILLegendItem>().ElementAt(index).Text}')";
            }

            return null;
        }

        #endregion

        #region Nested type: ILLinePlotConverter

        private class ILLinePlotConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLinePlotWrapper)
                    return ((ILLinePlotWrapper) value).Label;

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
