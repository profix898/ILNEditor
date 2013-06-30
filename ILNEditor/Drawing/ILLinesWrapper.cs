using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILLinesConverter))]
    internal class ILLinesWrapper : ILDrawableWrapper
    {
        private readonly ILPanelEditor editor;
        private readonly ILLines source;

        public ILLinesWrapper(ILLines source, ILPanelEditor editor)
            : base(source, editor)
        {
            this.source = source;
            this.editor = editor;
        }

        #region ILLines

        [Category("Format")]
        public int Width
        {
            get { return source.Width; }
            set { source.Width = value; }
        }

        [Category("Format")]
        public DashStyle DashStyle
        {
            get { return source.DashStyle; }
            set { source.DashStyle = value; }
        }

        [Category("Format")]
        public short Pattern
        {
            get { return source.Pattern; }
            set { source.Pattern = value; }
        }

        [Category("Format")]
        public float PatternScale
        {
            get { return source.PatternScale; }
            set { source.PatternScale = value; }
        }

        [Category("Format")]
        public bool Antialiasing
        {
            get { return source.Antialiasing; }
            set { source.Antialiasing = value; }
        }

        #endregion

        #region Nested type: ILLinesConverter

        private class ILLinesConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLinesWrapper)
                {
                    var lines = (ILLinesWrapper) value;
                    string color = lines.Color.HasValue ? (lines.Color.Value.IsKnownColor ? lines.Color.Value.ToKnownColor().ToString() : lines.Color.Value.ToString()) : "";

                    return String.Format("Line ({0}, {1}, {2})", color, lines.DashStyle, lines.Width);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
