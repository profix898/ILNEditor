using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILLegendItemConverter))]
    public class ILLegendItemWrapper : ILGroupWrapper
    {
        private readonly ILLabelWrapper label;
        private readonly ILLegendItem source;

        public ILLegendItemWrapper(ILLegendItem source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILLegendItem.LegendItemTag),
                   String.IsNullOrEmpty(label) ? GetLegendItemLabel(source) : label)
        {
            this.source = source;

            this.label = new ILLabelWrapper(source.Label, editor, Path, ILLegendItem.LabelTag);
        }

        #region ILLegendItem

        [Category("LegendItem")]
        public string Text
        {
            get { return source.Text; }
            set { source.Text = value; }
        }

        [Category("LegendItem")]
        public ILLabelWrapper Label
        {
            get { return label; }
        }

        #endregion

        #region Overrides of ILGroupWrapper

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            // Do not traverse children
        }

        #endregion

        #region Helpers

        private static string GetLegendItemLabel(ILLegendItem source)
        {
            string text = String.IsNullOrEmpty(source.Text)
                              ? (String.IsNullOrEmpty(source.Label.Text) ? "<empty>" : source.Label.Text)
                              : source.Text;

            return String.Format("{0} ('{1}')", ILLegendItem.LegendItemTag, text);
        }

        #endregion

        #region Nested type: ILLegendItemConverter

        private class ILLegendItemConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLegendItemWrapper)
                {
                    var label = (ILLegendItemWrapper) value;

                    return String.Format("{0} ('{1}')", label.Name, label.Label);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
