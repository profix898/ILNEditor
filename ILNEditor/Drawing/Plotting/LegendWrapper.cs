using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    [TypeConverter(typeof(LegendConverter))]
    public class LegendWrapper : ScreenObjectWrapper
    {
        private readonly Legend source;
        private readonly LabelWrapper defaultLabelStyle;
        private readonly LegendItemWrapper defaultItemStyle;

        public LegendWrapper(Legend source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, Legend.LegendTag),
                   String.IsNullOrEmpty(label) ? GetLegendLabel(source, editor.Panel) : label)
        {
            this.source = source;

            defaultLabelStyle = new LabelWrapper(source.DefaultLabelStyle ?? new Label(), editor, path, TickCollection.TickLabelTag, "DefaultLabelStyle");
            if (source.DefaultItemStyle != null)
                defaultItemStyle = new LegendItemWrapper(source.DefaultItemStyle, editor, path, LegendItem.LegendItemTag);
        }

        #region Legend

        [Category("Legend")]
        public float LegendItemSize
        {
            get { return source.LegendItemSize; }
            set { source.LegendItemSize = value; }
        }

        [Category("Legend")]
        public float Padding
        {
            get { return source.Padding; }
            set { source.Padding = value; }
        }

        [Category("Legend")]
        public LabelWrapper DefaultLabelStyle
        {
            get { return defaultLabelStyle; }
        }

        [Category("Legend")]
        public LegendItemWrapper DefaultItemStyle
        {
            get { return defaultItemStyle; }
        }

        #endregion

        #region Overrides of GroupWrapper

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            base.Traverse(null);

            // Add a copy of the legend item to the associated plot item
            if (ILNEditorConfig.LegendItemsLinkToOrigin)
            {
                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (LegendItemWrapper legendItemWrapper in Wrappers.Where(node => node is LegendItemWrapper).ToList())
                {
                    ILegendItemDataProvider provider = ((LegendItem) legendItemWrapper.Source).GetProvider();
                    if (provider != null)
                    {
                        WrapperBase providerWrapper = FindWrapperById(provider.GetID());
                        if (providerWrapper != null)
                            new LegendItemWrapper((LegendItem) legendItemWrapper.Source, Editor, providerWrapper.Path, legendItemWrapper.Name);
                    }
                }
            }
        }

        #endregion

        #region Helpers

        private static string GetLegendLabel(Legend source, Panel panel)
        {
            if (panel.Scene.Find<Legend>().Count() == 1)
                return Legend.LegendTag;

            return BuildDefaultName(panel, source, Legend.LegendTag);
        }

        #endregion

        #region Nested type: LegendConverter

        private class LegendConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is LegendWrapper)
                    return ((LegendWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
