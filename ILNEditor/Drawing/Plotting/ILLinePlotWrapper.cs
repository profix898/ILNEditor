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
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class ILLinePlotWrapper : ILGroupWrapper
    {
        private readonly ILLinesWrapper line;
        private readonly ILMarkerWrapper marker;
        private readonly ReadOnlyCollection<float> positions;
        private readonly ILLinePlot source;

        public ILLinePlotWrapper(ILLinePlot source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? GetLinePlotLabelFromLegend(source, editor.Panel) : name)
        {
            this.source = source;

            line = new ILLinesWrapper(source.Line, editor, FullName, ILLinePlot.LineTag);
            marker = new ILMarkerWrapper(source.Marker, editor, FullName, ILLinePlot.MarkerTag);
            positions = new ReadOnlyCollection<float>(source.Positions.ToList());

            source.MouseDoubleClick += (sender, args) => editor.MouseDoubleClickShowEditor(this, args);
        }

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

        #region Helper

        private static string GetLinePlotLabelFromLegend(ILLinePlot source, ILPanel panel)
        {
            // Find index of ILLinePlot
            var plotCube = panel.Scene.First<ILPlotCube>();
            IEnumerable<ILLinePlot> linePlots = plotCube.Find<ILLinePlot>();
            int index = linePlots.TakeWhile(linePlot => linePlot != source).Count();

            var legend = panel.Scene.First<ILLegend>();
            if (legend != null)
            {
                // Get text from ILLegendItem at the index
                if (legend.Items.Children.Count() > index)
                    return String.Format("{0}(\"{1}\")", ILLinePlot.LinePlotTag, legend.Items.Find<ILLegendItem>().ElementAt(index).Text);
            }

            return String.Format("{0}#{1}", ILLinePlot.LinePlotTag, index + 1);
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

                    return String.Format("Positions (N = {0})", positions.Count);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #region Overrides of ILWrapperBase

        internal override bool TraverseChildren
        {
            get { return false; }
        }

        #endregion
    }
}
