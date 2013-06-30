using System;
using System.ComponentModel;
using System.Windows.Forms;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    internal class ILPlotCubeWrapper
    {
        private readonly ILAxisCollectionWrapper axes;
        private readonly ILPanelEditor editor;
        private readonly ILLimitsWrapper limits;
        private readonly ILLinesWrapper lines;
        private readonly ILScaleModesWrapper scaleModes;
        private readonly ILPlotCube source;

        public ILPlotCubeWrapper(ILPlotCube source, ILPanelEditor editor)
        {
            this.source = source;
            this.editor = editor;

            axes = new ILAxisCollectionWrapper(source.Axes, editor);
            scaleModes = new ILScaleModesWrapper(source.ScaleModes, editor);
            limits = new ILLimitsWrapper(source.Limits, editor);
            lines = new ILLinesWrapper(source.Lines, editor);

            source.MouseClick += PlotCube_MouseClick;
            source.MouseDoubleClick += (sender, args) =>
            {
                if (!args.DirectionUp)
                    return;

                editor.MouseDoubleClickPropertyForm(this, "PlotCube", args);
            };

            foreach (ILLinePlot linePlot in editor.Panel.Scene.Find<ILLinePlot>())
            {
                var linePlotClosure = new ILLinePlotWrapper(linePlot, editor);
                linePlot.MouseDoubleClick += (sender, args) => editor.MouseDoubleClickPropertyForm(linePlotClosure, "LinePlot", args);
            }
        }

        private void PlotCube_MouseClick(object sender, ILMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Reset View", (o, args) =>
            {
                editor.Panel.SceneSyncRoot.First<ILPlotCube>().Reset();
                editor.Panel.Refresh();
            });
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("X-Y Plane", (o, args) =>
            {
                editor.Panel.SceneSyncRoot.First<ILPlotCube>().Rotation = Matrix4.Identity;
                editor.Panel.Refresh();
            });
            contextMenu.MenuItems.Add("X-Z Plane", (o, args) =>
            {
                editor.Panel.SceneSyncRoot.First<ILPlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitX, Math.PI / 2.0);
                editor.Panel.Refresh();
            });
            contextMenu.MenuItems.Add("Y-Z Plane", (o, args) =>
            {
                editor.Panel.SceneSyncRoot.First<ILPlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitY, Math.PI / 2.0);
                editor.Panel.Refresh();
            });

            contextMenu.Show(editor.Panel, e.Location);

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
