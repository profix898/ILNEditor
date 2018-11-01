using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using ILNEditor.TypeConverters;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PointLightWrapper : NodeWrapper
    {
        private readonly PointLight source;

        public PointLightWrapper(PointLight source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, PointLight.DefaultPointLightTag), label)
        {
            this.source = source;
        }

        #region PointLight

        [Category("Light")]
        public float Intensity
        {
            get { return source.Intensity; }
            set { source.Intensity = value; }
        }

        [Category("Light")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Position
        {
            get { return source.Position; }
            set { source.Position = value; }
        }

        [Category("Light")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color Color
        {
            get { return source.Color; }
            set { source.Color = value; }
        }

        #endregion
    }
}
