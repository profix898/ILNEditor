using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(LabelConverter))]
    public class LabelWrapper : DrawableWrapper
    {
        private readonly Label source;

        private bool disposed;

        public LabelWrapper(Label source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Label"),
                   String.IsNullOrEmpty(label) ? GetLabel(source) : label)
        {
            this.source = source;

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region Lines

        [Category("Label")]
        public string Text
        {
            get { return source.Text; }
            set { source.Text = value; }
        }

        [Category("Label")]
        public Font Font
        {
            get { return source.Font; }
            set { source.Font = value; }
        }

        #endregion

        private void OnMouseDoubleClick(object sender, MouseEventArgs args)
        {
            MouseDoubleClickShowEditor(this, args);
        }

        #region Helpers

        private static string GetLabel(Label source)
        {
            return $"Label ('{source.Text}')";
        }

        #endregion

        #region Overrides of WrapperBase

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

        #region Nested type: LabelConverter

        private class LabelConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is LabelWrapper)
                {
                    var label = (LabelWrapper) value;
                    string color = label.Color.HasValue ? (label.Color.Value.IsKnownColor ? label.Color.Value.ToKnownColor().ToString() : label.Color.Value.ToString()) : "";

                    return $"{label.Name} ({label.Font.Name} {(int) label.Font.SizeInPoints}pt, {color})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
