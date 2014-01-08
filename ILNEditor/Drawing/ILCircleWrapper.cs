using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILCircleConverter))]
    internal class ILCircleWrapper : ILGroupWrapper
    {
        private readonly ILLinesWrapper border;
        private readonly ILTrianglesWrapper fill;
        private readonly ILCircle source;

        private bool disposed;

        public ILCircleWrapper(ILCircle source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILCircle.CircleGroupTag), label)
        {
            this.source = source;

            fill = new ILTrianglesWrapper(source.Fill, editor, Path, ILCircle.FillTagDefault);
            border = new ILLinesWrapper(source.Border, editor, Path, ILCircle.BorderTagDefault);

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region ILCircle

        [Category("Format")]
        public ILTrianglesWrapper Fill
        {
            get { return fill; }
        }

        [Category("Format")]
        public ILLinesWrapper Border
        {
            get { return border; }
        }

        #endregion

        private void OnMouseDoubleClick(object sender, ILMouseEventArgs args)
        {
            Editor.MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of ILWrapperBase

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new ILNode[] { source.Fill, source.Border }));
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

        #region Nested type: ILCircleConverter

        private class ILCircleConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILCircleWrapper)
                    return ((ILCircleWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
