using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILConeConverter))]
    public class ILConeWrapper : ILGroupWrapper
    {
        private readonly ILLinesWrapper border;
        private readonly ILTrianglesWrapper bottom;
        private readonly ILTrianglesWrapper hull;
        private readonly ILCone source;

        private bool disposed;

        public ILConeWrapper(ILCone source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILCone.ConeGroupTag), label)
        {
            this.source = source;

            bottom = new ILTrianglesWrapper(source.Bottom, editor, Path, ILCone.BottomTag);
            hull = new ILTrianglesWrapper(source.Hull, editor, Path, ILCone.HullTag);
            border = new ILLinesWrapper(source.Border, editor, Path, ILCone.BorderTag);

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region ILCone

        [Category("Format")]
        public ILTrianglesWrapper Bottom
        {
            get { return bottom; }
        }

        [Category("Format")]
        public ILTrianglesWrapper Hull
        {
            get { return hull; }
        }

        [Category("Format")]
        public ILLinesWrapper Border
        {
            get { return border; }
        }

        #endregion

        private void OnMouseDoubleClick(object sender, ILMouseEventArgs args)
        {
            MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of ILWrapperBase

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new ILNode[] { source.Bottom, source.Hull, source.Border }));
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

        #region Nested type: ILConeConverter

        private class ILConeConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILConeWrapper)
                    return ((ILConeWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
