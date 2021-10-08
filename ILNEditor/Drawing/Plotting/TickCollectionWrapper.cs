using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(TickCollectionConverter))]
    public class TickCollectionWrapper : GroupWrapper
    {
        private readonly TickCollection source;
        private readonly LinesWrapper lines;
        private readonly LabelWrapper defaultLabel;
        private readonly ReadOnlyCollection<TickWrapper> ticks;

        public TickCollectionWrapper(TickCollection source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, PlotCube.DefaultTag), label)
        {
            // TickCollection needs to be accessed from SceneSyncRoot (instead of Scene)
            this.source = GetSyncNode(source);

            defaultLabel = new LabelWrapper(source.DefaultLabel, editor, path, TickCollection.TickLabelTag, "DefaultTickLabel");
            lines = new LinesWrapper(this.source.Lines, editor, Path, TickCollection.TickLinesTag, "TickLines");
            ticks = new ReadOnlyCollection<TickWrapper>(((IEnumerable<Tick>) source).Select(tick => new TickWrapper(tick, editor, Path)).ToList());
        }

        #region TickCollection

        [Category("Format")]
        public LabelWrapper DefaultLabel
        {
            get { return defaultLabel; }
        }

        [Category("Format")]
        public int MaxNumberDigitsShowFull
        {
            get { return source.MaxNumberDigitsShowFull; }
            set { source.MaxNumberDigitsShowFull = value; }
        }

        [Category("Format")]
        [TypeConverter(typeof(SizeFConverter))]
        public SizeF DefaultTickLabelSize
        {
            get { return source.DefaultTickLabelSize; }
            set { source.DefaultTickLabelSize = value; }
        }

        [Category("Format")]
        public LinesWrapper Lines
        {
            get { return lines; }
        }

        [Category("Format")]
        public TickMode Mode
        {
            get { return source.Mode; }
            set { source.Mode = value; }
        }

        [Category("Format")]
        public Color Color
        {
            get { return source.Color; }
            set { source.Color = value; }
        }

        [Category("Format")]
        public int Width
        {
            get { return source.Width; }
            set { source.Width = value; }
        }

        [Category("Format")]
        public float TickLength
        {
            get { return source.TickLength; }
            set { source.TickLength = value; }
        }

        [Category("Format")]
        public float LengthLevelFraction
        {
            get { return source.LengthLevelFraction; }
            set { source.LengthLevelFraction = value; }
        }

        [Category("Ticks")]
        public ReadOnlyCollection<TickWrapper> Ticks
        {
            get { return ticks; }
        }

        #endregion

        #region Nested type: TickCollectionConverter

        private class TickCollectionConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is TickCollectionWrapper)
                    return ((TickCollectionWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
