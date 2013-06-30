using System.ComponentModel;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    internal abstract class ILNodeWrapper
    {
        private readonly ILPanelEditor editor;
        private readonly ILNode source;

        protected ILNodeWrapper(ILNode source, ILPanelEditor editor)
        {
            this.source = source;
            this.editor = editor;
        }

        #region ILNode

        [Category("Format")]
        public bool Visible
        {
            get { return source.Visible; }
            set { source.Visible = value; }
        }

        [Category("Format")]
        public bool Markable
        {
            get { return source.Markable; }
            set { source.Markable = value; }
        }

        #endregion

        internal ILGroup Parent
        {
            get { return source.Parent; }
        }
    }
}
