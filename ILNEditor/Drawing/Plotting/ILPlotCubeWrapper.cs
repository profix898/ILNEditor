using System;
using System.ComponentModel;
using System.Windows.Forms;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class ILPlotCubeWrapper : ILWrapperBase
    {
        private readonly ILAxisCollectionWrapper axes;
        private readonly ILLimitsWrapper limits;
        private readonly ILLinesWrapper lines;
        private readonly ILScaleModesWrapper scaleModes;
        private readonly ILPlotCube source;

        public ILPlotCubeWrapper(ILPlotCube source, ILPanelEditor editor, string path, string name = null)
            : base(editor, path, String.IsNullOrEmpty(name) ? "PlotCube" : name)
        {
            this.source = source;

            axes = new ILAxisCollectionWrapper(source.Axes, Editor, FullName);
            scaleModes = new ILScaleModesWrapper(source.ScaleModes, Editor, FullName);
            limits = new ILLimitsWrapper(source.Limits, Editor, FullName);
            lines = new ILLinesWrapper(source.Lines, Editor, FullName, "Box");

            source.MouseClick += PlotCube_MouseClick;
            source.MouseDoubleClick += (sender, args) =>
            {
                if (!args.DirectionUp)
                    return;

                Editor.MouseDoubleClickShowEditor(this, args);
            };
        }

        private void PlotCube_MouseClick(object sender, ILMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Reset View", (o, args) =>
            {
                Editor.Panel.SceneSyncRoot.First<ILPlotCube>().Reset();
                Editor.Panel.Refresh();
            });
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("X-Y Plane", (o, args) =>
            {
                Editor.Panel.SceneSyncRoot.First<ILPlotCube>().Rotation = Matrix4.Identity;
                Editor.Panel.Refresh();
            });
            contextMenu.MenuItems.Add("X-Z Plane", (o, args) =>
            {
                Editor.Panel.SceneSyncRoot.First<ILPlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitX, Math.PI / 2.0);
                Editor.Panel.Refresh();
            });
            contextMenu.MenuItems.Add("Y-Z Plane", (o, args) =>
            {
                Editor.Panel.SceneSyncRoot.First<ILPlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitY, Math.PI / 2.0);
                Editor.Panel.Refresh();
            });

            contextMenu.Show(Editor.Panel, e.Location);

            e.Cancel = true;
        }

        #region ILPlotCube

        [Category("Appearance")]
        public bool TwoDMode
        {
            get { return source.TwoDMode; }
            set { source.TwoDMode = value; }
        }

        [Category("Mouse")]
        public bool AllowPan
        {
            get { return source.AllowPan; }
            set { source.AllowPan = value; }
        }

        [Category("Mouse")]
        public bool AllowRotation
        {
            get { return source.AllowRotation; }
            set { source.AllowRotation = value; }
        }

        [Category("Mouse")]
        public bool AllowZoom
        {
            get { return source.AllowZoom; }
            set { source.AllowZoom = value; }
        }

        [Category("Behavior")]
        public bool AutoScaleOnAdd
        {
            get { return source.AutoScaleOnAdd; }
            set { source.AutoScaleOnAdd = value; }
        }

        [Category("Format")]
        public ILAxisCollectionWrapper Axes
        {
            get { return axes; }
        }

        [Category("Format")]
        public ILScaleModesWrapper ScaleModes
        {
            get { return scaleModes; }
        }

        [Category("Format")]
        public ILLimitsWrapper Limits
        {
            get { return limits; }
        }

        [Category("Appearance")]
        public ILLinesWrapper Lines
        {
            get { return lines; }
        }

        [Category("Appearance")]
        public Projection Projection
        {
            get { return source.Projection; }
            set { source.Projection = value; }
        }

        #endregion
    }
}
