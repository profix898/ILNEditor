using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILLegendConverter))]
    public class ILLegendWrapper : ILScreenObjectWrapper
    {
        private readonly ILLegend source;

        public ILLegendWrapper(ILLegend source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, ILLegend.LegendTag),
                   String.IsNullOrEmpty(label) ? GetLegendLabel(source, editor.Panel) : label)
        {
            this.source = source;
        }

        #region ILLegend

        [Category("Legend")]
        public float LegendItemSize
        {
            get { return source.LegendItemSize; }
            set { source.LegendItemSize = value; }
        }

        #endregion

        #region Overrides of ILGroupWrapper

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            base.Traverse(null);

            // Add a copy of the legend item to the associated plot item
            if (ILNEditorConfig.LegendItemsLinkToOrigin)
            {
                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (ILLegendItemWrapper legendItemWrapper in Wrappers.Where(node => node is ILLegendItemWrapper).ToList())
                {
                    IILLegendItemDataProvider provider = ((ILLegendItem) legendItemWrapper.Source).GetProvider();
                    if (provider != null)
                    {
                        ILWrapperBase providerWrapper = FindWrapperById(provider.GetID());
                        if (providerWrapper != null)
                            new ILLegendItemWrapper((ILLegendItem) legendItemWrapper.Source, Editor, providerWrapper.Path, legendItemWrapper.Name);
                    }
                }
            }
        }

        #endregion

        #region Helper

        private static string GetLegendLabel(ILLegend source, ILPanel panel)
        {
            if (panel.Scene.Find<ILLegend>().Count() == 1)
                return ILLegend.LegendTag;

            return BuildDefaultName(panel, source, ILLegend.LegendTag);
        }

        #endregion

        #region Nested type: ILLegendConverter

        private class ILLegendConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLegendWrapper)
                    return ((ILLegendWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
