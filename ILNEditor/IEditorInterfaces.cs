using System;
using System.ComponentModel;

namespace ILNEditor
{
    public interface IDialog
    {
        bool Visible { get; }

        void Show();

        void Hide();
    }

    public interface IPanelEditor : IDialog, IDisposable, INotifyPropertyChanged
    {
        IPlotBrowser PlotBrowser { get; }

        void UpdateNodes();

        void SelectNode(string node);
    }

    public interface IPlotBrowser : IDialog
    {
        void UpdateList();
    }
}
