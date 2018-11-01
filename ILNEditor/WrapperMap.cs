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
            Add(typeof(Group), typeof(GroupWrapper));
            Add(typeof(Camera), typeof(CameraWrapper));
            Add(typeof(PointLight), typeof(PointLightWrapper));
            Add(typeof(ScreenObject), typeof(ScreenObjectWrapper));

            // Shapes
            Add(typeof(Points), typeof(PointsWrapper));
            Add(typeof(Lines), typeof(LinesWrapper));
            Add(typeof(LineStrip), typeof(LinesWrapper));
            Add(typeof(Triangles), typeof(TrianglesWrapper));
            Add(typeof(TrianglesFan), typeof(TrianglesWrapper));
            Add(typeof(TrianglesStrip), typeof(TrianglesWrapper));

            // Elements
            Add(typeof(Label), typeof(LabelWrapper));
            Add(typeof(Circle), typeof(CircleWrapper));
            Add(typeof(Cone), typeof(ConeWrapper));
            Add(typeof(Cylinder), typeof(CylinderWrapper));
            Add(typeof(Sphere), typeof(SphereWrapper));
            Add(typeof(Gear), typeof(GearWrapper));

            // Plotting
            Add(typeof(PlotCube), typeof(PlotCubeWrapper));
            Add(typeof(PlotCubeScaleGroup), typeof(PlotCubeScaleGroupWrapper));
            Add(typeof(PlotCubeDataGroup), typeof(PlotCubeDataGroupWrapper));
            Add(typeof(Axis), typeof(AxisWrapper));
            Add(typeof(AxisCollection), typeof(AxisCollectionWrapper));
            Add(typeof(Limits), typeof(LimitsWrapper));
            Add(typeof(ScaleModes), typeof(ScaleModesWrapper));
            Add(typeof(Tick), typeof(TickWrapper));
            Add(typeof(TickCollection), typeof(TickCollectionWrapper));
            Add(typeof(Legend), typeof(LegendWrapper));
            Add(typeof(LegendItem), typeof(LegendItemWrapper));
            Add(typeof(SelectionRectangle), typeof(SelectionRectangleWrapper));
            Add(typeof(Marker), typeof(MarkerWrapper));

            // PlotTypes
            Add(typeof(LinePlot), typeof(LinePlotWrapper));
            Add(typeof(Surface), typeof(SurfaceWrapper));
        }
    }
}
