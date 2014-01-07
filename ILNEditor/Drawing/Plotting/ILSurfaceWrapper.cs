using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILSurfaceConverter))]
    internal class ILSurfaceWrapper : ILGroupWrapper
    {
        private readonly ILTrianglesWrapper fill;
        private readonly ReadOnlyCollection<float> positions;
        private readonly ILSurface source;
        private readonly ILLinesWrapper wireframe;

        public ILSurfaceWrapper(ILSurface source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? GetSurfaceLabelFromLegend(source, editor.Panel) : name)
        {
            this.source = editor.Panel.SceneSyncRoot.FindById<ILSurface>(source.ID);

            fill = new ILTrianglesWrapper(source.Fill, editor, FullName, ILSurface.FillTag);
            wireframe = new ILLinesWrapper(source.Wireframe, editor, FullName, ILSurface.WireframeTag);
            positions = new ReadOnlyCollection<float>(source.Positions.ToList());
        }

        #region ILSurface

        [Category("Format")]
        public ILSurface.ColorModes ColorMode
        {
            get { return source.ColorMode; }
            set { source.ColorMode = value; }
        }

        [Category("Format")]
        public Colormaps Colormap
        {
            get { return source.Colormap.Type; }
            set { source.Colormap = new ILColormap(value); }
        }

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

        [Category("Rendering")]
        public bool? UseLighting
        {
            get { return source.UseLighting; }
            set { source.UseLighting = value; }
        }

        [Category("Positions")]
        [TypeConverter(typeof(PositionsConverter))]
        public ReadOnlyCollection<float> Positions
        {
            // TODO: Make this editable
            get { return positions; }
        }

        #endregion

        #region Helper

        private static string GetSurfaceLabelFromLegend(ILSurface source, ILPanel panel)
        {
            // Find index of ILLinePlot
            var plotCube = panel.Scene.First<ILPlotCube>();
            IEnumerable<ILSurface> surfaces = plotCube.Find<ILSurface>();
            int index = surfaces.TakeWhile(surface => surface != source).Count();

            var legend = panel.Scene.First<ILLegend>();
            if (legend != null)
            {
                // Get text from ILLegendItem at the index
                if (legend.Items.Children.Count() > index)
                    return String.Format("{0}(\"{1}\")", ILSurface.SurfaceDefaultTag, legend.Items.Find<ILLegendItem>().ElementAt(index).Text);
            }

            return String.Format("{0}#{1}", ILSurface.SurfaceDefaultTag, index + 1);
        }

        #endregion

        #region Overrides of ILGroupWrapper

        internal override void Traverse()
        {
            // Do not traverse children
        }

        #endregion

        #region Nested type: ILSurfaceConverter

        private class ILSurfaceConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILSurfaceWrapper)
                    return ((ILSurfaceWrapper) value).Name;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #region Nested type: PositionsConverter

        private class PositionsConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ReadOnlyCollection<float>)
                {
                    var positions = (ReadOnlyCollection<float>) value;

                    return String.Format("Positions (N = {0})", positions.Count);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
