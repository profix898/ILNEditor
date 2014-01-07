using System;
using System.Collections.Generic;
using ILNEditor.Drawing;
using ILNEditor.Drawing.Plotting;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor
{
    public class WrapperMap : Dictionary<Type, Type>
    {
        public WrapperMap()
        {
            AddDefaultMapping();
        }

        public WrapperMap(IDictionary<Type, Type> dictionary)
            : base(dictionary)
        {
        }

        private void AddDefaultMapping()
        {
            Add(typeof(ILCamera), typeof(ILCameraWrapper));
            Add(typeof(ILGroup), typeof(ILGroupWrapper));
            Add(typeof(ILScreenObject), typeof(ILScreenObjectWrapper));
            Add(typeof(ILTriangles), typeof(ILTrianglesWrapper));

            // Drawing
            Add(typeof(ILLabel), typeof(ILLabelWrapper));
            Add(typeof(ILLines), typeof(ILLinesWrapper));
            Add(typeof(ILSphere), typeof(ILSphereWrapper));

            // Plotting
            Add(typeof(ILPlotCube), typeof(ILPlotCubeWrapper));
            Add(typeof(ILAxis), typeof(ILAxisWrapper));
            Add(typeof(ILAxisCollection), typeof(ILAxisCollectionWrapper));
            Add(typeof(ILLimits), typeof(ILLimitsWrapper));
            Add(typeof(ILScaleModes), typeof(ILScaleModesWrapper));
            Add(typeof(ILTick), typeof(ILTickWrapper));
            Add(typeof(ILTickCollection), typeof(ILTickCollectionWrapper));
            Add(typeof(ILMarker), typeof(ILMarkerWrapper));
            Add(typeof(ILLegend), typeof(ILLegendWrapper));
            Add(typeof(ILLegendItem), typeof(ILLegendItemWrapper));

            // Plot Types
            Add(typeof(ILLinePlot), typeof(ILLinePlotWrapper));
            Add(typeof(ILSurface), typeof(ILSurfaceWrapper));
        }
    }
}
