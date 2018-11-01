using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(SphereConverter))]
    public class SphereWrapper : GroupWrapper
    {
        private readonly TrianglesWrapper fill;
        private readonly Sphere source;
        private readonly LinesWrapper wireframe;

        private bool disposed;

        public SphereWrapper(Sphere source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, Sphere.DefaultSphereTag), label)
        {
            this.source = source;

            fill = new TrianglesWrapper(source.Fill, editor, Path, Sphere.DefaultFillTag);
            wireframe = new LinesWrapper(source.Wireframe, editor, Path, Sphere.DefaultWireframeTag);

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region Sphere

        [Category("Format")]
        public TrianglesWrapper Fill
        {
            get { return fill; }
        }

        [Category("Format")]
        public LinesWrapper Wireframe
        {
            get { return wireframe; }
        }

        #endregion

        private void OnMouseDoubleClick(object sender, MouseEventArgs args)
        {
            MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of WrapperBase

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new Node[] { source.Fill, source.Wireframe }));
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

        #region Nested type: SphereConverter

        private class SphereConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is SphereWrapper)
                    return ((SphereWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
