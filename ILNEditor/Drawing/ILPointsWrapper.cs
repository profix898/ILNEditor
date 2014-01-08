using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILPointsConverter))]
    internal class ILPointsWrapper : ILShapeWrapper
    {
        private readonly ILPoints source;

        private bool disposed;

        public ILPointsWrapper(ILPoints source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Points"), label)
        {
            // Shape needs to be accessed from SceneSyncRoot (instead of Scene)
            this.source = editor.Panel.SceneSyncRoot.FindById<ILPoints>(source.ID);

            this.source.MouseDoubleClick += OnMouseDoubleClick;
        }

        #region ILPoints

        [Category("Points")]
        public float Size
        {
            get { return source.Size; }
            set { source.Size = value; }
        }

        #endregion

        #region Overrides of ILWrapperBase

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    source.MouseDoubleClick -= OnMouseDoubleClick;

                disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Nested type: ILPointsConverter

        private class ILPointsConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILPointsWrapper)
                {
                    var points = (ILPointsWrapper) value;
                    string specularColor = points.SpecularColor.IsKnownColor ? points.SpecularColor.ToKnownColor().ToString() : points.SpecularColor.ToString();
                    string emissionColor = points.SpecularColor.IsKnownColor ? points.SpecularColor.ToKnownColor().ToString() : points.SpecularColor.ToString();

                    return String.Format("{0} ({1}, {2}, {3})", points.Label, specularColor, emissionColor, points.Size);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
