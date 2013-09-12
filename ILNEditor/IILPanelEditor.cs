using System;
using System.ComponentModel;

namespace ILNEditor
{
    public interface IILPanelEditor : IDisposable, INotifyPropertyChanged
    {
        bool Visible { get; }

        void Show();

        void Hide();

        void UpdateNodes();

        void SelectNode(string node);
    }
}
