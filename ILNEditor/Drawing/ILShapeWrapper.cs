using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    public abstract class ILShapeWrapper : ILDrawableWrapper
    {
        private readonly ILShape source;
        private readonly ILShape sourceSync;

        private bool disposed;

        protected ILShapeWrapper(ILShape source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Shape"), label)
        {
            this.source = source;

            // Subscribe mouse events on SceneSyncRoot (instead of Scene)
            sourceSync = editor.Panel.SceneSyncRoot.FindById<ILShape>(source.ID);
            sourceSync.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region ILShape

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

        private void OnMouseDoubleClick(object sender, ILMouseEventArgs args)
        {
            if (!args.DirectionUp)
                return;

            MouseDoubleClickShowEditor(this, args);
        }

        #region Overrides of ILWrapperBase

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
