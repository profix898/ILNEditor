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
    internal class ILTickCollectionWrapper : ILGroupWrapper
    {
        private readonly ILPanelEditor editor;
        private readonly ILTickCollection source;
        private readonly ReadOnlyCollection<ILTickWrapper> ticks;

        public ILTickCollectionWrapper(ILTickCollection source, ILPanelEditor editor)
            : base(source, editor)
        {
            this.source = source;
            this.editor = editor;

            ticks = new ReadOnlyCollection<ILTickWrapper>(((IEnumerable<ILTick>) source).Select(tick => new ILTickWrapper(tick, editor)).ToList());
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
        public Font DefaultFont
        {
            get { return source.DefaultFont; }
            set { source.DefaultFont = value; }
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
                    return "Ticks";

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
