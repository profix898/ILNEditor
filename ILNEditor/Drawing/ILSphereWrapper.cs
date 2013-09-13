using System;
using System.ComponentModel;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class ILSphereWrapper : ILGroupWrapper
    {
        private readonly ILTrianglesWrapper fill;
        private readonly ILSphere source;
        private readonly ILLinesWrapper wireframe;

        public ILSphereWrapper(ILSphere source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? ILSphere.DefaultSphereTag : name)
        {
            this.source = source;

            fill = new ILTrianglesWrapper(source.Fill, editor, FullName, ILSphere.DefaultFillTag);
            wireframe = new ILLinesWrapper(source.Wireframe, editor, FullName, ILSphere.DefaultWireframeTag);
        }

        #region ILSphere

        [Category("Format")]
        public ILTrianglesWrapper Fill
        {
            get { return fill; }
        }

        [Category("Format")]
        public ILLinesWrapper Wireframe
        {
            get { return wireframe; }
        }

        #endregion

        #region Overrides of ILWrapperBase

        internal override bool TraverseChildren
        {
            get { return false; }
        }

        #endregion
    }
}
