﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILPlotCubeConverter))]
    internal class ILPlotCubeWrapper : ILCameraWrapper
    {
        private readonly ILPlotCube source;

        private bool disposed;

        public ILPlotCubeWrapper(ILPlotCube source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILPlotCube.DefaultTag),
                   String.IsNullOrEmpty(label) ? GetPlotCubeLabel(source, editor.Panel) : label)
        {
            // ILCamera needs to be accessed from SceneSyncRoot (instead of Scene)
            this.source = editor.Panel.SceneSyncRoot.FindById<ILPlotCube>(source.ID);

            this.source.MouseClick += OnMouseClick;
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

        //[Category("Format")]
        //public ILAxisCollectionWrapper Axes
        //{
        //    get { return axes; }
        //}

        //[Category("Format")]
        //public ILScaleModesWrapper ScaleModes
        //{
        //    get { return scaleModes; }
        //}

        //[Category("Format")]
        //public ILLimitsWrapper Limits
        //{
        //    get { return limits; }
        //}

        //[Category("Appearance")]
        //public ILLinesWrapper Lines
        //{
        //    get { return lines; }
        //}

        #endregion

        private void OnMouseClick(object sender, ILMouseEventArgs e)
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

        #region Overrides of ILWrapperBase

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source).Where(node => node is ILPlotCubeScaleGroup)); // ILPlotCubeScaleGroups
            base.Traverse((nodes ?? source.Children).Except(source.Plots));
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    source.MouseClick -= OnMouseClick;

                disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Helper

        private static string GetPlotCubeLabel(ILPlotCube source, ILPanel panel)
        {
            if (panel.Scene.Find<ILPlotCube>().Count() == 1)
                return ILPlotCube.DefaultTag;

            return BuildDefaultName(panel, source, ILPlotCube.DefaultTag);
        }

        #endregion

        #region Nested type: ILPlotCubeConverter

        private class ILPlotCubeConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILPlotCubeWrapper)
                    return ((ILPlotCubeWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
