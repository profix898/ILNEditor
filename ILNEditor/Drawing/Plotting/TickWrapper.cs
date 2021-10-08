using System;
using System.ComponentModel;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TickWrapper : WrapperBase
    {
        private readonly LabelWrapper label;
        private readonly Tick source;

        public TickWrapper(Tick source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Tick" : name, label)
        {
            this.source = source;

            this.label = new LabelWrapper(source.Label, editor, path, TickCollection.TickLabelTag);
        }

        #region Tick

        [Category("Format")]
        public bool AutoLabel
        {
            get { return source.AutoLabel; }
            set { source.AutoLabel = value; }
        }

        [Category("Format")]
        public int Level
        {
            get { return source.Level; }
            set { source.Level = value; }
        }

        [Category("Format")]
        public LabelWrapper TickLabel
        {
            get { return label; }
        }

        [Category("Format")]
        public float Position
        {
            get { return source.Position; }
            set { source.Position = value; }
        }

        #endregion
    }
}
