using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(CircleConverter))]
    public class CircleWrapper : GroupWrapper
    {
        private readonly LinesWrapper border;
        private readonly TrianglesWrapper fill;
        private readonly Circle source;

        private bool disposed;

        public CircleWrapper(Circle source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, Circle.CircleGroupTag), label)
        {
            this.source = source;

            fill = new TrianglesWrapper(source.Fill, editor, Path, Circle.FillTagDefault);
            border = new LinesWrapper(source.Border, editor, Path, Circle.BorderTagDefault);

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region Circle

        [Category("Format")]
        public TrianglesWrapper Fill
        {
            get { return fill; }
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
            base.Traverse((nodes ?? source.Children).Except(new Node[] { source.Fill, source.Border }));
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

        #region Nested type: CircleConverter

        private class CircleConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is CircleWrapper)
                    return ((CircleWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
