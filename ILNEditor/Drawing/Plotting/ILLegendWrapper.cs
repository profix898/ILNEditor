using System;
using System.ComponentModel;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Drawing.Plotting
{
    internal class ILLegendWrapper : ILScreenObjectWrapper
    {
        private readonly ILLegend source;

        public ILLegendWrapper(ILLegend source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Legend" : name)
        {
            this.source = source;
        }

        [Category("Legend")]
        public float LegendItemSize
        {
            get { return source.LegendItemSize; }
            set { source.LegendItemSize = value; }
        }

        //[Category("Legend")]
        //public IEnumerable<ILLegendItemWrapper> LegendItems
        //{
        //}
    }
}
