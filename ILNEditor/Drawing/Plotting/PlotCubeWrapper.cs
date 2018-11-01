using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using MouseEventArgs = ILNumerics.Drawing.MouseEventArgs;
using Panel = ILNumerics.Drawing.Panel;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(PlotCubeConverter))]
    public class PlotCubeWrapper : CameraWrapper
    {
        private readonly ContextMenu contextMenu = new ContextMenu();

        private readonly PlotCube source;
        private readonly PlotCube sourceSync;

        private bool disposed;

        public PlotCubeWrapper(PlotCube source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, PlotCube.DefaultTag),
                   String.IsNullOrEmpty(label) ? GetPlotCubeLabel(source, editor.Panel) : label)
        {
            this.source = source;

            // Reset view
            contextMenu.MenuItems.Add("Reset View", (o, args) =>
            {
                Panel.SceneSyncRoot.First<PlotCube>().Reset();
                Panel.SceneSyncRoot.First<PlotCubeDataGroup>().Reset();
                Panel.Refresh();
            });

            // Switch planes
            if (!TwoDMode)
            {
                contextMenu.MenuItems.Add("-");
                contextMenu.MenuItems.Add("X-Y Plane", (o, args) =>
                {
                    Panel.SceneSyncRoot.First<PlotCube>().Rotation = Matrix4.Identity;
                    Panel.Refresh();
                });
                contextMenu.MenuItems.Add("X-Z Plane", (o, args) =>
                {
                    Panel.SceneSyncRoot.First<PlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitX, Math.PI / 2.0);
                    Panel.Refresh();
                });
                contextMenu.MenuItems.Add("Y-Z Plane", (o, args) =>
                {
                    Panel.SceneSyncRoot.First<PlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitY, Math.PI / 2.0);
                    Panel.Refresh();
                });
            }

            // Plot browser
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("Plot Browser", (o, args) => Editor.Editor.PlotBrowser.Show());

            // Subscribe mouse events on SceneSyncRoot (instead of Scene)
            sourceSync = GetSyncNode(source);
            sourceSync.MouseClick += OnMouseClick;
        }

        #region PlotCube

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

        //[Category("Format")]
        //public AxisCollectionWrapper Axes
        //{
        //    get { return axes; }
        //}

        //[Category("Format")]
        //public ScaleModesWrapper ScaleModes
        //{
        //    get { return scaleModes; }
        //}

        //[Category("Format")]
        //public LimitsWrapper Limits
        //{
        //    get { return limits; }
        //}

        //[Category("Appearance")]
        //public LinesWrapper Lines
        //{
        //    get { return lines; }
        //}

        #endregion

        #region ContextMenu

        [Browsable(false)]
        public Menu.MenuItemCollection MenuItems => contextMenu.MenuItems;

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.Cancel)
                return;

            contextMenu.Show(Panel, e.Location);

            e.Cancel = true;
        }

        #endregion

        #region Overrides of WrapperBase

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse((nodes ?? source).Where(node => node is PlotCubeScaleGroup)); // PlotCubeScaleGroups
            base.Traverse((nodes ?? source.Children).Except(source.Plots));
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    sourceSync.MouseClick -= OnMouseClick;

                disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Helpers

        private static string GetPlotCubeLabel(PlotCube source, Panel panel)
        {
            if (panel.Scene.Find<PlotCube>().Count() == 1)
                return PlotCube.DefaultTag;

            return BuildDefaultName(panel, source, PlotCube.DefaultTag);
        }

        #endregion

        #region Nested type: PlotCubeConverter

        private class PlotCubeConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is PlotCubeWrapper)
                    return ((PlotCubeWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
