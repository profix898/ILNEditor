using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ILNumerics.Drawing;

namespace ILNEditor
{
    public class ILPanelEditor
    {
        private readonly ILPanel ilPanel;
        private readonly ILNumerics.Drawing.Plotting.ILPlotCube plotCube;

        private ObjectPropertyForm propertyForm;

        public ILPanelEditor(ILPanel ilPanel)
        {
            if (!ilPanel.Scene.Find<ILNumerics.Drawing.Plotting.ILPlotCube>().Any())
                throw new ArgumentException("ILPanel does not contain a ILPlotCube instance.", "ilPanel");

            this.ilPanel = ilPanel;

            plotCube = ilPanel.Scene.First<ILNumerics.Drawing.Plotting.ILPlotCube>();

            plotCube.MouseClick += PlotCube_MouseClick;
            plotCube.MouseDoubleClick += (sender, args) =>
            {
                if (!args.DirectionUp)
                    return;

                MouseDoubleClickPropertyForm(new ILPlotCube(plotCube), "PlotCube", args);
            };

            plotCube.Axes.XAxis.MouseDoubleClick += (sender, args) => MouseDoubleClickPropertyForm(new ILAxis(plotCube.Axes.XAxis), "X Axis", args);
            plotCube.Axes.YAxis.MouseDoubleClick += (sender, args) => MouseDoubleClickPropertyForm(new ILAxis(plotCube.Axes.YAxis), "Y Axis", args);
            plotCube.Axes.ZAxis.MouseDoubleClick += (sender, args) => MouseDoubleClickPropertyForm(new ILAxis(plotCube.Axes.ZAxis), "Z Axis", args);

            foreach (ILNumerics.Drawing.Plotting.ILLinePlot linePlot in ilPanel.Scene.Find<ILNumerics.Drawing.Plotting.ILLinePlot>())
            {
                var linePlotClosure = new ILLinePlot(linePlot);
                linePlot.MouseDoubleClick += (sender, args) => MouseDoubleClickPropertyForm(linePlotClosure, "LinePlot", args);
            }
        }

