using System;
using System.ComponentModel;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class ILTickWrapper : ILWrapperBase
    {
        private readonly ILLabelWrapper label;
        private readonly ILTick source;

        public ILTickWrapper(ILTick source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Tick" : name, label)
        {
            this.source = source;

            this.label = new ILLabelWrapper(source.Label, editor, path, ILTickCollection.TickLabelTag);
        }

        #region ILTick

        [Category("Format")]
        public float Position
        {
            get { return source.Position; }
            set { source.Position = value; }
        }

        [Category("Format")]
        public ILLabelWrapper TickLabel
        {
            get { return label; }
        }

        #endregion
    }
}
