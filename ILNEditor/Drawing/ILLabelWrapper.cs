﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILLabelConverter))]
    public class ILLabelWrapper : ILDrawableWrapper
    {
        private readonly ILLabel source;

        private bool disposed;

        public ILLabelWrapper(ILLabel source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Label"),
                   String.IsNullOrEmpty(label) ? GetLabel(source) : label)
        {
            this.source = source;

            this.source.MouseDoubleClick += OnMouseDoubleClick;
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

        private void OnMouseDoubleClick(object sender, ILMouseEventArgs args)
        {
            MouseDoubleClickShowEditor(this, args);
        }

        #region Helpers

        private static string GetLabel(ILLabel source)
        {
            return $"Label ('{source.Text}')";
        }

        #endregion

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

        #region Nested type: ILLabelConverter

        private class ILLabelConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLabelWrapper)
                {
                    var label = (ILLabelWrapper) value;
                    string color = label.Color.HasValue ? (label.Color.Value.IsKnownColor ? label.Color.Value.ToKnownColor().ToString() : label.Color.Value.ToString()) : "";

                    return $"{label.Name} ({label.Font.Name} {(int) label.Font.SizeInPoints}pt, {color})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
