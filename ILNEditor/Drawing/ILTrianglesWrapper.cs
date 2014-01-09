﻿using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILTrianglesConverter))]
    public class ILTrianglesWrapper : ILShapeWrapper
    {
        private readonly ILTriangles source;

        public ILTrianglesWrapper(ILTriangles source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Triangles"), label)
        {
            this.source = source;
        }

        #region Nested type: ILTrianglesConverter

        private class ILTrianglesConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILTrianglesWrapper)
                {
                    var triangles = (ILTrianglesWrapper) value;
                    string specularColor = triangles.SpecularColor.IsKnownColor ? triangles.SpecularColor.ToKnownColor().ToString() : triangles.SpecularColor.ToString();
                    string emissionColor = triangles.SpecularColor.IsKnownColor ? triangles.SpecularColor.ToKnownColor().ToString() : triangles.SpecularColor.ToString();

                    return String.Format("{0} ({1}, {2})", triangles.Label, specularColor, emissionColor);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
