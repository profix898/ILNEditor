using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(TrianglesConverter))]
    public class TrianglesWrapper : ShapeWrapper
    {
        private readonly Triangles source;

        public TrianglesWrapper(Triangles source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Triangles"), label)
        {
            this.source = source;
        }

        #region Nested type: TrianglesConverter

        private class TrianglesConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is TrianglesWrapper)
                {
                    var triangles = (TrianglesWrapper) value;
                    string specularColor = triangles.SpecularColor.IsKnownColor ? triangles.SpecularColor.ToKnownColor().ToString() : triangles.SpecularColor.ToString();
                    string emissionColor = triangles.SpecularColor.IsKnownColor ? triangles.SpecularColor.ToKnownColor().ToString() : triangles.SpecularColor.ToString();

                    return $"{triangles.Label} ({specularColor}, {emissionColor})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
