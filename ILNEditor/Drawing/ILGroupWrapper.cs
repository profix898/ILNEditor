using System;
using System.ComponentModel;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    internal class ILGroupWrapper : ILNodeWrapper
    {
        private readonly ILGroup source;

        public ILGroupWrapper(ILGroup source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Group" : name)
        {
            this.source = source;
        }

        #region ILGroup

        [Category("Format")]
        [Browsable(false)]
        public Matrix4 Transform
        {
            get { return source.Transform; }
            set { source.Transform = value; }
        }

        //public ILNodeCollectionWrapper Children{}

        [Category("Format")]
        public float? Alpha
        {
            get { return source.Alpha; }
            set { source.Alpha = value; }
        }

        //[Category("Format")]
        //public Color? ColorOverride
        //{
        //    get { return source.ColorOverride; }
        //    set { source.ColorOverride = value; }
        //}

        #endregion
    }
}
