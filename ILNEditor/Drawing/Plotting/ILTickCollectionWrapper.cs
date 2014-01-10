using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILTickCollectionConverter))]
    public class ILTickCollectionWrapper : ILGroupWrapper
    {
        private readonly ILLinesWrapper lines;
        private readonly ILTickCollection source;
        private readonly ReadOnlyCollection<ILTickWrapper> ticks;

        public ILTickCollectionWrapper(ILTickCollection source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILPlotCube.DefaultTag), label)
        {
            // ILTickCollection needs to be accessed from SceneSyncRoot (instead of Scene)
            this.source = GetSyncNode(source);

            lines = new ILLinesWrapper(this.source.Lines, editor, Path, ILTickCollection.TickLinesTag, "TickLines");
            ticks = new ReadOnlyCollection<ILTickWrapper>(((IEnumerable<ILTick>) source).Select(tick => new ILTickWrapper(tick, editor, Path)).ToList());
        }

        #region ILTickCollection

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
        public ILLinesWrapper Lines
        {
            get { return lines; }
        }

        [Category("Ticks")]
        public ReadOnlyCollection<ILTickWrapper> Ticks
        {
            get { return ticks; }
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

        #endregion

        #region Nested type: ILTickCollectionConverter

        private class ILTickCollectionConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILTickCollectionWrapper)
                    return ((ILTickCollectionWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
