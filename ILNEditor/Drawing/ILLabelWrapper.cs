using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILLabelConverter))]
    internal class ILLabelWrapper : ILDrawableWrapper
    {
        private readonly ILLabel source;

        public ILLabelWrapper(ILLabel source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Label" : name)
        {
            this.source = source;

            source.MouseDoubleClick += (sender, args) =>
            {
                if (!args.DirectionUp)
                    return;

                editor.MouseDoubleClickShowEditor(this, args);
            };
        }

        #region ILLines

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

        #region Nested type: ILLabelConverter

        private class ILLabelConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLabelWrapper)
                {
                    var label = (ILLabelWrapper) value;
                    string color = label.Color.HasValue ? (label.Color.Value.IsKnownColor ? label.Color.Value.ToKnownColor().ToString() : label.Color.Value.ToString()) : "";

                    return String.Format("{0} ({1}, {2} {3}pt, {4})", label.Name, label.Text, label.Font.Name, (int) label.Font.SizeInPoints, color);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
