using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILSphereConverter))]
    public class ILSphereWrapper : ILGroupWrapper
    {
        private readonly ILTrianglesWrapper fill;
        private readonly ILSphere source;
        private readonly ILLinesWrapper wireframe;

        private bool disposed;

        public ILSphereWrapper(ILSphere source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILSphere.DefaultSphereTag), label)
        {
            this.source = source;

            fill = new ILTrianglesWrapper(source.Fill, editor, Path, ILSphere.DefaultFillTag);
            wireframe = new ILLinesWrapper(source.Wireframe, editor, Path, ILSphere.DefaultWireframeTag);

            this.source.MouseDoubleClick += OnMouseDoubleClick;
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

        private void OnMouseDoubleClick(object sender, ILMouseEventArgs args)
        {
            MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of ILWrapperBase

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new ILNode[] { source.Fill, source.Wireframe }));
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    source.MouseDoubleClick -= OnMouseDoubleClick;

                disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Nested type: ILSphereConverter

        private class ILSphereConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILSphereWrapper)
                    return ((ILSphereWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
