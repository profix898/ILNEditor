using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ConeConverter))]
    public class ConeWrapper : GroupWrapper
    {
        private readonly LinesWrapper border;
        private readonly TrianglesWrapper bottom;
        private readonly TrianglesWrapper hull;
        private readonly Cone source;

        private bool disposed;

        public ConeWrapper(Cone source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, Cone.ConeGroupTag), label)
        {
            this.source = source;

            bottom = new TrianglesWrapper(source.Bottom, editor, Path, Cone.BottomTag);
            hull = new TrianglesWrapper(source.Hull, editor, Path, Cone.HullTag);
            border = new LinesWrapper(source.Border, editor, Path, Cone.BorderTag);

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region Cone

        [Category("Format")]
        public TrianglesWrapper Bottom
        {
            get { return bottom; }
        }

        [Category("Format")]
        public TrianglesWrapper Hull
        {
            get { return hull; }
        }

        [Category("Format")]
        public LinesWrapper Border
        {
            get { return border; }
        }

        #endregion

        private void OnMouseDoubleClick(object sender, MouseEventArgs args)
        {
            MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of WrapperBase

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new Node[] { source.Bottom, source.Hull, source.Border }));
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

        #region Nested type: ConeConverter

        private class ConeConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ConeWrapper)
                    return ((ConeWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
