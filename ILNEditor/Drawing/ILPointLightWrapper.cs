using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using ILNEditor.TypeConverters;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class ILPointLightWrapper : ILNodeWrapper
    {
        private readonly ILPointLight source;

        public ILPointLightWrapper(ILPointLight source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILPointLight.DefaultPointLightTag), label)
        {
            this.source = source;
        }

        #region ILPointLight

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
