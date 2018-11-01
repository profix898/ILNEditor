using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    public abstract class DrawableWrapper : NodeWrapper
    {
        private readonly Drawable source;

        protected DrawableWrapper(Drawable source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Drawable" : name, label)
        {
            this.source = source;
        }

        #region Drawable

        [Category("Format")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color? Color
        {
            get { return source.Color; }
            set { source.Color = value; }
        }

        #endregion
    }
}
