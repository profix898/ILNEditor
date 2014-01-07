using System;
using System.ComponentModel;
using System.Globalization;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILLegendItemConverter))]
    internal class ILLegendItemWrapper : ILGroupWrapper
    {
        private readonly ILLabelWrapper label;
        private readonly ILLegendItem source;

        public ILLegendItemWrapper(ILLegendItem source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? GetLegendItemName(source) : name)
        {
            this.source = source;

            label = new ILLabelWrapper(source.Label, editor, FullName, ILLegendItem.LabelTag);
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

        #region Helper

        private static string GetLegendItemName(ILLegendItem source)
        {
            return String.Format("{0}(\"{1}\")", ILLegendItem.LegendItemTag, source.Text);
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

                    return String.Format("{0} ('{1}')", label.Name, label.Text);
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
