using System;
using System.ComponentModel;
using ILNEditor.TypeConverters;
using ILNEditor.TypeExpander;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class ILCameraWrapper : ILGroupWrapper
    {
        private readonly ILCamera source;

        public ILCameraWrapper(ILCamera source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Camera" : name)
        {
            // ILCamera needs to be accessed from SceneSyncRoot (instead of Scene)
            this.source = editor.Panel.SceneSyncRoot.FindById<ILCamera>(source.ID);
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
    }
}
