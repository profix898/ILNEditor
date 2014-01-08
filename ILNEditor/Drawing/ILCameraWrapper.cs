using System;
using System.ComponentModel;
using System.Globalization;
using ILNEditor.TypeConverters;
using ILNEditor.TypeExpanders;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILCameraConverter))]
    internal class ILCameraWrapper : ILGroupWrapper
    {
        private readonly ILCamera source;

        private bool disposed;

        public ILCameraWrapper(ILCamera source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Camera"), label)
        {
            // ILCamera needs to be accessed from SceneSyncRoot (instead of Scene)
            this.source = editor.Panel.SceneSyncRoot.FindById<ILCamera>(source.ID);

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region ILCamera

        [Category("Camera")]
        public RectangleFExpander ScreenRect
        {
            get { return new RectangleFExpander(source, "ScreenRect"); }
        }

        [Category("Camera")]
        public bool IsGlobal
        {
            get { return source.IsGlobal; }
            set { source.IsGlobal = value; }
        }

        [Category("Camera")]
        public Projection Projection
        {
            get { return source.Projection; }
            set { source.Projection = value; }
        }

        [Category("Camera")]
        public float ZNear
        {
            get { return source.ZNear; }
            set { source.ZNear = value; }
        }

        [Category("Camera")]
        public float ZFar
        {
            get { return source.ZFar; }
            set { source.ZFar = value; }
        }

        [Category("Camera")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 LookAt
        {
            get { return source.LookAt; }
            set { source.LookAt = value; }
        }

        [Category("Camera")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Position
        {
            get { return source.Position; }
            set { source.Position = value; }
        }

        [Category("Camera")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Top
        {
            get { return source.Top; }
            set { source.Top = value; }
        }

        [Category("Camera")]
        public AspectRatioMode AspectRatioMode
        {
            get { return source.AspectRatioMode; }
            set { source.AspectRatioMode = value; }
        }

        [Category("Camera")]
        public float FieldOfView
        {
            get { return source.FieldOfView; }
            set { source.FieldOfView = value; }
        }

        #endregion

        private void OnMouseDoubleClick(object sender, ILMouseEventArgs args)
        {
            if (!args.DirectionUp || args.ShiftPressed)
                return;

            Editor.MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of ILWrapperBase

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

        #region Nested type: ILCameraConverter

        private class ILCameraConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILCameraWrapper)
                    return ((ILCameraWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
