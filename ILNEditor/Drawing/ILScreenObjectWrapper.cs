using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using ILNEditor.TypeConverters;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class ILScreenObjectWrapper : ILGroupWrapper
    {
        private readonly ILTrianglesWrapper background;
        private readonly ILLinesWrapper border;
        private readonly ILScreenObject source;

        public ILScreenObjectWrapper(ILScreenObject source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "ScreenObject"), label)
        {
            this.source = source;

            border = new ILLinesWrapper(source.Border, editor, Path, ILScreenObject.BorderTag, "Border");
            background = new ILTrianglesWrapper(source.Background, editor, Path, ILScreenObject.BackgroundTag, "Background");
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
        public ILTrianglesWrapper Background
        {
            get { return background; }
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

        [Category("Format")]
        [TypeConverter(typeof(SizeFConverter))]
        public SizeF MinimumSize
        {
            get { return source.MinimumSize; }
            set { source.MinimumSize = value; }
        }

        [Category("Format")]
        public Units LocationXUnit
        {
            get { return source.LocationXUnit; }
            set { source.LocationXUnit = value; }
        }

        [Category("Format")]
        public Units LocationYUnit
        {
            get { return source.LocationYUnit; }
            set { source.LocationYUnit = value; }
        }

        [Category("Format")]
        public Units WidthUnit
        {
            get { return source.WidthUnit; }
            set { source.WidthUnit = value; }
        }

        [Category("Format")]
        public Units HeightUnit
        {
            get { return source.HeightUnit; }
            set { source.HeightUnit = value; }
        }

        #endregion

        #region Overrides of ILGroupWrapper

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new ILNode[] { source.Border, source.Background }));
        }

        #endregion
    }
}
