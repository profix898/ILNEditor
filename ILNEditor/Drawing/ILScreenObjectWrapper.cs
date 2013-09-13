using System;
using System.ComponentModel;
using System.Drawing;
using ILNEditor.TypeConverters;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class ILScreenObjectWrapper : ILGroupWrapper
    {
        private readonly ILLinesWrapper border;
        private readonly ILScreenObject source;

        public ILScreenObjectWrapper(ILScreenObject source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "ScreenObject" : name)
        {
            this.source = source;

            border = new ILLinesWrapper(source.Border, editor, FullName, "Border");
        }

        #region ILScreenObject

        [Category("Format")]
        public float ZCoord
        {
            get { return source.ZCoord; }
            set { source.ZCoord = value; }
        }

        [Category("Format")]
        public bool Movable
        {
            get { return source.Movable; }
            set { source.Movable = value; }
        }

        [Category("Format")]
        public ILLinesWrapper Border
        {
            get { return border; }
        }

        [Category("Format")]
        [TypeConverter(typeof(PointFConverter))]
        public PointF Anchor
        {
            get { return source.Anchor; }
            set { source.Anchor = value; }
        }

        [Category("Format")]
        [TypeConverter(typeof(PointFConverter))]
        public PointF Location
        {
            get { return source.Location; }
            set { source.Location = value; }
        }

        [Category("Format")]
        public float? Width
        {
            get { return source.Width; }
            set { source.Width = value; }
        }

        [Category("Format")]
        public float? Height
        {
            get { return source.Height; }
            set { source.Height = value; }
        }

        #endregion
    }
}