        private void PlotCube_MouseClick(object sender, ILMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Reset View", (o, args) =>
            {
                ilPanel.SceneSyncRoot.First<ILNumerics.Drawing.Plotting.ILPlotCube>().Reset();
                ilPanel.Refresh();
            });
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("X-Y Plane", (o, args) =>
            {
                ilPanel.SceneSyncRoot.First<ILNumerics.Drawing.Plotting.ILPlotCube>().Rotation = Matrix4.Identity;
                ilPanel.Refresh();
            });
            contextMenu.MenuItems.Add("X-Z Plane", (o, args) =>
            {
                ilPanel.SceneSyncRoot.First<ILNumerics.Drawing.Plotting.ILPlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitX, Math.PI / 2.0);
                ilPanel.Refresh();
            });
            contextMenu.MenuItems.Add("Y-Z Plane", (o, args) =>
            {
                ilPanel.SceneSyncRoot.First<ILNumerics.Drawing.Plotting.ILPlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitY, Math.PI / 2.0);
                ilPanel.Refresh();
            });

            contextMenu.Show(ilPanel, e.Location);

            e.Cancel = true;
        }

        private void MouseDoubleClickPropertyForm(object sender, string label, ILMouseEventArgs e)
        {
            if (propertyForm != null)
                propertyForm.Dispose();

            propertyForm = new ObjectPropertyForm(sender, label);
            propertyForm.Closed += (o, args) => ilPanel.Refresh();
            propertyForm.Show();

            e.Cancel = true;
        }

        public static void AttachTo(ILPanel ilPanel)
        {
            new ILPanelEditor(ilPanel);
        }

        #region HullTypes

        #region ILAxis

        #region Nested type: ILAxis

        [TypeConverter(typeof(ILAxisConverter))]
        public class ILAxis : ILNode
        {
            private readonly ILNumerics.Drawing.Plotting.ILAxis source;

            public ILAxis(ILNumerics.Drawing.Plotting.ILAxis source)
                : base(source)
            {
                this.source = source;
            }

            #region ILAxis

            [Category("Label")]
            [TypeConverter(typeof(PointFConverter))]
            public PointF? LabelAnchor
            {
                get { return source.LabelAnchor; }
                set { source.LabelAnchor = value; }
            }

            [Category("Label")]
            [TypeConverter(typeof(PointFConverter))]
            public PointF? ScaleLabelAnchor
            {
                get { return source.ScaleLabelAnchor; }
                set { source.ScaleLabelAnchor = value; }
            }

            [Category("Label")]
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3? LabelPosition
            {
                get { return source.LabelPosition; }
                set { source.LabelPosition = value; }
            }

            [Category("Label")]
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3? ScaleLabelPosition
            {
                get { return source.ScaleLabelPosition; }
                set { source.ScaleLabelPosition = value; }
            }

            [Category("Axis")]
            public AxisNames AxisName
            {
                get { return source.AxisName; }
                set { source.AxisName = value; }
            }

            [Category("Axis")]
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3? Position
            {
                get { return source.Position; }
                set { source.Position = value; }
            }

            [Category("Axis")]
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 Direction
            {
                get { return source.Direction; }
                set { source.Direction = value; }
            }

            [Category("Axis")]
            public float? Min
            {
                get { return source.Min; }
                set { source.Min = value; }
            }

            [Category("Axis")]
            public float? Max
            {
                get { return source.Max; }
                set { source.Max = value; }
            }

            [Category("Label")]
            public ILLabel Label
            {
                get { return new ILLabel(source.Label); }
            }

            [Category("Label")]
            public ILLabel ScaleLabel
            {
                get { return new ILLabel(source.ScaleLabel); }
            }

            [Category("Axis")]
            public ILTickCollection Ticks
            {
                get { return new ILTickCollection(source.Ticks); }
            }

            [Category("Grid")]
            public ILLines GridMajor
            {
                get { return new ILLines(source.GridMajor); }
            }

            [Category("Grid")]
            public ILLines GridMinor
            {
                get { return new ILLines(source.GridMinor); }
            }

            #endregion
        }

        #endregion

        #region Nested type: ILAxisConverter

        internal class ILAxisConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILAxis)
                {
                    var axis = (ILAxis) value;

                    return String.Format("Axis ({0})", axis.AxisName);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #endregion

        #region ILAxisCollection

        #region Nested type: ILAxisCollection

        [TypeConverter(typeof(ILAxisCollectionConverter))]
        public class ILAxisCollection
        {
            private readonly ILNumerics.Drawing.Plotting.ILAxisCollection source;

            public ILAxisCollection(ILNumerics.Drawing.Plotting.ILAxisCollection source)
            {
                this.source = source;
            }

            #region ILPlotCube

            [Category("Axis")]
            public ILAxis XAxis
            {
                get { return new ILAxis(source.XAxis); }
            }

            [Category("Axis")]
            public ILAxis YAxis
            {
                get { return new ILAxis(source.YAxis); }
            }

            [Category("Axis")]
            public ILAxis ZAxis
            {
                get { return new ILAxis(source.ZAxis); }
            }

            #endregion
        }

        #endregion

        #region Nested type: ILAxisCollectionConverter

        internal class ILAxisCollectionConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILAxisCollection)
                    return "Axis Collection";

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #endregion

        #region ILDrawable

        public class ILDrawable : ILNode
        {
            private readonly ILNumerics.Drawing.ILDrawable source;

            public ILDrawable(ILNumerics.Drawing.ILDrawable source)
                : base(source)
            {
                this.source = source;
            }

            #region ILDrawable

            [Category("Format")]
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            public Color? Color
            {
                get { return source.Color; }
                set { source.Color = value; }
            }

            #endregion
        }

        #endregion

        #region ILLabel

        #region Nested type: ILLabel

        [TypeConverter(typeof(ILLabelConverter))]
        public class ILLabel : ILDrawable
        {
            private readonly ILNumerics.Drawing.ILLabel source;

            public ILLabel(ILNumerics.Drawing.ILLabel source)
                : base(source)
            {
                this.source = source;
            }

            #region ILLines

            [Category("Label")]
            public double Rotation
            {
                get { return source.Rotation; }
                set { source.Rotation = value; }
            }

            [Category("Label")]
            public string Text
            {
                get { return source.Text; }
                set { source.Text = value; }
            }

            [Category("Label")]
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 Position
            {
                get { return source.Position; }
                set { source.Position = value; }
            }

            [Category("Label")]
            [TypeConverter(typeof(PointFConverter))]
            public PointF Anchor
            {
                get { return source.Anchor; }
                set { source.Anchor = value; }
            }

            [Category("Label")]
            public Font Font
            {
                get { return source.Font; }
                set { source.Font = value; }
            }

            #endregion
        }

        #endregion

        #region Nested type: ILLabelConverter

        internal class ILLabelConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLabel)
                {
                    var label = (ILLabel) value;
                    string color = label.Color.HasValue ? (label.Color.Value.IsKnownColor ? label.Color.Value.ToKnownColor().ToString() : label.Color.Value.ToString()) : "";

                    return String.Format("Label ({0}, {1} {2}pt, {3})", label.Text, label.Font.Name, (int) label.Font.SizeInPoints, color);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #endregion

        #region ILLimits

        #region Nested type: ILLimits

        [TypeConverter(typeof(ILLimitsConverter))]
        public class ILLimits
        {
            private readonly ILNumerics.Drawing.ILLimits source;

            public ILLimits(ILNumerics.Drawing.ILLimits source)
            {
                this.source = source;
            }

            #region ILLimits

            [Category("Limits")]
            public float XMin
            {
                get { return source.XMin; }
                set { source.XMin = value; }
            }

            [Category("Limits")]
            public float YMin
            {
                get { return source.YMin; }
                set { source.YMin = value; }
            }

            [Category("Limits")]
            public float ZMin
            {
                get { return source.ZMin; }
                set { source.ZMin = value; }
            }

            [Category("Limits")]
            public float XMax
            {
                get { return source.XMax; }
                set { source.XMax = value; }
            }

            [Category("Limits")]
            public float YMax
            {
                get { return source.YMax; }
                set { source.YMax = value; }
            }

            [Category("Limits")]
            public float ZMax
            {
                get { return source.ZMax; }
                set { source.ZMax = value; }
            }

            [Category("Behavior")]
            public bool AllowZeroVolume
            {
                get { return source.AllowZeroVolume; }
                set { source.AllowZeroVolume = value; }
            }

            #endregion
        }

        #endregion

        #region Nested type: ILLimitsConverter

        internal class ILLimitsConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLimits)
                {
                    var limits = (ILLimits) value;

                    return String.Format("Limits (X:{0:F}/{1:F}, Y:{2:F}/{3:F}, Z:{4:F}/{5:F})", limits.XMin, limits.XMax, limits.YMin, limits.YMax, limits.ZMin, limits.ZMax);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #endregion

        #region ILLinePlot

        #region Nested type: ILLinePlot

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ILLinePlot
        {
            private readonly ILNumerics.Drawing.Plotting.ILLinePlot source;

            public ILLinePlot(ILNumerics.Drawing.Plotting.ILLinePlot source)
            {
                this.source = source;
            }

            [Category("Format")]
            public ILLines Line
            {
                get { return new ILLines(source.Line); }
            }

            [Category("Format")]
            public ILMarker Marker
            {
                get { return new ILMarker(source.Marker); }
            }

            [Category("Positions")]
            [TypeConverter(typeof(PositionsConverter))]
            public ReadOnlyCollection<float> Positions
            {
                get { return new ReadOnlyCollection<float>(source.Positions.ToList()); }
            }
        }

        #endregion

        #region Nested type: PositionsConverter

        internal class PositionsConverter : ExpandableObjectConverter
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

        #endregion

        #region ILLines

        #region Nested type: ILLines

        [TypeConverter(typeof(ILLinesConverter))]
        public class ILLines : ILDrawable
        {
            private readonly ILNumerics.Drawing.ILLines source;

            public ILLines(ILNumerics.Drawing.ILLines source)
                : base(source)
            {
                this.source = source;
            }

            #region ILLines

            [Category("Format")]
            public int Width
            {
                get { return source.Width; }
                set { source.Width = value; }
            }

            [Category("Format")]
            public DashStyle DashStyle
            {
                get { return source.DashStyle; }
                set { source.DashStyle = value; }
            }

            [Category("Format")]
            public short Pattern
            {
                get { return source.Pattern; }
                set { source.Pattern = value; }
            }

            [Category("Format")]
            public float PatternScale
            {
                get { return source.PatternScale; }
                set { source.PatternScale = value; }
            }

            [Category("Format")]
            public bool Antialiasing
            {
                get { return source.Antialiasing; }
                set { source.Antialiasing = value; }
            }

            #endregion
        }

        #endregion

        #region Nested type: ILLinesConverter

        internal class ILLinesConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLines)
                {
                    var lines = (ILLines) value;
                    string color = lines.Color.HasValue ? (lines.Color.Value.IsKnownColor ? lines.Color.Value.ToKnownColor().ToString() : lines.Color.Value.ToString()) : "";

                    return String.Format("Line ({0}, {1}, {2})", color, lines.DashStyle, lines.Width);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #endregion

        #region ILMarker

        #region Nested type: ILMarker

        [TypeConverter(typeof(ILMarkerConverter))]
        public class ILMarker
        {
            private readonly ILNumerics.Drawing.Plotting.ILMarker source;

            public ILMarker(ILNumerics.Drawing.Plotting.ILMarker source)
            {
                this.source = source;
            }

            [Category("Marker")]
            public int Size
            {
                get { return source.Size; }
                set { source.Size = value; }
            }

            [Category("Marker")]
            public Vector3? Position
            {
                get { return source.Position; }
                set { source.Position = value; }
            }

            [Category("Marker")]
            public ILLines Border
            {
                get { return new ILLines(source.Border); }
            }

            [Category("Marker")]
            public MarkerStyle Style
            {
                get { return source.Style; }
                set { source.Style = value; }
            }
        }

        #endregion

        #region Nested type: ILMarkerConverter

        internal class ILMarkerConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILMarker)
                {
                    var marker = (ILMarker) value;

                    return String.Format("Marker ({0}, {1})", marker.Style, marker.Size);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #endregion

        #region ILNode

        public class ILNode
        {
            private readonly ILNumerics.Drawing.ILNode source;

            public ILNode(ILNumerics.Drawing.ILNode source)
            {
                this.source = source;
            }

            #region ILNode

            [Category("Format")]
            public bool Visible
            {
                get { return source.Visible; }
                set { source.Visible = value; }
            }

            [Category("Format")]
            public bool Markable
            {
                get { return source.Markable; }
                set { source.Markable = value; }
            }

            #endregion
        }

        #endregion

        #region ILPlotCube

        public class ILPlotCube
        {
            private readonly ILNumerics.Drawing.Plotting.ILPlotCube source;

            public ILPlotCube(ILNumerics.Drawing.Plotting.ILPlotCube source)
            {
                this.source = source;
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
            public ILAxisCollection Axes
            {
                get { return new ILAxisCollection(source.Axes); }
            }

            [Category("Format")]
            public ILScaleModes ScaleModes
            {
                get { return new ILScaleModes(source.ScaleModes); }
            }

            [Category("Format")]
            public ILLimits Limits
            {
                get { return new ILLimits(source.Limits); }
            }

            [Category("Appearance")]
            public ILLines Lines
            {
                get { return new ILLines(source.Lines); }
            }

            [Category("Appearance")]
            public Projection Projection
            {
                get { return source.Projection; }
                set { source.Projection = value; }
            }

            #endregion
        }

        #endregion

        #region ILScaleModes

        #region Nested type: ILScaleModes

        [TypeConverter(typeof(ILScaleModesConverter))]
        public class ILScaleModes
        {
            private readonly ILNumerics.Drawing.Plotting.ILScaleModes source;

            public ILScaleModes(ILNumerics.Drawing.Plotting.ILScaleModes source)
            {
                this.source = source;
            }

            #region ILScaleModes

            [Category("Format")]
            public AxisScale XAxisScale
            {
                get { return source.XAxisScale; }
                set { source.XAxisScale = value; }
            }

            [Category("Format")]
            public AxisScale YAxisScale
            {
                get { return source.YAxisScale; }
                set { source.YAxisScale = value; }
            }

            [Category("Format")]
            public AxisScale ZAxisScale
            {
                get { return source.ZAxisScale; }
                set { source.ZAxisScale = value; }
            }

            #endregion
        }

        #endregion

        #region Nested type: ILScaleModesConverter

        internal class ILScaleModesConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILScaleModes)
                {
                    var scaleModes = (ILScaleModes) value;

                    return String.Format("ScaleModes (X:{0}, Y:{1}, Z:{2})", scaleModes.XAxisScale, scaleModes.YAxisScale, scaleModes.ZAxisScale);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #endregion

        #region ILTick

        public class ILTick
        {
            private readonly ILNumerics.Drawing.Plotting.ILTick source;

            public ILTick(ILNumerics.Drawing.Plotting.ILTick source)
            {
                this.source = source;
            }

            #region ILTick

            [Category("Format")]
            public float Position
            {
                get { return source.Position; }
                set { source.Position = value; }
            }

            [Category("Format")]
            public ILLabel Label
            {
                get { return new ILLabel(source.Label); }
            }

            #endregion
        }

        #endregion

        #region ILTickCollection

        #region Nested type: ILTickCollection

        [TypeConverter(typeof(ILTickCollectionConverter))]
        public class ILTickCollection
        {
            private readonly ILNumerics.Drawing.Plotting.ILTickCollection source;

            public ILTickCollection(ILNumerics.Drawing.Plotting.ILTickCollection source)
            {
                this.source = source;
            }

            #region ILTickCollection

            [Category("Format")]
            public int MaxNumberDigitsShowFull
            {
                get { return source.MaxNumberDigitsShowFull; }
                set { source.MaxNumberDigitsShowFull = value; }
            }

            [Category("Format")]
            [TypeConverter(typeof(SizeFConverter))]
            public SizeF DefaultTickLabelSize
            {
                get { return source.DefaultTickLabelSize; }
                set { source.DefaultTickLabelSize = value; }
            }

            [Category("Format")]
            public Font DefaultFont
            {
                get { return source.DefaultFont; }
                set { source.DefaultFont = value; }
            }

            [Category("Ticks")]
            public ReadOnlyCollection<ILTick> Ticks
            {
                get { return new ReadOnlyCollection<ILTick>(((IEnumerable<ILNumerics.Drawing.Plotting.ILTick>) source).Select(tick => new ILTick(tick)).ToList()); }
            }

            [Category("Format")]
            public Color Color
            {
                get { return source.Color; }
                set { source.Color = value; }
            }

            [Category("Format")]
            public int Width
            {
                get { return source.Width; }
                set { source.Width = value; }
            }

            [Category("Format")]
            public float TickLength
            {
                get { return source.TickLength; }
                set { source.TickLength = value; }
            }

            #endregion
        }

        #endregion

        #region Nested type: ILTickCollectionConverter

        internal class ILTickCollectionConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILTickCollection)
                    return "Ticks";

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #endregion

        #endregion

        #region TypeConverters

        #region PointFConverter

        public class PointFConverter : PointConverter
        {
            #region Overrides of PointConverter

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(String))
                    return true;

                return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(String))
                    return true;

                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                {
                    try
                    {
                        Match match = new Regex("X=([\\d.,-]+); Y=([\\d.,-]+)").Match(((string) value).Trim());
                        if (match.Success)
                            return new PointF(Single.Parse(match.Groups[1].Value), Single.Parse(match.Groups[2].Value));
                    }
                    catch
                    {
                        throw new ArgumentException(String.Format("Can not convert '{0}' to type PointF.", value));
                    }
                }

                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(String) && value is PointF)
                {
                    var point = (PointF) value;

                    return String.Format("X={0:F}; Y={1:F}", point.X, point.Y);
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
            {
                if (propertyValues == null)
                    throw new ArgumentNullException("propertyValues");

                object x = propertyValues["X"];
                object y = propertyValues["Y"];

                if (x != null && y != null && x is float && y is float)
                    return new PointF((float) x, (float) y);

                throw new Exception("PropertyValue Invalid Entry");
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(typeof(PointF), attributes).Sort(new[] { "X", "Y" });
            }

            #endregion
        }

        #endregion

        #region Vector3Converter

        public class Vector3Converter : PointConverter
        {
            #region Overrides of PointConverter

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(String))
                    return true;

                return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(String))
                    return true;

                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                {
                    try
                    {
                        Match match = new Regex("X=([\\d.,-]+); Y=([\\d.,-]+); Z=([\\d.,-]+)").Match(((string) value).Trim());
                        if (match.Success)
                            return new Vector3(Single.Parse(match.Groups[1].Value), Single.Parse(match.Groups[2].Value), Single.Parse(match.Groups[3].Value));
                    }
                    catch
                    {
                        throw new ArgumentException(String.Format("Can not convert '{0}' to type Vector3.", value));
                    }
                }

                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(String) && value is Vector3)
                {
                    var vector = (Vector3) value;

                    return String.Format("X={0:F}; Y={1:F}; Z={2:F}", vector.X, vector.Y, vector.Z);
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
            {
                if (propertyValues == null)
                    throw new ArgumentNullException("propertyValues");

                object x = propertyValues["X"];
                object y = propertyValues["Y"];
                object z = propertyValues["Z"];

                if (x != null && y != null && z != null && x is float && y is float && z is float)
                    return new Vector3((float) x, (float) y, (float) z);

                throw new Exception("PropertyValue Invalid Entry");
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Vector3), attributes).Sort(new[] { "X", "Y", "Z" });
                properties.Remove(properties.Find("Length", true));
                properties.Remove(properties.Find("LengthFast", true));
                properties.Remove(properties.Find("LengthSquared", true));

                return properties;
            }

            #endregion
        }

        #endregion

        #endregion
    }
}
