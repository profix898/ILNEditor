using System;
using System.ComponentModel;
using System.Globalization;
using ILNEditor.Serialization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILLinesConverter))]
    internal class ILLinesWrapper : ILDrawableWrapper
    {
        private readonly ILLines source;

        private bool disposed;

        public ILLinesWrapper(ILLines source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Lines" : name)
        {
            this.source = source;

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region ILLines

        [Category("Format")]
        public int Width
        {
            get { return source.Width; }
            set { source.Width = value; }
        }

        [Category("Format")]
        [SerializerWeight(10)] // De/Serialize last (setting Pattern/PatternScale/etc. forces DashStyle to 'UserPattern' -> we need to override this)
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

        private void OnMouseDoubleClick(object sender, ILMouseEventArgs args)
        {
            if (!args.DirectionUp)
                return;

            Editor.MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of ILWrapperBase

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    source.MouseDoubleClick -= OnMouseDoubleClick;

                disposed = true;
            }

            base.Dispose(disposing);
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

                    return String.Format("{0} ({1}, {2}, {3})", lines.Name, color, lines.DashStyle, lines.Width);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
