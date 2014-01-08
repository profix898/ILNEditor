using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class ILSelectionRectangleWrapper : ILGroupWrapper
    {
        private readonly ILLinesWrapper lines;
        private readonly ILSelectionRectangle source;

        public ILSelectionRectangleWrapper(ILSelectionRectangle source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILSelectionRectangle.LineTag), label)
        {
            this.source = source;

            lines = new ILLinesWrapper(source.Lines, editor, Path, ILScreenObject.BorderTag, "Lines");
        }

        #region ILScreenObject

        [Category("SelectionRectangle")]
        public ILLinesWrapper Lines
        {
            get { return lines; }
        }

        #endregion

        #region Overrides of ILGroupWrapper

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse((nodes ?? source.Children).Except(new ILNode[] { source.Lines }));
        }

        #endregion
    }
}
