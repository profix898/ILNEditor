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
            Add(typeof(ILGroup), typeof(ILGroupWrapper));
            Add(typeof(ILShape), typeof(ILShapeWrapper));
            Add(typeof(ILScreenObject), typeof(ILScreenObjectWrapper));

            // Drawing
            Add(typeof(ILLabel), typeof(ILLabelWrapper));
            Add(typeof(ILLines), typeof(ILLinesWrapper));

            // Plotting
            Add(typeof(ILPlotCube), typeof(ILPlotCubeWrapper));
            Add(typeof(ILAxis), typeof(ILAxisWrapper));
            Add(typeof(ILAxisCollection), typeof(ILAxisCollectionWrapper));
            Add(typeof(ILLimits), typeof(ILLimitsWrapper));
            Add(typeof(ILScaleModes), typeof(ILScaleModesWrapper));
            Add(typeof(ILTick), typeof(ILTickWrapper));
            Add(typeof(ILTickCollection), typeof(ILTickCollectionWrapper));
            Add(typeof(ILMarker), typeof(ILMarkerWrapper));
            //Add(typeof(ILLegendItem), typeof(ILLegendItemWrapper));
            Add(typeof(ILLegend), typeof(ILLegendWrapper));

            // Plot Types
            Add(typeof(ILLinePlot), typeof(ILLinePlotWrapper));
            Add(typeof(ILSurface), typeof(ILSurfaceWrapper));
        }
    }
}
