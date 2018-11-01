using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(GearConverter))]
    public class GearWrapper : GroupWrapper
    {
        private readonly TrianglesWrapper fill;
        private readonly Gear source;
        private readonly LinesWrapper wireframe;

        private bool disposed;

        public GearWrapper(Gear source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Gear"), label)
        {
            this.source = source;

            fill = new TrianglesWrapper(source.Fill, editor, Path, "Fill");
            wireframe = new LinesWrapper(source.Wireframe, editor, Path, "Wireframe");

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region Gear

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

        #region Nested type: GearConverter

        private class GearConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is GearWrapper)
                    return ((GearWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
