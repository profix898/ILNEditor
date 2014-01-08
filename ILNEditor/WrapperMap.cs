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
            // Drawing
            Add(typeof(ILGroup), typeof(ILGroupWrapper));
            Add(typeof(ILCamera), typeof(ILCameraWrapper));
            Add(typeof(ILPointLight), typeof(ILPointLightWrapper));
            Add(typeof(ILScreenObject), typeof(ILScreenObjectWrapper));

            // Shapes
            Add(typeof(ILPoints), typeof(ILPointsWrapper));
            Add(typeof(ILLines), typeof(ILLinesWrapper));
            Add(typeof(ILLineStrip), typeof(ILLinesWrapper));
            Add(typeof(ILTriangles), typeof(ILTrianglesWrapper));
            Add(typeof(ILTrianglesFan), typeof(ILTrianglesWrapper));
            Add(typeof(ILTrianglesStrip), typeof(ILTrianglesWrapper));

            // Elements
            Add(typeof(ILLabel), typeof(ILLabelWrapper));
            Add(typeof(ILCircle), typeof(ILCircleWrapper));
            Add(typeof(ILCone), typeof(ILConeWrapper));
            Add(typeof(ILCylinder), typeof(ILCylinderWrapper));
            Add(typeof(ILSphere), typeof(ILSphereWrapper));
            Add(typeof(ILGear), typeof(ILGearWrapper));

            // Plotting
            Add(typeof(ILPlotCube), typeof(ILPlotCubeWrapper));
            Add(typeof(ILPlotCubeScaleGroup), typeof(ILPlotCubeScaleGroupWrapper));
            Add(typeof(ILPlotCubeDataGroup), typeof(ILPlotCubeDataGroupWrapper));
            Add(typeof(ILAxis), typeof(ILAxisWrapper));
            Add(typeof(ILAxisCollection), typeof(ILAxisCollectionWrapper));
            Add(typeof(ILLimits), typeof(ILLimitsWrapper));
            Add(typeof(ILScaleModes), typeof(ILScaleModesWrapper));
            Add(typeof(ILTick), typeof(ILTickWrapper));
            Add(typeof(ILTickCollection), typeof(ILTickCollectionWrapper));
            Add(typeof(ILLegend), typeof(ILLegendWrapper));
            Add(typeof(ILLegendItem), typeof(ILLegendItemWrapper));
            Add(typeof(ILSelectionRectangle), typeof(ILSelectionRectangleWrapper));
            Add(typeof(ILMarker), typeof(ILMarkerWrapper));

            // PlotTypes
            Add(typeof(ILLinePlot), typeof(ILLinePlotWrapper));
            Add(typeof(ILSurface), typeof(ILSurfaceWrapper));
        }
    }
}
