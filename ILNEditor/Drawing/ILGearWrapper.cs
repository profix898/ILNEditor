using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILGearConverter))]
    public class ILGearWrapper : ILGroupWrapper
    {
        private readonly ILTrianglesWrapper fill;
        private readonly ILGear source;
        private readonly ILLinesWrapper wireframe;

        private bool disposed;

        public ILGearWrapper(ILGear source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Gear"), label)
        {
            this.source = source;

            fill = new ILTrianglesWrapper(source.Fill, editor, Path, "Fill");
            wireframe = new ILLinesWrapper(source.Wireframe, editor, Path, "Wireframe");

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region ILGear

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

        #region Nested type: ILGearConverter

        private class ILGearConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILGearWrapper)
                    return ((ILGearWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
