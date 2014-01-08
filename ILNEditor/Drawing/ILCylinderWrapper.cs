using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILCylinderConverter))]
    internal class ILCylinderWrapper : ILGroupWrapper
    {
        private readonly ILCircleWrapper bottom;
        private readonly ILTrianglesWrapper hull;
        private readonly ILCylinder source;
        private readonly ILCircleWrapper top;

        private bool disposed;

        public ILCylinderWrapper(ILCylinder source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILCylinder.CylinderGroupTag), label)
        {
            this.source = source;

            bottom = new ILCircleWrapper(source.Bottom, editor, Path, ILCylinder.BottomTag);
            top = new ILCircleWrapper(source.Top, editor, Path, ILCylinder.TopTag);
            hull = new ILTrianglesWrapper(source.Hull, editor, Path, ILCylinder.HullTag);

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region ILCylinder

        [Category("Format")]
        public ILCircleWrapper Bottom
        {
            get { return bottom; }
        }

        [Category("Format")]
        public ILCircleWrapper Top
        {
            get { return top; }
        }

        [Category("Format")]
        public ILTrianglesWrapper Hull
        {
            get { return hull; }
        }

        #endregion

        private void OnMouseDoubleClick(object sender, ILMouseEventArgs args)
        {
            Editor.MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of ILWrapperBase

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new ILNode[] { source.Bottom, source.Top, source.Hull }));
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

        #region Nested type: ILCylinderConverter

        private class ILCylinderConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILCylinderWrapper)
                    return ((ILCylinderWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
