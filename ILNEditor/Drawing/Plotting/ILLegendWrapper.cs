using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(ILLegendConverter))]
    internal class ILLegendWrapper : ILScreenObjectWrapper
    {
        private readonly ReadOnlyCollection<ILLegendItemWrapper> legendItems;
        private readonly ILLegend source;

        public ILLegendWrapper(ILLegend source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? ILLegend.LegendTag : name)
        {
            this.source = source;

            legendItems =
                new ReadOnlyCollection<ILLegendItemWrapper>(source.Items.Find<ILLegendItem>().Select(legendItem => new ILLegendItemWrapper(legendItem, editor, FullName)).ToList());
        }

        #region ILLegend

        [Category("Legend")]
        public float LegendItemSize
        {
            get { return source.LegendItemSize; }
            set { source.LegendItemSize = value; }
        }

        [Category("Legend")]
        public ReadOnlyCollection<ILLegendItemWrapper> LegendItems
        {
            get { return legendItems; }
        }

        #endregion

        #region Overrides of ILGroupWrapper

        internal override void Traverse()
        {
            // Background
            ILTrianglesFan backgroundTriangles = source.Find<ILTrianglesFan>(ILScreenObject.BackgroundTag).FirstOrDefault();
            if (backgroundTriangles != null)
                new ILTrianglesWrapper(backgroundTriangles, Editor, FullName, "Background");

            // Add a copy of the legend item to the associated plot item
            if (ILNEditorConfig.LegendItemsLinkToOrigin)
            {
                foreach (ILLegendItemWrapper legendItemWrapper in legendItems)
                {
                    IILLegendItemDataProvider provider = ((ILLegendItem) legendItemWrapper.Source).GetProvider();
                    if (provider != null)
                    {
                        ILWrapperBase providerWrapper = Editor.FindWrapperById(provider.GetID());
                        if (providerWrapper != null)
                            new ILLegendItemWrapper((ILLegendItem) legendItemWrapper.Source, Editor, providerWrapper.FullName, legendItemWrapper.Name);
                    }
                }
            }

            // Do not traverse children
        }

        #endregion

        #region Nested type: ILLegendConverter

        private class ILLegendConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILLegendWrapper)
                    return ((ILLegendWrapper) value).Name;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
