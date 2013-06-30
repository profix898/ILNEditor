using System.ComponentModel;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    internal class ILTickWrapper
    {
        private readonly ILPanelEditor editor;
        private readonly ILLabelWrapper label;
        private readonly ILTick source;

        public ILTickWrapper(ILTick source, ILPanelEditor editor)
        {
            this.source = source;
            this.editor = editor;

            label = new ILLabelWrapper(source.Label, editor);
        }

        #region ILTick

        [Category("Format")]
        public float Position
        {
            get { return source.Position; }
            set { source.Position = value; }
        }

        [Category("Format")]
        public ILLabelWrapper Label
        {
            get { return label; }
        }

        #endregion
    }
}
