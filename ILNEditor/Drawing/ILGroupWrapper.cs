using System.ComponentModel;
using System.Drawing;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    internal abstract class ILGroupWrapper : ILNodeWrapper
    {
        private readonly ILPanelEditor editor;
        private readonly ILGroup source;

        protected ILGroupWrapper(ILGroup source, ILPanelEditor editor)
            : base(source, editor)
        {
            this.source = source;
            this.editor = editor;
        }

        #region ILGroup

        [Category("Format")]
        [Browsable(false)]
        public Matrix4 Transform
        {
            get { return source.Transform; }
            set { source.Transform = value; }
        }

        //public ILNodeCollectionWrapper Childs{}

        [Category("Format")]
        public float? Alpha
        {
            get { return source.Alpha; }
            set { source.Alpha = value; }
        }

        [Category("Format")]
        public Color? ColorOverride
        {
            get { return source.ColorOverride; }
            set { source.ColorOverride = value; }
        }

        #endregion
    }
}
