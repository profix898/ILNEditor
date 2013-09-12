using System;
using System.ComponentModel;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    internal class ILTickWrapper : ILWrapperBase
    {
        private readonly ILLabelWrapper label;
        private readonly ILTick source;

        public ILTickWrapper(ILTick source, ILPanelEditor editor, string path, string name = null)
            : base(editor, path, String.IsNullOrEmpty(name) ? "Tick" : name)
        {
            this.source = source;

            label = new ILLabelWrapper(source.Label, editor, path);
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
