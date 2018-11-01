using System;
using System.ComponentModel;

namespace ILNEditor
{
    public interface IPanelEditor : IDialog, IDisposable, INotifyPropertyChanged
    {
        IPlotBrowser PlotBrowser { get; }

        void UpdateNodes();

        void SelectNode(string node);
    }
}