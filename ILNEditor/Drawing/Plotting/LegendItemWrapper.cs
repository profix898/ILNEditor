using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(LegendItemConverter))]
    public class LegendItemWrapper : GroupWrapper
    {
        private readonly LabelWrapper label;
        private readonly LegendItem source;

        public LegendItemWrapper(LegendItem source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, LegendItem.LegendItemTag),
                   String.IsNullOrEmpty(label) ? GetLegendItemLabel(source) : label)
        {
            this.source = source;

            this.label = new LabelWrapper(source.Label, editor, Path, LegendItem.LabelTag);
        }

        #region LegendItem

        [Category("LegendItem")]
        public string Text
        {
            get { return source.Text; }
            set { source.Text = value; }
        }

        [Category("LegendItem")]
        public new LabelWrapper Label
        {
            get { return label; }
        }

        #endregion

        #region Overrides of GroupWrapper

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            // Do not traverse children
        }

        #endregion

        #region Helpers

        private static string GetLegendItemLabel(LegendItem source)
        {
            string text = String.IsNullOrEmpty(source.Text)
                              ? (String.IsNullOrEmpty(source.Label.Text) ? "<empty>" : source.Label.Text)
                              : source.Text;

            return $"{LegendItem.LegendItemTag} ('{text}')";
        }

        #endregion

        #region Nested type: LegendItemConverter

        private class LegendItemConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is LegendItemWrapper)
                {
                    var label = (LegendItemWrapper) value;

                    return $"{label.Name} ('{label.Label}')";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
