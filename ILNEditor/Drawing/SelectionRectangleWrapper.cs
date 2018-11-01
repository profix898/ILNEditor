using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SelectionRectangleWrapper : GroupWrapper
    {
        private readonly LinesWrapper lines;
        private readonly SelectionRectangle source;

        public SelectionRectangleWrapper(SelectionRectangle source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, SelectionRectangle.LineTag), label)
        {
            this.source = source;

            lines = new LinesWrapper(source.Lines, editor, Path, ScreenObject.BorderTag, "Lines");
        }

        #region ScreenObject

        [Category("SelectionRectangle")]
        public LinesWrapper Lines
        {
            get { return lines; }
        }

        #endregion

        #region Overrides of GroupWrapper

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new Node[] { source.Lines }));
        }

        #endregion
    }
}
