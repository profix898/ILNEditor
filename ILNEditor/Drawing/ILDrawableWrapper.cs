using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    internal abstract class ILDrawableWrapper : ILNodeWrapper
    {
        private readonly ILPanelEditor editor;
        private readonly ILDrawable source;

        protected ILDrawableWrapper(ILDrawable source, ILPanelEditor editor)
            : base(source, editor)
        {
            this.source = source;
            this.editor = editor;
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
