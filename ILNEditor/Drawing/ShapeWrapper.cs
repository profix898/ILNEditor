using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    public abstract class ShapeWrapper : DrawableWrapper
    {
        private readonly Shape source;
        private readonly Shape sourceSync;

        private bool disposed;

        protected ShapeWrapper(Shape source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Shape"), label)
        {
            this.source = source;

            // Subscribe mouse events on SceneSyncRoot (instead of Scene)
            sourceSync = GetSyncNode(source);
            sourceSync.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region Shape

        [Category("Format")]
        public bool Selectable
        {
            get { return source.Selectable; }
            set { source.Selectable = value; }
        }

        [Category("Format")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color SpecularColor
        {
            get { return source.SpecularColor; }
            set { source.SpecularColor = value; }
        }

        [Category("Format")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color EmissionColor
        {
            get { return source.EmissionColor; }
            set { source.EmissionColor = value; }
        }

        [Category("Format")]
        public float Shininess
        {
            get { return source.Shininess; }
            set { source.Shininess = value; }
        }

        [Category("Format")]
        public bool AutoNormals
        {
            get { return source.AutoNormals; }
            set { source.AutoNormals = value; }
        }

        #endregion

        private void OnMouseDoubleClick(object sender, MouseEventArgs args)
        {
            if (!args.DirectionUp)
                return;

            MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of WrapperBase

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    sourceSync.MouseDoubleClick -= OnMouseDoubleClick;

                disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
