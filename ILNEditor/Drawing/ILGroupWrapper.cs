using System;
using System.ComponentModel;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
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
        //[Editor(typeof(Matrix4Editor), typeof(UITypeEditor))]
        public Matrix4 Transform
        {
            get { return source.Transform; }
            set { source.Transform = value; }
        }

        [Category("Format")]
        public float? Alpha
        {
            get { return source.Alpha; }
            set { source.Alpha = value; }
        }

        #endregion
    }

    //internal class Matrix4Editor : UITypeEditor
    //{
    //}
}
