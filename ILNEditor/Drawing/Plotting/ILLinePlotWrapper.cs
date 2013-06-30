using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class ILLinePlotWrapper : ILGroupWrapper
    {
        private readonly ILPanelEditor editor;
        private readonly ILLinesWrapper line;
        private readonly ILMarkerWrapper marker;
        private readonly ReadOnlyCollection<float> positions;
        private readonly ILLinePlot source;

        public ILLinePlotWrapper(ILLinePlot source, ILPanelEditor editor)
            : base(source, editor)
        {
            this.source = source;
            this.editor = editor;

            line = new ILLinesWrapper(source.Line, editor);
            marker = new ILMarkerWrapper(source.Marker, editor);
            positions = new ReadOnlyCollection<float>(source.Positions.ToList());
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
            get { return positions; }
        }

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
    }
}
