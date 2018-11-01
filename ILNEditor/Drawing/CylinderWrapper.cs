using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(CylinderConverter))]
    public class CylinderWrapper : GroupWrapper
    {
        private readonly CircleWrapper bottom;
        private readonly TrianglesWrapper hull;
        private readonly Cylinder source;
        private readonly CircleWrapper top;

        private bool disposed;

        public CylinderWrapper(Cylinder source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, Cylinder.CylinderGroupTag), label)
        {
            this.source = source;

            bottom = new CircleWrapper(source.Bottom, editor, Path, Cylinder.BottomTag);
            top = new CircleWrapper(source.Top, editor, Path, Cylinder.TopTag);
            hull = new TrianglesWrapper(source.Hull, editor, Path, Cylinder.HullTag);

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region Cylinder

        [Category("Format")]
        public CircleWrapper Bottom
        {
            get { return bottom; }
        }

        [Category("Format")]
        public CircleWrapper Top
        {
            get { return top; }
        }

        [Category("Format")]
        public TrianglesWrapper Hull
        {
            get { return hull; }
        }

        #endregion

        private void OnMouseDoubleClick(object sender, MouseEventArgs args)
        {
            MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of WrapperBase

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new Node[] { source.Bottom, source.Top, source.Hull }));
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

        #region Nested type: CylinderConverter

        private class CylinderConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is CylinderWrapper)
                    return ((CylinderWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
