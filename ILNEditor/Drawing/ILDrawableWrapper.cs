using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    internal abstract class ILDrawableWrapper : ILNodeWrapper
    {
        private readonly ILDrawable source;

        protected ILDrawableWrapper(ILDrawable source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Drawable" : name, label)
        {
            this.source = source;
        }

        #region ILDrawable

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
